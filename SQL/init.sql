CREATE TABLE users_account
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  email VARCHAR(100),
  password_hash BYTEA,
  password_salt BYTEA
);

CREATE TABLE users_base_info
(
  user_id INT,
  first_name VARCHAR(50),
  second_name VARCHAR(50),
  gender VARCHAR(10),
  date_of_birth DATE,
  description VARCHAR(1000),
  CONSTRAINT fk_user_base_info
	FOREIGN KEY(user_id)
	  REFERENCES users_account(id)
);

CREATE TABLE messages
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  from_user INT,
  to_user INT,
  message_text VARCHAR(10000),
  sent_timestamp BIGINT,
  CONSTRAINT fk_from_user
	FOREIGN KEY(from_user)
	  REFERENCES users_account(id),
  CONSTRAINT fk_to_user
	FOREIGN KEY(to_user)
	  REFERENCES users_account(id)
);

CREATE TABLE users_dislikes
(
  user_id INT,
  disliked_user_id INT,
  CONSTRAINT fk_user
	FOREIGN KEY(user_id)
	  REFERENCES users_account(id),
  CONSTRAINT fk_disliked_user
	FOREIGN KEY(disliked_user_id)
	  REFERENCES users_account(id)
);

CREATE TABLE users_matching_info
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  age INT,
  gender VARCHAR(10),
  sexuality VARCHAR(50),
  education_level INT,
  want_children INT,
  relationship_type INT,
  attachment_style INT,
  love_languages_rating JSONB,
  big_five_traits_rating JSONB,
  values_and_beliefs_rating JSONB,
  CONSTRAINT fk_user
  FOREIGN KEY(user_id)
    REFERENCES users_account(id)
);

CREATE TABLE users_behavior
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  target_user_id INT,
  rating INT,
  CONSTRAINT fk_user
  FOREIGN KEY(user_id)
    REFERENCES users_account(id),
  CONSTRAINT fk_target_user
  FOREIGN KEY(target_user_id)
    REFERENCES users_account(id),
  CONSTRAINT ckeck_rating
  CHECK (rating >= 1 AND rating <= 5)
);

CREATE TABLE questionnaires
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  user_id INT,
  q1 INT,
  q2 INT,
  q3 INT,
  q4 INT,
  q5 INT,
  q6 INT,
  CONSTRAINT fk_user
  FOREIGN KEY(user_id)
    REFERENCES users_account(id)
);