from flask import Blueprint, request, abort, jsonify, make_response, current_app
from .compatibility import find_matches
from .user_generator import generate_random_users, generate_random_ratings
from .exceptions import ParameterError
from .svd import update_svd_model

blueprint = Blueprint("views", __name__, url_prefix='/')

@blueprint.route('/matches')
def matches():
    user_id = request.args.get('user_id', type=int)
    num_matches = request.args.get('num_matches', 10, type=int)
    try:
        if not user_id:
            raise ParameterError("Missing user_id parameter")
        matches = find_matches(user_id, num_matches)
    except ParameterError as e:
        abort(400, description=str(e))

    return jsonify(matches)

@blueprint.route('/update_model')
def update_model():
    succeeded = update_svd_model(current_app)
    if not succeeded:
        return make_response(f'Failed to update model', 409)
    return make_response('Model updated successfully', 200)

@blueprint.route('/generate_users')
def generate_users():
    num_users = request.args.get('num_users', 1, type=int)
    generated_user_ids = generate_random_users(num_users)
    return jsonify(generated_user_ids)

@blueprint.route('/generate_ratings')
def generate_ratings():
    num_ratings = request.args.get('num_ratings', 1, type=int)
    try:
        generated_ratings = generate_random_ratings(num_ratings)
    except ParameterError as e:
        abort(400, description=str(e))
    return jsonify(generated_ratings)