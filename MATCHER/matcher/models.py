from .database import Base
from sqlalchemy import Column, ForeignKey, Integer, String
from sqlalchemy.orm import relationship
from sqlalchemy.dialects.postgresql import JSONB

class User(Base):
    __tablename__ = 'users_matching_info'

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users_account.id'))
    age = Column(Integer)
    gender = Column(String)
    sexuality = Column(String)
    education_level = Column(Integer)
    relationship_type = Column(Integer)
    want_children = Column(Integer)
    love_languages_rating = Column(JSONB)
    big_five_traits_rating = Column(JSONB)
    attachment_style = Column(Integer)
    values_and_beliefs_rating = Column(JSONB)

    questionnaire = relationship('Questionnaire', back_populates='user', uselist=False)
    
class Questionnaire(Base):
    __tablename__ = 'questionnaires'

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users_matching_info.id'), unique=True)
    q1 = Column(Integer)
    q2 = Column(Integer)
    q3 = Column(Integer)
    q4 = Column(Integer)
    q5 = Column(Integer)
    q6 = Column(Integer)

    user = relationship('User', back_populates='questionnaire')

class UserBehavior(Base):
    __tablename__ = 'users_behavior'

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users_account.id'))
    target_user_id = Column(Integer, ForeignKey('users_account.id'))
    rating = Column(Integer)
