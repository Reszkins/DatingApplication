from flask import current_app as app
from scipy.spatial.distance import cosine

from .models import UserMatchingInfo, UserAccount, UserBaseInfo, UserBehavior
from .svd import svd
from .exceptions import ParameterError

weights = {
    'collaborative_filtering': 0.1,
    'age_compatibility': 0.15,
    'trait_compatibility': 0.4,
    'want_children': 0.1,
    'relationship_type': 0.25
}

def find_matches(user_id, num_matches):
    user_account = UserAccount.query.get(user_id)
    if not user_account:
        raise ParameterError(f"No user with [id]={user_id}")
    user = UserMatchingInfo.query.get(user_id)
    if not user or not UserBaseInfo.query.get(user_id):
        raise ParameterError(f"No base/matching info for user with [id]={user_id}")    
    
    rated_user_ids = [rating.target_user_id for rating in UserBehavior.query.filter(UserBehavior.user_id == user_id).all()]
    users_not_rated_by_user = UserAccount.query.filter(UserAccount.id != user_id, ~UserAccount.id.in_(rated_user_ids)).all()
    user_ids = [x.id for x in users_not_rated_by_user]

    compatibility_scores = []
    for target_user_id in user_ids:
        target_user = UserMatchingInfo.query.get(target_user_id)

        if not is_sexually_compatible(user, target_user):
            continue

        prediction_score = collaborative_filtering(user_id, target_user_id)
        age_score = age_compatibility(user, target_user)
        trait_compatibility_score = trait_compatibility(user, target_user)

        combined_score = weights['collaborative_filtering'] * prediction_score + \
            weights['age_compatibility'] * age_score + \
            weights['trait_compatibility'] * trait_compatibility_score + \
            weights['want_children'] * (1 if user.want_children == target_user.want_children else 0) + \
            weights['relationship_type'] * (1 if user.relationship_type == target_user.relationship_type else 0)
        
        compatibility_scores.append((target_user_id, combined_score))

    compatibility_scores.sort(key=lambda x: x[1], reverse=True)
    top_matches = compatibility_scores[:num_matches]

    matches = []
    for targer_user_id, score in top_matches:
        matches.append({'user_id': user_id, 'target_user_id': targer_user_id, 'score': score})
    return matches

def age_compatibility(user: UserMatchingInfo, target_user: UserMatchingInfo):
    age_difference = abs(user.age - target_user.age)
    return 1.0 - (age_difference / max(user.age, target_user.age))

def love_languages_vector(user: UserMatchingInfo):
    return [user.love_languages_acts, user.love_languages_gifts, user.love_languages_quality_time,
            user.love_languages_touch, user.love_languages_words]

def big_five_vector(user: UserMatchingInfo):
    return [user.big_five_agreeableness, user.big_five_conscientiousness, user.big_five_extraversion,
            user.big_five_neuroticism, user.big_five_openness]

def values_beliefs_vector(user: UserMatchingInfo):
    return [user.values_beliefs_career, user.values_beliefs_family, user.values_beliefs_political,
            user.values_beliefs_religious]

def trait_compatibility(user: UserMatchingInfo, target_user: UserMatchingInfo):
    love_languages_similarity = cosine_similarity(love_languages_vector(user), love_languages_vector(target_user))
    big_five_traits_similarity = cosine_similarity(big_five_vector(user), big_five_vector(target_user))
    values_beliefs_similarity = cosine_similarity(values_beliefs_vector(user), values_beliefs_vector(target_user))
    trait_compatibility_score = (love_languages_similarity + big_five_traits_similarity + values_beliefs_similarity) / 3
    return trait_compatibility_score

def cosine_similarity(traits1, traits2):
    return 1 - cosine(traits1, traits2)

def collaborative_filtering(user_id, target_user_id):
    if (svd.model == None):
        app.logger.warn("Skipping callaborative_filtering, model is not initialized")
        return 0
    prediction = svd.model.predict(user_id, target_user_id).est
    normalized_prediction = (prediction - 1) / 4
    return normalized_prediction

def is_sexually_compatible(user: UserMatchingInfo, target_user: UserMatchingInfo):
    if user.sexuality == 'heterosexual':
        if target_user.sexuality == 'heterosexual' and user.gender != target_user.gender:
            return True
        if target_user.sexuality == 'bisexual' and user.gender != target_user.gender:
            return True
    elif user.sexuality == 'homosexual':
        if target_user.sexuality == 'homosexual' and user.gender == target_user.gender:
            return True
        if target_user.sexuality == 'bisexual' and user.gender == target_user.gender:
            return True
    elif user.sexuality == 'bisexual':
        if (target_user.sexuality == 'heterosexual' and user.gender != target_user.gender) or \
           (target_user.sexuality == 'homosexual' and user.gender == target_user.gender) or \
           target_user.sexuality == 'bisexual':
            return True
    return False