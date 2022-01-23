use `promo.harvester`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `harvested_record_create`(IN harvestedUid VARCHAR(50), IN ruleUid VARCHAR(50), IN harvestedValue VARCHAR(8000))
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `tweet_create`(IN tweetUid VARCHAR(50), IN harvestedUid VARCHAR(50), IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN body TEXT, IN retweeted BIT, IN externalCreatedAt DATETIME)
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `tweet_upsert`(IN tweetUid VARCHAR(50), IN harvestedUid VARCHAR(50), IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN body TEXT, IN retweeted BIT, IN externalCreatedAt DATETIME)
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
		
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `tweet_user_upsert`(IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN profileImageUrl VARCHAR(5000), IN verifiedInd BIT, IN location VARCHAR(150), IN description VARCHAR(500), IN screenName VARCHAR(100))
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `users_get_by_screen_name`(IN userIdentifier VARCHAR(100))
BEGIN
	SELECT *
    FROM users
    WHERE screen_name = userIdentifier;
END$$
DELIMITER ;
