use `promo.sweepstakes`

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_add_entry`(IN sweepEntryUid VARCHAR(50), IN sweepUid VARCHAR(50), IN userIdentifier VARCHAR(500), IN userIdentifierType VARCHAR(100), IN acceptedTerms BIT)
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_add_entry_via_campaign_uid`(IN sweepEntryUid VARCHAR(50), IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50), IN userIdentifier VARCHAR(500), IN userIdentifierType VARCHAR(100), IN acceptedTerms BIT)
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_create`(IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50), IN sweepName VARCHAR(100), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_entry_get_by_uid`(IN sweepEntryGuid VARCHAR(50))
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_get_by_uid`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep
    WHERE sweep_uid = sweepUid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_update`(IN sweepUid VARCHAR(50), IN sweepName VARCHAR(100), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
BEGIN
	UPDATE sweep
		SET sweep_name = sweepName,
			start_date = startDate,
            end_date = endDate,
            draw_date = drawDate,
            updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweeps_get_by_campaignUid`(IN campaignUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep
    WHERE campaign_uid = campaignUid;
END$$
DELIMITER ;
