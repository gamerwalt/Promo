CREATE DATABASE `promo.email` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION=`N` */;

CREATE TABLE `emails` (
  `email_id` int unsigned NOT NULL AUTO_INCREMENT,
  `email_uid` varchar(50) NOT NULL,
  `email_address_to` varchar(400) NOT NULL,
  `parameters` varchar(8000) NOT NULL,
  `template_name` varchar(45) NOT NULL,
  `status` varchar(45) NOT NULL,
  `sent` bit(1) NOT NULL DEFAULT b`0`,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`email_id`),
  UNIQUE KEY `email_id_UNIQUE` (`email_id`),
  UNIQUE KEY `email_uid_UNIQUE` (`email_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE PROCEDURE `mail_create`(IN email_uid VARCHAR(50), IN email_address_to VARCHAR(400), IN parameters VARCHAR(500), IN template_name VARCHAR(45), IN status VARCHAR(45), IN sent BIT)
BEGIN
	INSERT INTO emails (email_uid, email_address_to, parameters, template_name, status, sent, created_date)
    VALUES (email_uid, email_address_to, parameters, template_name, status, sent, CURRENT_TIMESTAMP());

CREATE PROCEDURE `mail_get_not_sent`()
BEGIN
	SELECT *
    FROM emails
    WHERE sent = 0;

CREATE PROCEDURE `mail_update_as_sent`(IN email_uid VARCHAR(50), IN status VARCHAR(45))
BEGIN
	UPDATE emails
		SET sent = 1,
			status = status,
            updated_date = CURRENT_TIMESTAMP()
	WHERE email_uid = email_uid;
