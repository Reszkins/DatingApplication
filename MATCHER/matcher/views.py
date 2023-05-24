from flask import Blueprint, request, abort, jsonify
from .utils import find_matches
from .exceptions import ParameterError

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