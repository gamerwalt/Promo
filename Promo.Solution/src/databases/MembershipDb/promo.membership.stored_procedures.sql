use `promo.membership`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `accounts_get`()
BEGIN
	SELECT * FROM accounts;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `accounts_unverified_get`()
BEGIN
	SELECT *
    FROM accounts
    WHERE email_address_verified = 0;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `account_create`(IN email VARCHAR(400), IN emailVerified BIT, IN phone VARCHAR(20), IN phoneVerified BIT, IN accountUid VARCHAR(50), IN displayName VARCHAR(50), IN pin VARCHAR(100), IN termsAccepted BIT)
BEGIN
	INSERT INTO accounts(account_uid, email_address, phone_number, email_address_verified, phone_number_verified, display_name, pin, terms_accepted, created_date)
    VALUES (accountUid, email, phone, emailVerified, phoneVerified, displayName, pin, termsAccepted, CURRENT_TIMESTAMP());
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `account_email_verify`(IN accountUid VARCHAR(50))
BEGIN
	UPDATE accounts
		SET email_address_verified = 1,
			updated_date = CURRENT_TIMESTAMP()
	WHERE account_uid = accountUid;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `account_get_by_display_name`(IN displayName VARCHAR(50))
BEGIN
	SELECT *
    FROM accounts
    WHERE display_name = displayName;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `account_get_by_email`(IN email VARCHAR(400))
BEGIN
	SELECT *
    FROM accounts
    WHERE email_address = email;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `account_get_by_phone`(IN phone VARCHAR(20))
BEGIN
	SELECT *
    FROM accounts
    WHERE phone_number = phone;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `account_get_by_uid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT *
    FROM accounts
    WHERE account_uid = accountUid;
END$$
DELIMITER ;



DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `campaigns_get_by_account_uid`(IN accountUid VARCHAR(50))
BEGIN
	    
    SELECT
		c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.start_date,
        c.end_date,
        c.created_date,
        c.updated_date,
        c.active_ind,
        c.account_uid,
        ct.campaign_type_id,
        ct.campaign_type_uid,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.active_ind as campaign_type_active_ind
    FROM campaigns as c
    INNER JOIN campaign_types as ct ON ct.campaign_type_id = c.campaign_type_id
	WHERE account_uid = accountUid;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `campaign_create`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(50), IN campaignDescription VARCHAR(500), IN startDate DATETIME, IN endDate DATETIME, IN campaignTypeUid VARCHAR(50), IN accountUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
	
    SELECT @campaignTypeId := campaign_type_id FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;

    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
    
	INSERT INTO campaigns
    (
		account_uid,
        campaign_type_id,
        campaign_uid,
        campaign_name,
        campaign_description,
        start_date,
        end_date,
        created_date
    )
    SELECT
		accountUid,
        @campaignTypeId,
        campaignUid,
        campaignName,
        campaignDescription,
        startDate,
        endDate,
        CURRENT_TIMESTAMP();
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `campaign_get_by_uid`(IN campaignUid VARCHAR(50))
BEGIN
	SELECT
		c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.start_date,
        c.end_date,
        c.created_date,
        c.updated_date,
        c.active_ind,
        c.account_uid,
        ct.campaign_type_id,
        ct.campaign_type_uid,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.active_ind as campaign_type_active_ind
    FROM campaigns as c
    INNER JOIN campaign_types as ct ON ct.campaign_type_id = c.campaign_type_id
    WHERE campaign_uid = campaignUid;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `campaign_types_get`()
BEGIN
	SELECT *
    FROM campaign_types;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `campaign_type_get_by_uid`(IN campaignTypeUid VARCHAR(50))
BEGIN
	SELECT *
    FROM campaign_types
    WHERE campaign_type_uid = campaignTypeUid;
END$$
DELIMITER ;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `campaign_update_via_uid`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(50), IN campaignDescription VARCHAR(500), IN startDate DATETIME, IN endDate DATETIME, IN activeInd BIT, IN campaignTypeUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
    
	SELECT @campaignTypeId := campaign_type_id FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;

    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
	

	UPDATE campaigns
		SET campaign_name = campaignName,
			campaign_description = campaignDescription,
            start_date = startDate,
            end_date = endDate,
            active_ind = activeInd,
            campaign_type_id = @campaignTypeId
    WHERE campaign_uid = campaignUid;
END$$
DELIMITER ;

