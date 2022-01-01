CREATE DATABASE `promo.campaign` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION=`N` */;

CREATE TABLE `campaign_types` (
  `campaign_type_id` int unsigned NOT NULL AUTO_INCREMENT,
  `campaign_type_uid` varchar(50) NOT NULL,
  `campaign_type_code` varchar(60) NOT NULL,
  `campaign_type_name` varchar(50) NOT NULL DEFAULT `T`,
  `campaign_type_description` varchar(200) DEFAULT NULL,
  `active_ind` bit(1) NOT NULL DEFAULT b`1`,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`campaign_type_id`),
  UNIQUE KEY `campaign_type_id_UNIQUE` (`campaign_type_id`),
  UNIQUE KEY `campaign_type_uid_UNIQUE` (`campaign_type_uid`),
  UNIQUE KEY `campaign_name_UNIQUE` (`campaign_type_code`)
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `campaigns` (
  `campaign_id` int unsigned NOT NULL AUTO_INCREMENT,
  `account_uid` varchar(50) NOT NULL,
  `campaign_type_id` int unsigned NOT NULL,
  `campaign_uid` varchar(50) NOT NULL,
  `campaign_name` varchar(50) NOT NULL,
  `campaign_description` varchar(500) DEFAULT NULL,
  `active_ind` bit(1) NOT NULL DEFAULT b`1`,
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`campaign_id`),
  UNIQUE KEY `campaign_id_UNIQUE` (`campaign_id`),
  UNIQUE KEY `campaign_uid_UNIQUE` (`campaign_uid`),
  UNIQUE KEY `account_to_campaign_unique` (`account_uid`,`campaign_name`,`campaign_type_id`),
  KEY `campaign_type_campaign_idx` (`campaign_type_id`),
  CONSTRAINT `campaign_type_campaign` FOREIGN KEY (`campaign_type_id`) REFERENCES `campaign_types` (`campaign_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE PROCEDURE `campaigns_get_by_account_uid`(IN accountUid VARCHAR(50))
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




CREATE PROCEDURE `campaign_create`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(50), IN campaignDescription VARCHAR(500), IN startDate DATETIME, IN endDate DATETIME, IN campaignTypeUid VARCHAR(50), IN accountUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
	
    SELECT @campaignTypeId := campaign_type_id FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;

    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE `45000`
			SET MESSAGE_TEXT = `Campaign not found`;
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




CREATE PROCEDURE `campaign_get_by_uid`(IN campaignUid VARCHAR(50))
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

CREATE PROCEDURE `campaign_types_get`()
BEGIN
	SELECT *
    FROM campaign_types;

CREATE PROCEDURE `campaign_type_get_by_uid`(IN campaignTypeUid VARCHAR(50))
BEGIN
	SELECT *
    FROM campaign_types
    WHERE campaign_type_uid = campaignTypeUid;

CREATE PROCEDURE `campaign_update_via_uid`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(50), IN campaignDescription VARCHAR(500), IN startDate DATETIME, IN endDate DATETIME, IN activeInd BIT, IN campaignTypeUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
    
	SELECT @campaignTypeId := campaign_type_id FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;

    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE `45000`
			SET MESSAGE_TEXT = `Campaign not found`;
	END IF;
	

	UPDATE campaigns
		SET campaign_name = campaignName,
			campaign_description = campaignDescription,
            start_date = startDate,
            end_date = endDate,
            active_ind = activeInd,
            campaign_type_id = @campaignTypeId
    WHERE campaign_uid = campaignUid;
