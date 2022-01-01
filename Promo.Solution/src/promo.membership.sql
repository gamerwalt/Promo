CREATE DATABASE `promo.membership` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION=`N` */;

CREATE TABLE `accounts` (
  `account_id` int unsigned NOT NULL AUTO_INCREMENT,
  `account_uid` varchar(50) NOT NULL,
  `email_address` varchar(400) NOT NULL,
  `phone_number` varchar(20) NOT NULL,
  `email_address_verified` bit(1) DEFAULT NULL,
  `phone_number_verified` bit(1) DEFAULT NULL,
  `display_name` varchar(50) DEFAULT NULL,
  `pin` varchar(100) DEFAULT NULL,
  `terms_accepted` bit(1) DEFAULT NULL,
  `created_date` timestamp NOT NULL,
  `updated_date` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`account_id`,`account_uid`,`email_address`,`phone_number`),
  UNIQUE KEY `user_id_UNIQUE` (`account_id`),
  UNIQUE KEY `user_uid_UNIQUE` (`account_uid`),
  UNIQUE KEY `email_address_UNIQUE` (`email_address`),
  UNIQUE KEY `phone_number_UNIQUE` (`phone_number`),
  UNIQUE KEY `display_name_UNIQUE` (`display_name`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE PROCEDURE `accounts_get`()
BEGIN
	SELECT * FROM accounts;

CREATE PROCEDURE `accounts_unverified_get`()
BEGIN
	SELECT *
    FROM accounts
    WHERE email_address_verified = 0;

CREATE PROCEDURE `account_create`(IN email VARCHAR(400), IN emailVerified BIT, IN phone VARCHAR(20), IN phoneVerified BIT, IN accountUid VARCHAR(50), IN displayName VARCHAR(50), IN pin VARCHAR(100), IN termsAccepted BIT)
BEGIN
	INSERT INTO accounts(account_uid, email_address, phone_number, email_address_verified, phone_number_verified, display_name, pin, terms_accepted, created_date)
    VALUES (accountUid, email, phone, emailVerified, phoneVerified, displayName, pin, termsAccepted, CURRENT_TIMESTAMP());

CREATE PROCEDURE `account_email_verify`(IN accountUid VARCHAR(50))
BEGIN
	UPDATE accounts
		SET email_address_verified = 1,
			updated_date = CURRENT_TIMESTAMP()
	WHERE account_uid = accountUid;

CREATE PROCEDURE `account_get_by_display_name`(IN displayName VARCHAR(50))
BEGIN
	SELECT *
    FROM accounts
    WHERE display_name = displayName;

CREATE PROCEDURE `account_get_by_email`(IN email VARCHAR(400))
BEGIN
	SELECT *
    FROM accounts
    WHERE email_address = email;

CREATE PROCEDURE `account_get_by_phone`(IN phone VARCHAR(20))
BEGIN
	SELECT *
    FROM accounts
    WHERE phone_number = phone;


CREATE PROCEDURE `account_get_by_uid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT *
    FROM accounts
    WHERE account_uid = accountUid;