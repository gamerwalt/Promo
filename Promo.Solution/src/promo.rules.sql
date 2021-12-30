CREATE DATABASE promo.rules /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

CREATE TABLE rules (
  rule_id int unsigned NOT NULL AUTO_INCREMENT,
  rule_uid varchar(50) NOT NULL,
  campaign_uid varchar(50) NOT NULL,
  watch_type_id int unsigned NOT NULL,
  watch_value varchar(500) NOT NULL,
  active_ind bit(1) NOT NULL DEFAULT b'0',
  added_for_tracking bit(1) DEFAULT b'0',
  date_added_for_tracking datetime DEFAULT NULL,
  created_date datetime NOT NULL,
  updated_date datetime DEFAULT NULL,
  PRIMARY KEY (rule_id),
  UNIQUE KEY rule_id_UNIQUE (rule_id),
  UNIQUE KEY rule_uid_UNIQUE (rule_uid),
  KEY watch_types_rules_idx (watch_type_id),
  CONSTRAINT watch_types_rules FOREIGN KEY (watch_type_id) REFERENCES watch_types (watch_type_id)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE watch_type_campaign_types (
  id int unsigned NOT NULL AUTO_INCREMENT,
  watch_type_id int unsigned NOT NULL,
  campaign_type_uid varchar(50) NOT NULL,
  PRIMARY KEY (id),
  UNIQUE KEY id_UNIQUE (id)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE watch_types (
  watch_type_id int unsigned NOT NULL AUTO_INCREMENT,
  watch_type_code varchar(50) NOT NULL,
  watch_type_name varchar(100) NOT NULL,
  PRIMARY KEY (watch_type_id),
  UNIQUE KEY watch_id_UNIQUE (watch_type_id)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE PROCEDURE rules_get_by_watchvalue(IN watchValue VARCHAR(500))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.watch_value,
        r.active_ind,
        wv.watch_type_id,
        wv.watch_type_code,
        wv.watch_type_name,
        r.created_date,
        r.updated_date
    FROM rules as r
    INNER JOIN watch_types as wv ON wv.watch_type_id = r.watch_type_id
    WHERE r.watch_value = watchValue;

CREATE PROCEDURE rules_get_by_watch_type_code(IN watchTypeCode VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.watch_value,
        r.active_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date
    FROM rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    WHERE wt.watch_type_code = watchTypeCode;

CREATE PROCEDURE rules_get_not_added_for_tracking(IN watchTypeCode VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.watch_value,
        r.active_ind,
        r.added_for_tracking,
        r.date_added_for_tracking
    FROM rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    WHERE wt.watch_type_code = watchTypeCode;

CREATE PROCEDURE rules_unadded_get()
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.watch_value,
        r.active_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date
    FROM rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    WHERE r.added_for_tracking = 0;

CREATE PROCEDURE rule_create(IN ruleUid VARCHAR(50), IN campaignUid VARCHAR(50), IN watchTypeCode VARCHAR(50), IN watchValue VARCHAR(500), IN activeInd BIT)
BEGIN
	DECLARE watchTypeId INT DEFAULT 0;
	
    SELECT @watchTypeId := watch_type_id FROM watch_types WHERE watch_type_code = watchTypeCode;

    IF(@watchTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Watch type not found';
	END IF;
    
    INSERT INTO rules
    (
		rule_uid,
        campaign_uid,
        watch_type_id,
        watch_value,
        active_ind,
        created_date
    )
    SELECT
		ruleUid,
        campaignUid,
        @watchTypeId,
        watchValue,
        activeInd,
        CURRENT_TIMESTAMP();

CREATE PROCEDURE rule_get(IN ruleUid VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.watch_value,
        r.active_ind,
        r.created_date,
        r.updated_date,
        wt.watch_type_id,
        wt.watch_type_code,
        wt.watch_type_name
    FROM rules as r
    INNER JOIN watch_types as wt ON wt.watch_type_id = r.watch_type_id
    WHERE r.rule_uid = ruleUid;

CREATE PROCEDURE watch_type_campaign_types_get()
BEGIN
	SELECT
		ct.id,
        wt.watch_type_code,
        wt.watch_type_name,
        ct.campaign_type_uid
    FROM watch_type_campaign_types as ct
    INNER JOIN watch_types as wt ON ct.watch_type_id = wt.watch_type_id;