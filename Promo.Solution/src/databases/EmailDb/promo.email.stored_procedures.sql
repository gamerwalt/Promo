use `promo.email`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `mail_create`(IN email_uid VARCHAR(50), IN email_address_to VARCHAR(400), IN parameters VARCHAR(500), IN template_name VARCHAR(45), IN status VARCHAR(45), IN sent BIT)
BEGIN
	INSERT INTO emails (email_uid, email_address_to, parameters, template_name, status, sent, created_date)
    VALUES (email_uid, email_address_to, parameters, template_name, status, sent, CURRENT_TIMESTAMP());
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `mail_get_not_sent`()
BEGIN
	SELECT *
    FROM emails
    WHERE sent = 0;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `mail_update_as_sent`(IN email_uid VARCHAR(50), IN status VARCHAR(45))
BEGIN
	UPDATE emails
		SET sent = 1,
			status = status,
            updated_date = CURRENT_TIMESTAMP()
	WHERE email_uid = email_uid;
END$$
DELIMITER ;