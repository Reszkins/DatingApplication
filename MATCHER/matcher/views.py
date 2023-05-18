from flask import Blueprint, request, jsonify
from .utils import find_matches

blueprint = Blueprint("matches", __name__, url_prefix='/')

@blueprint.route('/matches')
def matches():
    user_id = request.args.get('user_id', type=int)
    num_matches = request.args.get('num_matches', 10, type=int)
    matches = find_matches(user_id, num_matches)
    return jsonify(matches)