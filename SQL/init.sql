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
