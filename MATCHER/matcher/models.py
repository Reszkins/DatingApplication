from .database import Base
from sqlalchemy import Column, ForeignKey, Integer, String
from sqlalchemy.orm import relationship
from sqlalchemy.ext.hybrid import hybrid_property
from sqlalchemy.dialects.postgresql import DATE, TEXT
from datetime import date

class UserAccount(Base):
    __tablename__ = 'users_account'

    id = Column(Integer, primary_key=True)
    email = Column(String)

class UserBaseInfo(Base):
    __tablename__ = 'users_base_info'

    user_id = Column(Integer, ForeignKey('users_account.id'), primary_key=True)
    gender = Column(String)
    sexuality = Column(String)
    date_of_birth = Column(DATE)
    description = Column(TEXT)
    first_name = Column(String)
    second_name = Column(String)

    account = relationship('UserAccount')

    def json(self):
        return {
            "user_id": self.user_id,
            "email": self.account.email,
            "first_name": self.first_name,
            "second_name": self.second_name,
            "gender": self.gender,
            "sexuality": self.sexuality,
            "date_of_birth": self.date_of_birth,
            "description": self.description
        }

class UserMatchingInfo(Base):
    __tablename__ = 'users_matching_info'

    user_id = Column(Integer, ForeignKey('users_base_info.user_id'), primary_key=True)
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

    user = relationship('UserBaseInfo')

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
