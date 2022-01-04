use `promo.reporting`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_entry_report_get`(
	IN campaignUid VARCHAR(50), 
    IN sweepUid VARCHAR(50), 
    IN userIdentifierType VARCHAR(100), 
    IN externalUserId VARCHAR(100), 
    IN screenName VARCHAR(100))
BEGIN
	SELECT *
    FROM sweep_entries
    WHERE campaign_uid = campaignUid
    AND sweep_uid = sweepUid
    AND user_identifier_type = userIdentifierType
    AND external_user_id = externalUserId
    AND screen_name = screenName;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_entry_upsert`(IN sweepEntryReportUid VARCHAR(50), IN campaignUid VARCHAR(50), IN sweepUid VARCHAR(50), IN screenName VARCHAR(100), IN userIdentifierType VARCHAR(100), IN externalId VARCHAR(100), IN externalUserId VARCHAR(100), IN count INT)
BEGIN
	INSERT INTO sweep_entries
    (
		sweep_entry_uid,
        sweep_uid,
        campaign_uid,
        screen_name,
        user_identifier_type,
        external_id,
        external_user_id,
        entries,
        created_date
    )
    VALUES
    (
		sweepEntryReportUid,
        sweepUid,
        campaignUid,
        screenName,
        userIdentifierType,
        externalId,
        externalUserId,
        count,
        CURRENT_TIMESTAMP()
    )
    ON DUPLICATE KEY UPDATE
	  screen_name = screenName,
	  entries = entries + count,
      updated_date = CURRENT_TIMESTAMP();
END$$
DELIMITER ;
