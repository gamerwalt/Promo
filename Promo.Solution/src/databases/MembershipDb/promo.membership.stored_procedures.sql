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
