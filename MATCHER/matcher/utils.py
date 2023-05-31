from flask import current_app as app
import numpy as np
from scipy.spatial.distance import cosine

from .models import User
from .svd import svd
from .exceptions import ParameterError

weights = {
    'collaborative_filtering': 0.1,
    'questionnaire_correlation': 0.3,
    'age_compatibility': 0.2,
    'trait_compatibility': 0.4
}

def find_matches(user_id, num_matches):
    user = User.query.get(user_id)
    if not user:
        raise ParameterError(f"No user with [id]={user_id}")
    all_users = User.query.all()
    user_ids = [user.id for user in all_users if user.id != user_id]
    
    compatibility_scores = []
    for target_user_id in user_ids:
        target_user = User.query.get(target_user_id)

        if not is_sexually_compatible(user, target_user):
            continue
        
        prediction_score = collaborative_filtering(user_id, target_user_id)
        questionnaire_score = questionnaire_correlation(user, target_user)
        age_score = age_compatibility(user, target_user)
        trait_compatibility_score = trait_compatibility(user, target_user)

        combined_score = weights['collaborative_filtering'] * prediction_score + \
            weights['questionnaire_correlation'] * questionnaire_score + \
            weights['age_compatibility'] * age_score + \
            weights['trait_compatibility'] * trait_compatibility_score

        compatibility_scores.append((target_user_id, combined_score))

    compatibility_scores.sort(key=lambda x: x[1], reverse=True)
    top_matches = compatibility_scores[:num_matches]

    matches = []
    for targer_user_id, score in top_matches:
        matches.append({'user_id': user_id, 'target_user_id': targer_user_id, 'score': score})
    return matches

def age_compatibility(user: User, target_user: User):
    age_difference = abs(user.age - target_user.age)
    return 1.0 - (age_difference / max(user.age, target_user.age))

def trait_compatibility(user: User, target_user: User):
    compatibility_score = 0
    
    if user.education_level == target_user.education_level:
        compatibility_score += 1.0
    
    if user.want_children == target_user.want_children:
        compatibility_score += 1.0
    
    if user.relationship_type == target_user.relationship_type:
        compatibility_score += 1.0
    
    if user.attachment_style == target_user.attachment_style:
        compatibility_score += 1.0
    
    love_languages_similarity = cosine_similarity(user.love_languages_rating, target_user.love_languages_rating)
    big_five_traits_similarity = cosine_similarity(user.big_five_traits_rating, target_user.big_five_traits_rating)
    values_beliefs_similarity = cosine_similarity(user.values_and_beliefs_rating, target_user.values_and_beliefs_rating)
    
    compatibility_score += (love_languages_similarity + big_five_traits_similarity + values_beliefs_similarity) / 3
    
    return compatibility_score

def cosine_similarity(traits1, traits2):
    return 1 - cosine(list(traits1.values), list(traits2.values))

def collaborative_filtering(user_id, target_user_id):
    if (svd.model == None):
        app.logger.warn("Skipping callaborative_filtering, model is not initialized")
        return 0
    prediction = svd.model.predict(user_id, target_user_id).est
    normalized_prediction = (prediction - 1) / 4
    return normalized_prediction

def questionnaire_correlation(user: User, targer_user: User):
    user_answers = [ user.questionnaire.q1, user.questionnaire.q2, user.questionnaire.q3,
                    user.questionnaire.q4, user.questionnaire.q5, user.questionnaire.q6 ]
    target_user_answers = [ targer_user.questionnaire.q1, targer_user.questionnaire.q2, targer_user.questionnaire.q3,
                    targer_user.questionnaire.q4, targer_user.questionnaire.q5, targer_user.questionnaire.q6 ]
    correlation_coefficient = np.corrcoef(user_answers, target_user_answers)[0][1]
    normalized_questionnaire_correlation = (correlation_coefficient + 1) / 2
    return normalized_questionnaire_correlation

def is_sexually_compatible(user, target_user):
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