CREATE TABLE users_account
(
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  email VARCHAR(100),
  password_hash BYTEA,
  password_salt BYTEA
);

CREATE TABLE users_base_info
(
  user_id INT PRIMARY KEY,
  first_name VARCHAR(50),
  second_name VARCHAR(50),
  gender VARCHAR(10),
  sexuality VARCHAR(50),
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

CREATE TABLE users_matching_info
(
  user_id INT PRIMARY KEY,
  want_children INT,
  relationship_type INT,
  love_languages_words INT,
  love_languages_acts INT,
  love_languages_gifts INT,
  love_languages_quality_time INT,
  love_languages_touch INT,
  big_five_openness INT,
  big_five_conscientiousness INT,
  big_five_extraversion INT,
  big_five_agreeableness INT,
  big_five_neuroticism INT,
  values_beliefs_religious INT,
  values_beliefs_political INT,
  values_beliefs_family INT,
  values_beliefs_career INT,
  openness_conversation INT,
  time_together INT,
  physical_closeness INT,
  new_challenges INT,
  shared_interests INT,
  personal_space INT,
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

CREATE TABLE questions (
  id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  question_text TEXT NOT NULL,
  matching_info_column_name VARCHAR(30) NOT NULL,
  question_number INT UNIQUE NOT NULL
);

INSERT INTO questions (question_text, matching_info_column_name, question_number)
VALUES
  ('Jak ważne są dla Ciebie słowa uznania?', 'love_languages_words', 1),
  ('Jak ważne są dla Ciebie wsparcie w codziennych zadaniach?', 'love_languages_acts', 2),
  ('Jak ważne jest dla Ciebie częste otrzymywanie prezentów?', 'love_languages_gifts', 3),
  ('Jak ważne jest dla Ciebie wspólne spędzanie czasu w sposób angażujący ("quality time")?', 'love_languages_quality_time', 4),
  ('Jak ważny jest dla Ciebie kontakt fizyczny?', 'love_languages_touch', 5),
  ('Jak otwarty jesteś na nowe doświadczenia?', 'big_five_openness', 6),
  ('Czy uważasz się za osobę sumienną?', 'big_five_conscientiousness', 7),
  ('Czy jesteś osobą ekstrawertyczną?', 'big_five_extraversion', 8),
  ('Jak bardzo ugodową osobą jesteś?', 'big_five_agreeableness', 9),
  ('Czy masz tendendcję do posiadania złego humoru?', 'big_five_neuroticism', 10),
  ('Jak ważne są dla Ciebie przekonania religijne?', 'values_beliefs_religious', 11),
  ('Jak ważne są dla Ciebie przekonania polityczne?', 'values_beliefs_political', 12),
  ('Jak ważna jest dla Ciebie rodzina?', 'values_beliefs_family', 13),
  ('Jak ważny jest dla Ciebie rozwój kariery?', 'values_beliefs_career', 14),
  ('Jak często chcesz prowadzić szczere i otwarte rozmowy w związku?', 'openness_conversation', 15),
  ('Jak często chciałbyś spędzać czas razem jako para?', 'time_together', 16),
  ('Jak istotne dla Ciebie jest okazywanie fizycznej bliskości (np. przytulanie, trzymanie za ręce) w związku?', 'physical_closeness', 17),
  ('Jak bardzo jesteś skłonny do podejmowania nowych i ekscytujących wyzwań?', 'new_challenges', 18),
  ('Jak ważne jest dla Ciebie, aby Twój partner i Ty mieli wspólne zainteresowania i hobby?', 'shared_interests', 19),
  ('Jak bardzo potrzebujesz czasu i przestrzeni dla siebie w związku?', 'personal_space', 20),
  ('Jak ważne jest dla Ciebie posiadanie dzieci?', 'want_children', 21),
  ('Jak bardzo poważnego związku szukasz?', 'relationship_type', 22)
ON CONFLICT (question_number) DO NOTHING;