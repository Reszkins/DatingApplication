from .database import Base
from sqlalchemy import Column, ForeignKey, Integer, String
from sqlalchemy.orm import relationship
from sqlalchemy.ext.hybrid import hybrid_property
from sqlalchemy.dialects.postgresql import DATE
from datetime import date

class User(Base):
    __tablename__ = 'users_base_info'

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users_account.id'))
    gender = Column(String)
    sexuality = Column(String)
    date_of_birth = Column(DATE)

    matching_info = relationship('UserMatchingInfo', back_populates='user')

class UserMatchingInfo(Base):
    __tablename__ = 'users_matching_info'

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users_base_info.id'))
    want_children = Column(Integer)
    relationship_type = Column(Integer)
    love_languages_words = Column(Integer)
    love_languages_acts = Column(Integer)
    love_languages_gifts = Column(Integer)
    love_languages_quality_time = Column(Integer)
    love_languages_touch = Column(Integer)
    big_five_openness = Column(Integer)
    big_five_conscientiousness = Column(Integer)
    big_five_extraversion = Column(Integer)
    big_five_agreeableness = Column(Integer)
    big_five_neuroticism = Column(Integer)
    values_beliefs_religious = Column(Integer)
    values_beliefs_political = Column(Integer)
    values_beliefs_family = Column(Integer)
    values_beliefs_career = Column(Integer)
    openness_conversation = Column(Integer)
    time_together = Column(Integer)
    physical_closeness = Column(Integer)
    new_challenges = Column(Integer)
    shared_interests = Column(Integer)
    personal_space = Column(Integer)

    user = relationship('User', back_populates='matching_info')

    @hybrid_property
    def age(self):
        today = date.today()
        return today.year - self.user.date_of_birth.year - ((today.month, today.day) < (self.user.date_of_birth.month, self.user.date_of_birth.day))
    
    @hybrid_property
    def gender(self):
        return self.user.gender
    
    @hybrid_property
    def sexuality(self):
        return self.user.sexuality

class UserBehavior(Base):
    __tablename__ = 'users_behavior'

    id = Column(Integer, primary_key=True)
    user_id = Column(Integer, ForeignKey('users_account.id'))
    target_user_id = Column(Integer, ForeignKey('users_account.id'))
    rating = Column(Integer)
