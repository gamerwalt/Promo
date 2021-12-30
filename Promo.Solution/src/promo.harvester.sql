CREATE DATABASE 'promo.harvester' /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
CREATE TABLE 'harvested' (
  'harvested_id' int unsigned NOT NULL AUTO_INCREMENT,
  'rule_uid' varchar(45) NOT NULL,
  'harvested_uid' varchar(50) NOT NULL,
  'harvested_value' varchar(8000) NOT NULL,
  'created_date' datetime NOT NULL,
  'updated_date' datetime DEFAULT NULL,
  PRIMARY KEY ('harvested_id'),
  UNIQUE KEY 'id_UNIQUE' ('harvested_id'),
  UNIQUE KEY 'harvested_uid_UNIQUE' ('harvested_uid')
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE 'tweets' (
  'tweet_id' int unsigned NOT NULL AUTO_INCREMENT,
  'harvested_id' int unsigned NOT NULL,
  'user_id' int unsigned NOT NULL,
  'tweet_uid' varchar(50) NOT NULL,
  'external_id' varchar(100) NOT NULL,
  'body' text NOT NULL,
  'retweeted' bit(1) NOT NULL DEFAULT b'0',
  'external_created_at' datetime NOT NULL,
  'created_date' datetime NOT NULL,
  PRIMARY KEY ('tweet_id'),
  UNIQUE KEY 'tweet_id_UNIQUE' ('tweet_id'),
  UNIQUE KEY 'tweet_uid_UNIQUE' ('tweet_uid'),
  UNIQUE KEY 'external_id_UNIQUE' ('external_id'),
  KEY 'users_tweets_idx' ('user_id'),
  KEY 'harvested_tweets_idx' ('harvested_id'),
  CONSTRAINT 'harvested_tweets' FOREIGN KEY ('harvested_id') REFERENCES 'harvested' ('harvested_id'),
  CONSTRAINT 'users_tweets' FOREIGN KEY ('user_id') REFERENCES 'users' ('user_id')
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE 'users' (
  'user_id' int unsigned NOT NULL AUTO_INCREMENT,
  'user_uid' varchar(50) NOT NULL,
  'external_id' varchar(100) NOT NULL,
  'profile_image_url' varchar(5000) NOT NULL,
  'verified_ind' bit(1) NOT NULL DEFAULT b'0',
  'location' varchar(150) NOT NULL,
  'description' varchar(500) NOT NULL,
  'screen_name' varchar(100) NOT NULL,
  'created_date' datetime NOT NULL,
  'updated_date' datetime DEFAULT NULL,
  PRIMARY KEY ('user_id'),
  UNIQUE KEY 'user_id_UNIQUE' ('user_id'),
  UNIQUE KEY 'user_uid_UNIQUE' ('user_uid'),
  UNIQUE KEY 'external_id_UNIQUE' ('external_id')
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE PROCEDURE 'harvested_record_create'(IN harvestedUid VARCHAR(50), IN ruleUid VARCHAR(50), IN harvestedValue VARCHAR(8000))
BEGIN
	INSERT INTO harvested
    (
		harvested_uid,
        rule_uid,
        harvested_value,
        created_date
    )
    SELECT
		harvestedUid,
        ruleUid,
        harvestedValue,
        CURRENT_TIMESTAMP();




CREATE PROCEDURE 'tweet_create'(IN tweetUid VARCHAR(50), IN harvestedUid VARCHAR(50), IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN body TEXT, IN retweeted BIT, IN externalCreatedAt DATETIME)
BEGIN
   
	DECLARE harvestedId INT DEFAULT 0;
    DECLARE userId INT DEFAULT 0;
    
    SELECT @harvestedId := harvested_id FROM harvested WHERE harvested_uid = harvestedUid;
    SELECT @userId := user_id FROM users  WHERE user_uid = userUid;
    
    IF(@harvestedId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Harvested data not found';
	END IF;
    IF(@userId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'User not found';
	END IF;
    
    INSERT INTO tweets
    (
		harvested_id,
        user_id,
        tweet_uid,
        external_id,
        body,
        retweeted,
        external_created_at,
        created_date
    )
    VALUES
    (
		@harvestedId,
        @userId,
        tweetUid,
        externalId,
        body,
        retweeted,
        externalCreatedAt,
        CURRENT_TIMESTAMP()
    );




CREATE PROCEDURE 'tweet_upsert'(IN tweetUid VARCHAR(50), IN harvestedUid VARCHAR(50), IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN body TEXT, IN retweeted BIT, IN externalCreatedAt DATETIME)
BEGIN
	
	DECLARE harvestedId INT DEFAULT 0;
    DECLARE userId INT DEFAULT 0;
    
    SELECT @harvestedId := harvested_id FROM harvested WHERE harvested_uid = harvestedUid;
    SELECT @userId := user_id FROM users  WHERE user_uid = userUid;
    
    IF(@harvestedId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Harvested data not found';
	END IF;
    IF(@userId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'User not found';
	END IF;
    
    INSERT IGNORE  INTO tweets
    (
		harvested_id,
        user_id,
        tweet_uid,
        external_id,
        body,
        retweeted,
        external_created_at,
        created_date
    )
    VALUES
    (
		@harvestedId,
        @userId,
        tweetUid,
        externalId,
        body,
        retweeted,
        externalCreatedAt,
        CURRENT_TIMESTAMP()
    );
		




CREATE PROCEDURE 'tweet_user_upsert'(IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN profileImageUrl VARCHAR(5000), IN verifiedInd BIT, IN location VARCHAR(150), IN description VARCHAR(500), IN screenName VARCHAR(100))
BEGIN
	INSERT INTO users
    (
		user_uid,
        external_id,
        profile_image_url,
        verified_ind,
        location,
        description,
        screen_name,
        created_date
    )
    VALUES
    (
		userUid,
        externalId,
        profileImageUrl,
        verifiedInd,
        location,
        description,
        screenName,
        CURRENT_TIMESTAMP()
    )
    ON DUPLICATE KEY UPDATE
	  user_uid = userUid,
	  profile_image_url = profileImageUrl,
      verified_ind = verifiedInd,
      location = location,
      description = description,
      screen_name = screenName,
      updated_date = CURRENT_TIMESTAMP();

