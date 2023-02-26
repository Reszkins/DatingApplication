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
  CONSTRAINT fk_user_base_info
	FOREIGN KEY(user_id)
	  REFERENCES users_account(id)
);