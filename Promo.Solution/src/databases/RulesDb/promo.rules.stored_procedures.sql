use `promo.rules`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `entry_rule_create`(IN entryRuleUid VARCHAR(50), IN campaignUid VARCHAR(50), IN sweepUid VARCHAR(50), IN entryPerUser INT)
BEGIN
	INSERT INTO sweep_entry_rules
    (
		entry_rule_uid,
        campaign_uid,
        sweep_uid,
        entry_per_user,
        created_date
    )
    VALUES
    (
		entryRuleUid,
        campaignUid,
        sweepUid,
        entryPerUser,
        CURRENT_TIMESTAMP()
    );
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `entry_rule_get`(IN entryRuleUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_entry_rules
    WHERE entry_rule_uid = entryRuleUid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `entry_rule_get_by_sweep_and_campaign_uid`(IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_entry_rules
    WHERE sweep_uid = sweepUid
    AND campaign_uid = campaignUid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `rule_create`(IN ruleUid VARCHAR(50), IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50), IN watchTypeCode VARCHAR(50), IN watchValue VARCHAR(500), IN activeInd BIT)
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
        sweep_uid,
        watch_type_id,
        watch_value,
        active_ind,
        created_date
    )
    SELECT
		ruleUid,
        campaignUid,
        sweepUid,
        @watchTypeId,
        watchValue,
        activeInd,
        CURRENT_TIMESTAMP();
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `rule_get`(IN ruleUid VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.sweep_uid,
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `rules_get_by_watch_type_code`(IN watchTypeCode VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.sweep_uid,
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `rules_get_by_watchvalue`(IN watchValue VARCHAR(500))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.sweep_uid,
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `rules_get_not_added_for_tracking`(IN watchTypeCode VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.sweep_uid,
        r.watch_value,
        r.active_ind,
        r.added_for_tracking,
        r.date_added_for_tracking
    FROM rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    WHERE wt.watch_type_code = watchTypeCode;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `rules_unadded_get`()
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        r.campaign_uid,
        r.sweep_uid,
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
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `watch_type_campaign_types_get`()
BEGIN
	SELECT
		ct.id,
        wt.watch_type_code,
        wt.watch_type_name,
        ct.campaign_type_uid
    FROM watch_type_campaign_types as ct
    INNER JOIN watch_types as wt ON ct.watch_type_id = wt.watch_type_id;
END$$
DELIMITER ;
