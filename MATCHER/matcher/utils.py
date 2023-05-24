from flask import current_app as app
import pandas as pd
import numpy as np

from .database import db
from .models import User
from .svd import svd
from .exceptions import ParameterError

weights = {
    'collaborative_filtering': 0.5,
    'questionnaire_correlation': 0.5
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

        combined_score = weights['collaborative_filtering'] * prediction_score + \
            weights['questionnaire_correlation'] * questionnaire_score

        compatibility_scores.append((target_user_id, combined_score))

    compatibility_scores.sort(key=lambda x: x[1], reverse=True)
    top_matches = compatibility_scores[:num_matches]

    matches = []
    for targer_user_id, score in top_matches:
        matches.append({'user_id': user_id, 'target_user_id': targer_user_id, 'score': score})
    return matches

def normalize_user_data(user_data):
    return user_data

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