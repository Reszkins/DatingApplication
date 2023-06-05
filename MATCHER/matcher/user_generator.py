from faker import Faker
from itertools import permutations
import random

from .models import UserAccount, UserBaseInfo, UserMatchingInfo, UserBehavior
from .database import db
from .exceptions import ParameterError

fake = Faker()

def generate_random_user_matching_info(user_id):
    matching_info = UserMatchingInfo()

    matching_info.user_id = user_id

    matching_info.want_children = random.randint(1, 5)
    matching_info.relationship_type = random.randint(1, 5)
    matching_info.love_languages_words = random.randint(1, 5)
    matching_info.love_languages_acts = random.randint(1, 5)
    matching_info.love_languages_gifts = random.randint(1, 5)
    matching_info.love_languages_quality_time = random.randint(1, 5)
    matching_info.love_languages_touch = random.randint(1, 5)
    matching_info.big_five_openness = random.randint(1, 5)
    matching_info.big_five_conscientiousness = random.randint(1, 5)
    matching_info.big_five_extraversion = random.randint(1, 5)
    matching_info.big_five_agreeableness = random.randint(1, 5)
    matching_info.big_five_neuroticism = random.randint(1, 5)
    matching_info.values_beliefs_religious = random.randint(1, 5)
    matching_info.values_beliefs_political = random.randint(1, 5)
    matching_info.values_beliefs_family = random.randint(1, 5)
    matching_info.values_beliefs_career = random.randint(1, 5)
    matching_info.openness_conversation = random.randint(1, 5)
    matching_info.time_together = random.randint(1, 5)
    matching_info.physical_closeness = random.randint(1, 5)
    matching_info.new_challenges = random.randint(1, 5)
    matching_info.shared_interests = random.randint(1, 5)
    matching_info.personal_space = random.randint(1, 5)

    return matching_info

sexualities = ['homosexual', 'heterosexual', 'bisexual']
genders = ['male', 'female']

def generate_random_users(num_users):
    users = []
    for _ in range(num_users):
        user_account = UserAccount(email=fake.email())
        db.session.add(user_account)
        db.session.flush()
        user_id = user_account.id
        gender = genders[random.randint(0, 1)]
        user_base_info = UserBaseInfo(user_id=user_id, \
                                      first_name=(fake.first_name_male() if gender == 'male' else fake.first_name_female()), \
                                      second_name=fake.last_name(), \
                                      date_of_birth=fake.date_of_birth(minimum_age=18, maximum_age=70), \
                                      gender=gender, \
                                      sexuality=sexualities[random.randint(0, 2)], \
                                      description=fake.paragraph(nb_sentences=1))
        db.session.add(user_base_info)
        db.session.flush()
        users.append(user_base_info.json())
        user_matching_info = generate_random_user_matching_info(user_id)
        db.session.add(user_matching_info)
        db.session.flush()
    db.session.commit()
    return users

def generate_random_ratings(num_ratings):
    user_ids = [id[0] for id in db.session.query(UserAccount.id).all()]
    possible_ratings = list(permutations(user_ids, 2))
    possible_ratings = [(user1, user2) for user1, user2 in possible_ratings if user1 != user2]
    random.shuffle(possible_ratings)
    existing_ratings = [(rating.user_id, rating.target_user_id) for rating in UserBehavior.query.all()]
    possible_ratings = [rating for rating in possible_ratings if rating not in existing_ratings]
    ratings = []
    if num_ratings > len(possible_ratings):
        raise ParameterError("Cannot generate more unique ratings than possible pairs of users.")
    for _ in range(num_ratings):
        user_id, target_user_id = possible_ratings.pop()
        rating = random.randint(1, 5)
        new_rating = UserBehavior(user_id=user_id, target_user_id=target_user_id, rating=rating)
        db.session.add(new_rating)
        db.session.flush()
        ratings.append(new_rating.json())
    db.session.commit()
    return ratings