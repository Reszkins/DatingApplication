import logging
from flask import Flask
from sqlalchemy.orm import scoped_session, sessionmaker
from apscheduler.schedulers.background import BackgroundScheduler

from .database import db, Base
from .views import blueprint as views_blueprint

SQLALCHEMY_DATABASE_URI = "postgresql+psycopg2://admin:admin@postgresql_database/ApplicationDatabase"

scheduler = BackgroundScheduler(daemon=True)

def create_app():
    app = Flask(__name__)
    app.config['SQLALCHEMY_DATABASE_URI'] = SQLALCHEMY_DATABASE_URI
    app.register_blueprint(views_blueprint)
    app.logger.setLevel(logging.INFO)
    initialize_db(app)
    initialize_svd_model(app)
    return app

def initialize_db(app: Flask):
    from .models import User, UserBehavior, UserMatchingInfo
    db.init_app(app)
    with app.app_context():
        db_session = scoped_session(sessionmaker(db.engine))
        db.session = db_session
        Base.query = db_session.query_property()
        Base.metadata.create_all(db.engine)

def initialize_svd_model(app: Flask):
    from .svd import update_svd_model
    update_svd_model(app)
    scheduler.add_job(lambda: update_svd_model(app), 'interval', minutes=15)
    scheduler.start()