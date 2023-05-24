from flask import Blueprint, request, abort, jsonify, make_response, current_app
from .utils import find_matches
from .exceptions import ParameterError
from .svd import update_svd_model

blueprint = Blueprint("matches", __name__, url_prefix='/')

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
