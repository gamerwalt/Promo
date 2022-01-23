use `promo.prizes`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_prize_get`(IN prizeUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_prizes
    WHERE prize_uid = prizeUid;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_prizes_create`(IN prizeUid VARCHAR(50), IN sweepUid VARCHAR(50), IN enabledInd BIT, IN prizeName VARCHAR(100), IN prizeDescription VARCHAR(500),
	IN prizeValue DECIMAL(18, 2), IN locale VARCHAR(45), IN prizePosition INT)
BEGIN
	INSERT INTO sweep_prizes
    (
		prize_uid,
        sweep_uid,
        enabled_ind,
        prize_position,
        prize_name,
        prize_description,
        prize_value,
        locale,
        created_date
    )
    VALUES
    (
		prizeUid,
        sweepUid,
        enabledInd,
        prizePosition,
        prizeName,
        prizeDescription,
        prizeValue,
        locale,
        CURRENT_TIMESTAMP()
    );
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_prizes_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_prizes
    WHERE sweep_uid = sweepUid;
END$$
DELIMITER ;
