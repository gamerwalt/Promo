CREATE DATABASE 'promo.sweepstakes' /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

CREATE TABLE 'sweep' (
  'sweep_id' int unsigned NOT NULL AUTO_INCREMENT,
  'sweep_uid' varchar(50) NOT NULL,
  'campaign_uid' varchar(50) NOT NULL,
  'sweep_name' varchar(100) NOT NULL,
  'start_date' datetime NOT NULL,
  'end_date' datetime NOT NULL,
  'draw_date' datetime DEFAULT NULL,
  'created_date' datetime NOT NULL,
  'updated_date' datetime DEFAULT NULL,
  PRIMARY KEY ('sweep_id'),
  UNIQUE KEY 'sweep_id_UNIQUE' ('sweep_id'),
  UNIQUE KEY 'sweep_uid_UNIQUE' ('sweep_uid')
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE 'sweep_entry' (
  'sweep_entry_id' int unsigned NOT NULL AUTO_INCREMENT,
  'sweep_entry_uid' varchar(50) NOT NULL,
  'sweep_id' int unsigned NOT NULL,
  'user_identifier' varchar(500) NOT NULL,
  'user_identifier_type' varchar(100) NOT NULL,
  'accepted_terms' bit(1) NOT NULL DEFAULT b'0',
  'created_date' datetime NOT NULL,
  'updated_date' datetime DEFAULT NULL,
  PRIMARY KEY ('sweep_entry_id'),
  UNIQUE KEY 'sweep_entry_id_UNIQUE' ('sweep_entry_id'),
  UNIQUE KEY 'sweep_entry_uid_UNIQUE' ('sweep_entry_uid'),
  KEY 'sweep_sweep_entry_idx' ('sweep_id'),
  CONSTRAINT 'sweep_sweep_entry' FOREIGN KEY ('sweep_id') REFERENCES 'sweep' ('sweep_id')
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE PROCEDURE 'sweeps_get_by_campaignUid'(IN campaignUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep
    WHERE campaign_uid = campaignUid;

CREATE PROCEDURE 'sweep_add_entry'(IN sweepEntryUid VARCHAR(50), IN sweepUid VARCHAR(50), IN userIdentifier VARCHAR(500), IN userIdentifierType VARCHAR(100), IN acceptedTerms BIT)
BEGIN
	DECLARE sweepId INT DEFAULT 0;
    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;

    IF(@sweepId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found';
	END IF;
    
    INSERT INTO sweep_entry
    (
		sweep_entry_uid,
        sweep_id,
        user_identifier,
        user_identifier_type,
        accepted_terms,
        created_date
    )
    SELECT
		sweepEntryUid,
        @sweepId,
        userIdentifier,
        userIdentifierType,
        acceptedTerms,
        CURRENT_TIMESTAMP();

CREATE PROCEDURE 'sweep_add_entry_via_campaign_uid'(IN sweepEntryUid VARCHAR(50), IN campaignUid VARCHAR(50), IN userIdentifier VARCHAR(500), IN userIdentifierType VARCHAR(100), IN acceptedTerms BIT)
BEGIN
	DECLARE sweepId INT DEFAULT 0;
    SELECT @sweepId := sweep_id FROM sweep WHERE campaign_uid = campaignUid;

    IF(@sweepId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found';
	END IF;
    
    INSERT INTO sweep_entry
    (
		sweep_entry_uid,
        sweep_id,
        user_identifier,
        user_identifier_type,
        accepted_terms,
        created_date
    )
    SELECT
		sweepEntryUid,
        @sweepId,
        userIdentifier,
        userIdentifierType,
        acceptedTerms,
        CURRENT_TIMESTAMP();

CREATE PROCEDURE `sweep_create`(IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50), IN sweepName VARCHAR(100), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
BEGIN
	INSERT INTO sweep
	(
		sweep_uid,
        campaign_uid,
        sweep_name,
        start_date,
        end_date,
        draw_date,
        created_date
    )
    SELECT
		sweepUid,
        campaignUid,
        sweepName,
        startDate,
        endDate,
        drawDate,
		CURRENT_TIMESTAMP();
END

CREATE PROCEDURE 'sweep_entry_get_by_uid'(IN sweepEntryGuid VARCHAR(50))
BEGIN
	SELECT 
		se.sweep_entry_id,
        se.sweep_entry_uid,
        se.user_identifier,
        se.user_identifier_type,
        se.accepted_terms,
        se.created_date,
        s.sweep_id,
        s.sweep_uid,
        s.campaign_uid,
        s.sweep_name,
        s.start_date,
        s.end_date,
        s.draw_date,
        s.created_date as sweep_created_date
    FROM sweep_entry as se
    INNER JOIN sweep as s ON se.sweep_id = s.sweep_id
    WHERE se.sweep_entry_uid = sweepEntryGuid;

CREATE PROCEDURE 'sweep_get_by_uid'(IN sweepUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep
    WHERE sweep_uid = sweepUid;

CREATE PROCEDURE 'sweep_update'(IN sweepUid VARCHAR(50), IN sweepName VARCHAR(100), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
BEGIN
	UPDATE sweep
		SET sweep_name = sweepName,
			start_date = startDate,
            end_date = endDate,
            draw_date = drawDate,
            updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
