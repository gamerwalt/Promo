-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: hrvster_test
-- ------------------------------------------------------
-- Server version	8.0.27

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping routines for database 'hrvster_test'
--
/*!50003 DROP PROCEDURE IF EXISTS `accounts_unverified_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `accounts_unverified_get`()
BEGIN
	SELECT *
    FROM accounts
    WHERE email_address_verified = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_create`(
	IN email VARCHAR(400), 
    IN emailVerified BIT, 
    IN phone VARCHAR(20), 
    IN phoneVerified BIT, 
    IN accountUid VARCHAR(50), 
    IN displayName VARCHAR(50), 
    IN pin VARCHAR(100), 
    IN termsAccepted BIT,
    IN verificationLink VARCHAR(1000))
BEGIN
	INSERT INTO accounts
    (
		account_uid, 
        email_address, 
        phone_number, 
        email_address_verified, 
        phone_number_verified, 
        display_name, 
        pin, 
        terms_accepted, 
        verification_link,
        created_date
	)
    VALUES 
    (
		accountUid, 
        email, 
        phone, 
        emailVerified, 
        phoneVerified, 
        displayName, 
        pin, 
        termsAccepted, 
        verificationLink,
        CURRENT_TIMESTAMP()
	);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_email_verify` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_email_verify`(IN accountUid VARCHAR(50))
BEGIN
	UPDATE accounts
		SET email_address_verified = 1,
			updated_date = CURRENT_TIMESTAMP()
	WHERE account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_get_by_displayname` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_get_by_displayname`(IN displayName VARCHAR(500))
BEGIN
	SELECT *
    FROM accounts
    WHERE display_name = displayName;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_get_by_email` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_get_by_email`(IN email VARCHAR(500))
BEGIN
	SELECT *
    FROM accounts
    WHERE email_address = email;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_get_by_phonenumber` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_get_by_phonenumber`(IN phoneNumber VARCHAR(50))
BEGIN
	SELECT *
    FROM accounts
    WHERE phone_number = phoneNumber;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_get_by_uid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT *
    FROM accounts
    WHERE account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_subscriptions_get_by_account_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_subscriptions_get_by_account_uid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT
		A.account_id,
        A.account_uid,
        ASB.subscription_id,
        ASB.subscription_uid,
        ASB.is_discounted,
        ASB.discounted_by,
        ASB.start_date,
        ASB.start_date_utc,
        ASB.end_date,
        ASB.end_date_utc,
        ASB.subscription_status,
        P.plan_id,
        P.plan_uid,
        P.plan_name,
        P.plan_description,
        P.promotions_per_month,
        P.sweeps_per_month,
        P.sweepstakes_maximum_entries,
        P.sweep_hashtags_per_month,
        P.price_per_month,
        P.price_per_year
    FROM accounts as A
    INNER JOIN account_subscriptions as ASB ON ASB.account_id = A.account_id
    INNER JOIN plans as P ON P.plan_id = ASB.plan_id
    WHERE A.account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_subscription_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_subscription_create`(
	IN subscriptionUid VARCHAR(50),
    IN accountUid VARCHAR(50),
    IN planId INT,
    IN subscriptionStatus VARCHAR(100),
    IN startDate DATETIME,
    IN startDateUtc DATETIME,
    IN endDate DATETIME,
    IN endDateUtc DATETIME,
    IN isDiscounted TINYINT,
    IN discountedBy DECIMAL)
BEGIN
	DECLARE accountId INT DEFAULT 0;

    SELECT account_id INTO @accountId FROM accounts WHERE account_uid = accountUid;
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found';
    END IF;

	INSERT INTO account_subscriptions
    (
		subscription_uid,
        subscription_status,
        account_id,
        plan_id,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        is_discounted,
        discounted_by,
        created_date
    )
    VALUES
    (
		subscriptionUid,
        subscriptionStatus,
        @accountId,
        planId,
        startDate,
        startDateUtc,
        endDate,
        endDateUtc,
        isDiscounted,
        discountedBy,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_subscription_get_by_email` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_subscription_get_by_email`(IN email VARCHAR(500))
BEGIN
	SELECT
		A.account_id,
        A.account_uid,
        ASB.subscription_id,
        ASB.subscription_uid,
        ASB.is_discounted,
        ASB.discounted_by,
        ASB.start_date,
        ASB.start_date_utc,
        ASB.end_date,
        ASB.end_date_utc,
        ASB.subscription_status,
        P.plan_id,
        P.plan_uid,
        P.plan_name,
        P.plan_description,
        P.promotions_per_month,
        P.sweeps_per_month,
        P.sweepstakes_maximum_entries,
        P.sweep_hashtags_per_month,
        P.price_per_month,
        P.price_per_year
    FROM accounts as A
    INNER JOIN account_subscriptions as ASB ON ASB.account_id = A.account_id
    INNER JOIN plans as P ON P.plan_id = ASB.plan_id
    WHERE A.email_address = email;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_subscription_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_subscription_get_by_uid`(IN subscriptionUid VARCHAR(50))
BEGIN
	SELECT
		A.account_id,
        A.account_uid,
        ASB.subscription_id,
        ASB.subscription_uid,
        ASB.is_discounted,
        ASB.discounted_by,
        ASB.start_date,
        ASB.start_date_utc,
        ASB.end_date,
        ASB.end_date_utc,
        ASB.subscription_status,
        P.plan_id,
        P.plan_uid,
        P.plan_name,
        P.plan_description,
        P.promotions_per_month,
        P.sweeps_per_month,
        P.sweepstakes_maximum_entries,
        P.sweep_hashtags_per_month,
        P.price_per_month,
        P.price_per_year
    FROM accounts as A
    INNER JOIN account_subscriptions as ASB ON ASB.account_id = A.account_id
    INNER JOIN plans as P ON P.plan_id = ASB.plan_id
    WHERE ASB.subscription_uid = subscriptionUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_subscription_update_dates` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_subscription_update_dates`(
	IN subscriptionUid VARCHAR(50),
    IN startDate DATETIME,
    IN startDateUtc DATETIME,
    IN endDate DATETIME,
    IN endDateUtc DATETIME)
BEGIN
	INSERT INTO account_subscriptions_history
    (
		subscription_history_uid,
        account_id,
        subscription_status,
        plan_id,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        is_discounted,
        discounted_by,
        created_date,
        updated_date
    )
    SELECT
		subscription_uid,
        account_id,
        subscription_status,
        plan_id,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        is_discounted,
        discounted_by,
        created_date,
        updated_date
    FROM account_subscriptions
    WHERE subscription_uid = subscriptionUid;
    
    UPDATE account_subscriptions
		SET start_date = startDate,
			start_date_utc = startDateUtc,
            end_date = endDate,
            end_date_utc = endDateUtc
	WHERE subscription_uid = subscriptionUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_subscription_update_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_subscription_update_status`(IN subscriptionUid VARCHAR(50), IN subscriptionStatus VARCHAR(100))
BEGIN
	
    INSERT INTO account_subscriptions_history
    (
		subscription_history_uid,
        account_id,
        subscription_status,
        plan_id,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        is_discounted,
        discounted_by,
        created_date,
        updated_date
    )
    SELECT
		subscription_uid,
        account_id,
        subscription_status,
        plan_id,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        is_discounted,
        discounted_by,
        created_date,
        updated_date
    FROM account_subscriptions
    WHERE subscription_uid = subscriptionUid;

	UPDATE account_subscriptions
		SET subscription_status = subscriptionStatus,
			updated_date = CURRENT_TIMESTAMP()
	WHERE subscription_uid = subscriptionUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_update`(
	IN accountUid VARCHAR(50),
    IN firstName VARCHAR(100),
    IN lastName VARCHAR(100),
    IN displayName VARCHAR(50))
BEGIN
	UPDATE accounts
		SET first_name = firstName,
			last_name = lastName,
			display_name = displayName,
            updated_date = CURRENT_TIMESTAMP()
		WHERE account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_update_email_phone` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_update_email_phone`(
	IN accountUid VARCHAR(50),
    IN emailAddress VARCHAR(500),
    IN phoneNumber VARCHAR(50))
BEGIN
	UPDATE accounts
		SET email_address = emailAddress,
			phone_number = phoneNumber,
            updated_date = CURRENT_TIMESTAMP()
	WHERE account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_update_history_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_update_history_create`(
	IN uid VARCHAR(50),
    IN accountId INT,
    IN oldValue VARCHAR(600),
    IN newValue VARCHAR(600))
BEGIN
	INSERT INTO account_update_history
    (
		uid,
        account_id,
        old_value,
        new_value,
        created_date
    )
    VALUES
    (
		uid,
        accountId,
        oldValue,
        newValue,
		CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `account_update_last_login` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_update_last_login`(IN accountUid VARCHAR(50), IN lastLoginDate DATETIME, IN lastLoginDateUTC DATETIME)
BEGIN
	UPDATE accounts
		SET last_login_date = lastLoginDate,
			last_login_date_utc = lastLoginDateUTC,
            updated_date = CURRENT_TIMESTAMP()
	WHERE account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `activity_add` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `activity_add`(
	IN activityUid VARCHAR(50), 
    IN accountUid VARCHAR(50), 
    IN linkedUid VARCHAR(50), 
    IN activityType VARCHAR(100), 
    IN activityTableBaseName VARCHAR(100),
    IN createdDate DATETIME,
    IN createdDateUtc DateTime)
BEGIN
	DECLARE accountId INT DEFAULT 0;
    
	SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found';
    END IF;

	INSERT INTO user_activities
    (
		activity_uid,
        account_id,
        linked_uid,
        activity_type,
        activity_table_base_name,
        created_date,
        created_date_utc
    )
    VALUES
    (
		activityUid,
        @accountId,
        linkedUid,
        activityType,
        activityTableBaseName,
        createdDate,
        createdDateUtc
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaigns_get_by_account_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaigns_get_by_account_uid`(IN accountUid VARCHAR(50))
BEGIN
	    
    SELECT
		c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.start_date,
        c.start_date_utc,
        c.end_date,
        c.end_date_utc,
        c.created_date,
        c.updated_date,
        c.active_ind,
        c.account_id,
        a.account_id,
        a.account_uid,
        a.email_address,
        a.phone_number,
        a.display_name,
        a.terms_accepted,
        ct.campaign_type_id,
        ct.campaign_type_uid,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.active_ind as campaign_type_active_ind,
        ct.campaign_type_description
    FROM campaigns as c
    INNER JOIN accounts a ON a.account_id = c.account_id
    INNER JOIN campaign_types as ct ON ct.campaign_type_id = c.campaign_type_id
	WHERE a.account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_create`(
	IN campaignUid VARCHAR(50), 
    IN campaignName VARCHAR(50), 
    IN campaignDescription VARCHAR(500), 
    IN startDate DATETIME, 
    IN startDateUtc DATETIME,
    IN endDate DATETIME, 
    IN endDateUtc DATETIME,
    IN campaignTypeUid VARCHAR(50), 
    IN accountUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
    DECLARE accountId INT DEFAULT 0;

    SELECT campaign_type_id INTO @campaignTypeId FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;
    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
    
    SELECT account_id INTO @accountId FROM accounts WHERE account_uid = accountUid;
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found';
    END IF;
    
	INSERT INTO campaigns
    (
		account_id,
        campaign_type_id,
        campaign_uid,
        campaign_name,
        campaign_description,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        created_date
    )
    SELECT
		@accountId,
        @campaignTypeId,
        campaignUid,
        campaignName,
        campaignDescription,
        startDate,
        startDateUtc,
        endDate,
        endDateUtc,
        CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_get`(IN campaignUid VARCHAR(50))
BEGIN
	SELECT
		c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.start_date,
        c.start_date_utc,
        c.end_date,
        c.end_date_utc,
        c.created_date,
        c.updated_date,
        c.active_ind,
        a.account_id,
        a.account_uid,
        a.email_address,
        a.phone_number,
        a.display_name,
        a.terms_accepted,
        ct.campaign_type_id,
        ct.campaign_type_uid,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.active_ind as campaign_type_active_ind,
        ct.campaign_type_description
    FROM campaigns as c
    INNER JOIN campaign_types as ct ON ct.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as a ON a.account_id = c.account_id
    WHERE c.campaign_uid = campaignUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_get_by_uid`(IN campaignUid VARCHAR(50), IN accountUid VARCHAR(50))
BEGIN	
	SELECT
		c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.start_date,
        c.start_date_utc,
        c.end_date,
        c.end_date_utc,
        c.created_date,
        c.updated_date,
        c.active_ind,
        a.account_id,
        a.account_uid,
        a.email_address,
        a.phone_number,
        a.display_name,
        a.terms_accepted,
        ct.campaign_type_id,
        ct.campaign_type_uid,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.active_ind as campaign_type_active_ind,
        ct.campaign_type_description
    FROM campaigns as c
    INNER JOIN campaign_types as ct ON ct.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as a ON a.account_id = c.account_id
    WHERE c.campaign_uid = campaignUid
    AND a.account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_types_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_types_get`()
BEGIN
	SELECT *
    FROM campaign_types;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_type_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_type_get_by_uid`(IN campaignTypeUid VARCHAR(50))
BEGIN
	SELECT *
    FROM campaign_types
    WHERE campaign_type_uid = campaignTypeUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_update_via_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_update_via_uid`(
	IN campaignUid VARCHAR(50), 
    IN campaignName VARCHAR(50), 
    IN campaignDescription VARCHAR(500), 
    IN startDate DATETIME, 
    IN startDateUtc DATETIME,
    IN endDate DATETIME, 
    IN endDateUtc DATETIME,
    IN activeInd BIT, 
    IN campaignTypeUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
    
	SELECT campaign_type_id INTO @campaignTypeId FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;

    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
	

	UPDATE campaigns
		SET campaign_name = campaignName,
			campaign_description = campaignDescription,
            start_date = startDate,
            start_date_utc = startDateUtc,
            end_date = endDate,
            end_date_utc = endDateUtc,
            active_ind = activeInd,
            campaign_type_id = @campaignTypeId,
            updated_date = Current_Timestamp()
    WHERE campaign_uid = campaignUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `cron_services_enabled_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `cron_services_enabled_get`()
BEGIN
	SELECT
		id,
        name,
        fqn,
        enabled
    FROM cron_services
    WHERE enabled = 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `entry_rule_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `entry_rule_create`(IN entryRuleUid VARCHAR(50), IN sweepId INT, IN entryPerUser INT)
BEGIN
	INSERT INTO sweep_entry_rules
    (
		entry_rule_uid,
        sweep_id,
        entry_per_user,
        created_date
    )
    VALUES
    (
		entryRuleUid,
        sweepId,
        entryPerUser,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `entry_rule_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `entry_rule_get`(IN entryRuleUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_entry_rules
    WHERE entry_rule_uid = entryRuleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `flutterwave_card_transactions_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `flutterwave_card_transactions_create`(
	IN uid VARCHAR(50),
    IN id BIGINT,
    IN transactionReference VARCHAR(100),
    IN flowReference VARCHAR(100),
    IN orderReference VARCHAR(100))
BEGIN
	INSERT INTO flutterwave_card_transactions
    (
		flutterwave_card_transaction_uid,
        id,
        transaction_reference,
        flow_reference,
        order_reference,
        created_date
    )
    VALUES
    (
		uid,
        id,
        transactionReference,
        flowReference,
        orderReference,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `flutterwave_card_transactions_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `flutterwave_card_transactions_get`(
	IN id BIGINT,
    IN txRef VARCHAR(100),
    IN flwRef VARCHAR(100),
    IN orderRef VARCHAR(100))
BEGIN
	SELECT
		*
    FROM flutterwave_card_transactions
    WHERE id = id
    AND transaction_reference = txRef
    AND flow_reference = flwRef
    AND order_reference = orderRef;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `flutterwave_events_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `flutterwave_events_create`(
	IN flutterWaveEventUid VARCHAR(50),
    IN payload VARCHAR(8000),
    IN eventStatus VARCHAR(100))
BEGIN
	INSERT INTO flutterwave_events
    (
		flutterwave_event_uid,
        payload_received,
        event_status,
        created_date
    )
    values
    (
		flutterWaveEventUid,
        payload,
        eventStatus,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `flutterwave_events_get_by_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `flutterwave_events_get_by_status`(IN eventStatus VARCHAR(100))
BEGIN
	SELECT *
    FROM flutterwave_events
    WHERE event_status = eventStatus;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `flutterwave_events_update_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `flutterwave_events_update_status`(IN flutterWaveEventUid VARCHAR(50), IN eventStatus VARCHAR(100))
BEGIN
	UPDATE flutterwave_events
		SET event_status = eventStatus,
			updated_date = CURRENT_TIMESTAMP()
	WHERE flutterwave_event_uid = flutterWaveEventUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `harvested_tweets_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `harvested_tweets_get`(IN accountUid VARCHAR(50), IN campaignUid VARCHAR(50), IN previousTime DateTime)
BEGIN
	SELECT
		COUNT(1) as count,
		S.sweep_name,
        S.sweep_uid,
		FROM_UNIXTIME(FLOOR(UNIX_TIMESTAMP(ph.created_date)/900)*900) AS the_time
	FROM accounts as A
	INNER JOIN campaigns as C ON A.account_id = C.account_id
	INNER JOIN sweep as S on S.campaign_id = C.campaign_id
	INNER JOIN sweep_rules as SR ON SR.sweep_id = S.sweep_id
	INNER JOIN `promo.harvester`.harvested as ph ON ph.rule_uid = SR.rule_uid
	WHERE A.account_uid = accountUid
    AND C.campaign_uid = campaignUid
    AND ph.created_date <= CURRENT_TIMESTAMP()
	/*AND ph.created_date BETWEEN previousTime AND CURRENT_TIMESTAMP()*/
	GROUP BY S.sweep_name, S.sweep_uid, the_time;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `harvester_service_command_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `harvester_service_command_create`(IN uid VARCHAR(50), IN command VARCHAR(100))
BEGIN
	INSERT INTO harvester_service_command
    (
		uid,
        command,
        created_date
    )
    VALUES
    (
		uid,
        command,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `harvester_service_command_delete_all` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `harvester_service_command_delete_all`()
BEGIN
	DELETE FROM harvester_service_command WHERE id > 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `harvester_service_command_get_most_recent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `harvester_service_command_get_most_recent`()
BEGIN
	SELECT *
    FROM harvester_service_command
    ORDER BY id DESC
    LIMIT 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `mail_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `mail_create`(
	IN email_uid VARCHAR(50), 
    IN email_address_to VARCHAR(400), 
    IN parameters VARCHAR(500), 
    IN template_name VARCHAR(45), 
    IN status VARCHAR(45), 
    IN sent BIT)
BEGIN
	INSERT INTO emails (email_uid, email_address_to, parameters, template_name, status, sent, created_date)
    VALUES (email_uid, email_address_to, parameters, template_name, status, sent, CURRENT_TIMESTAMP());
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `mail_get_not_sent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `mail_get_not_sent`()
BEGIN
	SELECT *
    FROM emails
    WHERE sent = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `mail_update_as_sent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `mail_update_as_sent`(IN email_uid VARCHAR(50), IN status VARCHAR(45))
BEGIN
	UPDATE emails
		SET sent = 1,
			status = status,
            updated_date = CURRENT_TIMESTAMP()
	WHERE email_uid = email_uid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_boards_get_by_match_game_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_boards_get_by_match_game_uid`(IN matchGameUid VARCHAR(50))
BEGIN
	SELECT
		MB.board_id,
        MB.board_uid,
        MB.is_winnable,
        MP.prize_id,
        MP.prize_uid
    FROM match_boards as MB
    INNER JOIN match_games as MG ON MB.match_game_id = MG.match_game_id
    LEFT JOIN match_prizes as MP ON MP.prize_id = MB.prize_id
    WHERE MG.match_game_uid = matchGameUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_board_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_board_create`(
	IN matchBoardUid VARCHAR(50),
    IN isWinnable bool,
    IN matchGameUid VARCHAR(50),
    IN matchPrizeUid VARCHAR(50))
BEGIN
	DECLARE matchGameId INT DEFAULT 0;
    DECLARE prizeId INT DEFAULT 0;

    SELECT match_game_id INTO @matchGameId FROM match_games WHERE match_game_uid = matchGameUid;
    IF(@matchGameId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Match Game not found';
	END IF;
    
    IF(matchPrizeUid != '00000000-0000-0000-0000-000000000000') THEN
		SELECT prize_id INTO @prizeId FROM match_prizes WHERE prize_uid = matchPrizeUid;
		IF(@accountId = 0) THEN
			SIGNAL SQLSTATE '45000'
				SET MESSAGE_TEXT = 'Match Prize not found';
		END IF;
	END IF;
    
    INSERT INTO match_boards
    (
		board_uid,
        match_game_id,
        prize_id,
        is_winnable,
        created_date
    )
    VALUES
    (
		matchBoardUid,
        @matchGameId,
        @prizeId,
        isWinnable,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_games_get_by_account_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `match_games_get_by_account_uid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT
		M.match_game_id,
        C.campaign_id,
        C.campaign_uid,
        M.match_game_uid,
        M.match_game_name,
        M.match_game_description,
        M.number_of_boards,
        M.tile_count_per_board,
        M.number_of_winnable_boards,
        M.win_probability_per_board,
        M.is_generated,
        M.ready_to_generate,
        M.start_date,
        M.start_date_utc,
        M.end_date,
        M.end_date_utc
    FROM match_games as M
    INNER JOIN campaigns as C ON C.campaign_id = M.campaign_id
    INNER JOIN accounts as A ON A.account_id = C.account_id
    WHERE A.account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_games_get_by_campaign_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_games_get_by_campaign_uid`(IN campaignUid VARCHAR(50))
BEGIN
	SELECT
		M.match_game_id,
        C.campaign_id,
        C.campaign_uid,
        M.match_game_uid,
        M.match_game_name,
        M.match_game_description,
        M.number_of_boards,
        M.tile_count_per_board,
        M.number_of_winnable_boards,
        M.win_probability_per_board,
        M.is_generated,
        M.ready_to_generate,
        M.start_date,
        M.start_date_utc,
        M.end_date,
        M.end_date_utc
    FROM match_games as M
    INNER JOIN campaigns as C ON C.campaign_id = M.campaign_id
    WHERE C.campaign_uid = campaignUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_game_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_game_create`(
	IN campaignId INT,
    IN matchGameUid VARCHAR(50),
    IN matchGameName VARCHAR(50),
    IN matchGameDescription VARCHAR(100),
    IN numberOfBoards INT,
    IN tileCountPerBoard INT,
    IN numberOfWinnableBoards INT,
    IN winProbabilityPerBoard INT,
    IN startDate DATETIME,
    IN startDateUtc DATETIME,
    IN endDate DATETIME,
    IN endDateUtc DATETIME
)
BEGIN
	INSERT INTO match_games
    (
		campaign_id,
        match_game_uid,
        match_game_name,
        match_game_description,
        number_of_boards,
        tile_count_per_board,
        number_of_winnable_boards,
        win_probability_per_board,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        created_date
    )
    VALUES
    (
		campaignId,
        matchGameUid,
        matchGameName,
        matchGameDescription,
        numberOfBoards,
        tileCountPerBoard,
        numberOfWinnableBoards,
        winProbabilityPerBoard,
        startDate,
        startDateUtc,
        endDate,
        endDateUtc,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_game_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_game_get_by_uid`(IN matchGameUid VARCHAR(50))
BEGIN
	SELECT
		M.match_game_id,
        C.campaign_id,
        C.campaign_uid,
        M.match_game_uid,
        M.match_game_name,
        M.match_game_description,
        M.number_of_boards,
        M.tile_count_per_board,
        M.number_of_winnable_boards,
        M.win_probability_per_board,
        M.is_generated,
        M.ready_to_generate,
        M.start_date,
        M.start_date_utc,
        M.end_date,
        M.end_date_utc
    FROM match_games as M
    INNER JOIN campaigns as C ON C.campaign_id = M.campaign_id
    WHERE M.match_game_uid = matchGameUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_game_set_as_generated` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_game_set_as_generated`(IN matchGameUid VARCHAR(50))
BEGIN
	UPDATE match_games
		SET is_generated = 1,
			ready_to_generate = 0,
            updated_date = CURRENT_TIMESTAMP()
    WHERE match_game_uid = matchGameUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_game_set_as_ready_to_be_generated` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_game_set_as_ready_to_be_generated`(IN matchGameUid VARCHAR(50))
BEGIN
	UPDATE match_games
		SET ready_to_generate = 1,
			updated_date = CURRENT_TIMESTAMP()
	WHERE match_game_uid = matchGameUid
	AND is_generated = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_game_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_game_update`(
    IN matchGameUid VARCHAR(50),
    IN matchGameName VARCHAR(50),
    IN matchGameDescription VARCHAR(100),
    IN numberOfBoards INT,
    IN tileCountPerBoard INT,
    IN numberOfWinnableBoards INT,
    IN winProbabilityPerBoard INT,
    IN startDate DATETIME,
    IN startDateUtc DATETIME,
    IN endDate DATETIME,
    IN endDateUtc DATETIME
)
BEGIN
	UPDATE match_games
		SET match_game_name = matchGameName,
			match_game_description = matchGameDescription,
            number_of_boards = numberOfBoards,
            tile_count_per_board = tileCountPerBoard,
            number_of_winnable_boards = numberOfWinnableBoards,
            win_probability_per_board = winProbabilityPerBoard,
            start_date = startDate,
            start_date_utc = startDateUtc,
            end_date = endDate,
            end_date_utc = endDateUtc,
            updated_date = CURRENT_TIMESTAMP()
    WHERE match_game_uid = matchGameUid
	AND is_generated = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_images_get_by_match_game_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_images_get_by_match_game_uid`(IN matchGameUid VARCHAR(50))
BEGIN
	SELECT
		MG.match_game_uid,
        MG.match_game_id,
        MI.image_id,
        MI.image_uid,
        MI.location_url
    FROM match_images as MI
    INNER JOIN match_games as MG ON MI.match_game_id = MG.match_game_id
    WHERE MG.match_game_uid = matchGameUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_image_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_image_create`(IN imageUid VARCHAR(50), IN matchGameUid VARCHAR(50), IN locationUrl VARCHAR(4000))
BEGIN
	DECLARE matchGameId INT DEFAULT 0;
    SELECT match_game_id INTO @matchGameId FROM match_games WHERE match_game_uid = matchGameUid;
    IF(@matchGameId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Match Game not found';
    END IF;
    
    INSERT INTO match_images
    (
		image_uid,
        match_game_id,
        location_url,
        created_date
    )
    VALUES
    (
		imageUid,
        @matchGameId,
        locationUrl,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_image_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_image_get_by_uid`(IN matchImageUid VARCHAR(50))
BEGIN
	SELECT
		MG.match_game_uid,
        MG.match_game_id,
        MI.image_id,
        MI.image_uid,
        MI.location_url
    FROM match_images as MI
    INNER JOIN match_games as MG ON MI.match_game_id = MG.match_game_id
    WHERE MI.image_uid = matchImageUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_prizes_get_by_match_game_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_prizes_get_by_match_game_uid`(IN matchGameUid VARCHAR(50))
BEGIN
	SELECT
		MP.prize_id,
        MP.prize_uid,
        MG.match_game_id,
        MG.match_game_uid,
        MP.prize_name,
        MP.prize_description,
        MP.prize_value,
        MI.image_id,
        MI.image_uid
    FROM match_prizes AS MP
    INNER JOIN match_games as MG ON MG.match_game_id = MP.match_game_id
    INNER JOIN match_images as MI ON MI.image_id = MP.match_image_id
    WHERE MG.match_game_uid = matchGameUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_prize_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_prize_create`(
	IN matchGameUid VARCHAR(50),
    IN matchPrizeUid VARCHAR(50),
    IN matchPrizeName VARCHAR(50),
    IN matchPrizeDescription VARCHAR(100),
    IN matchPrizeValue DECIMAL(18, 2),
    IN matchImageUid VARCHAR(50)
)
BEGIN
	DECLARE matchGameId INT DEFAULT 0;
    DECLARE matchImageId INT DEFAULT 0;
    
    SELECT match_game_id INTO @matchGameId FROM match_games WHERE match_game_uid = matchGameUid;
    IF(@matchGameId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Match Game not found';
    END IF;
    
    SELECT image_id INTO @matchImageId FROM match_images WHERE image_uid = matchImageUId;
    IF(@matchImageId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Match Image not found';
	END IF;
    
    INSERT INTO match_prizes
    (
		prize_uid,
        match_game_id,
        match_image_id,
        prize_name,
        prize_description,
        prize_value,
        created_date
    )
    VALUES
    (
		matchPrizeUid,
        @matchGameId,
        @matchImageId,
        matchPrizeName,
        matchPrizeDescription,
        matchPrizeValue,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_tiles_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_tiles_create`(
	IN tileUid VARCHAR(50),
    IN boardUid VARCHAR(50),
    IN imageUid VARCHAR(50),
    IN prizeUid VARCHAR(50),
    IN winIndicator bool)
BEGIN
    DECLARE prizeId INT DEFAULT 0;
    DECLARE boardId INT DEFAULT 0;
    DECLARE imageId INT DEFAULT 0;
    
    IF(prizeUid != '00000000-0000-0000-0000-000000000000') THEN
		SELECT prize_id INTO @prizeId FROM match_prizes WHERE prize_uid = prizeUid;
		IF(@accountId = 0) THEN
			SIGNAL SQLSTATE '45000'
				SET MESSAGE_TEXT = 'Match Prize not found';
		END IF;
	END IF;
    
    SELECT board_id INTO @boardId FROM match_boards WHERE board_uid = boardUid;
    IF(@boardId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Match board not found';
    END IF;
    
    SELECT image_id INTO @imageId FROM match_images WHERE image_uid = imageUid;
    IF(@imageId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Match image not found';
    END IF;
    
    INSERT INTO match_tiles
    (
		tile_uid,
        board_id,
        image_id,
        prize_id,
        win_indicator,
        created_date
    )
    VALUES
    (
		tileUid,
        @boardId,
        @imageId,
        @prizeId,
        winIndicator,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_tiles_get_by_board_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_tiles_get_by_board_uid`(IN boardUid VARCHAR(100))
BEGIN
	SELECT
		MT.tile_id,
        MT.tile_uid,
        MB.board_id,
        MB.board_uid,
        MI.image_id,
        MI.image_uid,
        MP.prize_id,
        MP.prize_uid,
        MT.win_indicator
    FROM match_tiles as MT
    INNER JOIN match_boards as MB ON MB.board_id = MT.board_id
    INNER JOIN match_images as MI ON MI.image_id = MT.image_id
    LEFT JOIN match_prizes as MP ON MP.prize_id = MT.prize_id
    INNER JOIN match_games as MG ON MG.match_game_id = MB.match_game_id
    WHERE MB.board_uid = boardUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `match_tiles_get_by_game_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `match_tiles_get_by_game_uid`(IN matchGameUid VARCHAR(50))
BEGIN
	SELECT
		MT.tile_id,
        MT.tile_uid,
        MB.board_id,
        MB.board_uid,
        MI.image_id,
        MI.image_uid,
        MP.prize_id,
        MP.prize_uid,
        MT.win_indicator
    FROM match_tiles as MT
    INNER JOIN match_boards as MB ON MB.board_id = MT.board_id
    INNER JOIN match_images as MI ON MI.image_id = MT.image_id
    INNER JOIN match_prizes as MP ON MP.prize_id = MT.prize_id
    INNER JOIN match_games as MG ON MG.match_game_id = MB.match_game_id
    WHERE MG.match_game_uid = matchGameUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `payment_services_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `payment_services_get`()
BEGIN
	SELECT * 
    FROM payment_services;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `plans_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `plans_get`()
BEGIN
	SELECT
		p.plan_id,
        p.plan_uid,
        p.plan_name,
        p.plan_description,
        p.promotions_per_month,
        p.sweeps_per_month,
        p.sweepstakes_maximum_entries,
        p.sweep_hashtags_per_month,
        p.price_per_month,
        p.price_per_year,
        fp.flutterwave_plan_id,
        fp.flutterwave_plan_uid,
        fp.external_monthly_payment_link,
        fp.external_yearly_payment_link
    FROM plans as p
    INNER JOIN flutterwave_plans as fp ON fp.plan_id = p.plan_id
    ORDER BY plan_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `plan_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `plan_get_by_uid`(IN planUid VARCHAR(50))
BEGIN
	SELECT
		p.plan_id,
        p.plan_uid,
        p.plan_name,
        p.plan_description,
        p.promotions_per_month,
        p.sweeps_per_month,
        p.sweepstakes_maximum_entries,
        p.sweep_hashtags_per_month,
        p.price_per_month,
        p.price_per_year,
        fp.flutterwave_plan_id,
        fp.flutterwave_plan_uid,
        fp.external_monthly_payment_link,
        fp.external_yearly_payment_link
    FROM plans as p
    INNER JOIN flutterwave_plans as fp ON fp.plan_id = p.plan_id
    WHERE p.plan_uid = planUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `plan_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `plan_update`(
	IN planUid VARCHAR(50),
    IN planName VARCHAR(100),
    IN planDescription VARCHAR(500),
    IN promotionsPerMonth INT,
    IN sweepsPerMonth INT,
    IN sweepsMaximumEntries INT,
    IN sweepHashtagsPerMonth INT,
    IN pricePerMonth INT,
    IN pricePerYear INT)
BEGIN
	UPDATE plans
		SET plan_name = planName,
			plan_description = planDescription,
            promotions_per_month = promotionsPerMonth,
            sweeps_per_month = sweepsPerMonth,
            sweep_hashtags_per_month = sweepHashtagsPerMonth,
            sweepstakes_maximum_entries = sweepsMaximumEntries,
            price_per_month = pricePerMonth,
            price_per_year = pricePerYear,
            updated_date = CURRENT_TIMESTAMP()
	WHERE plan_uid = planUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `profiles_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `profiles_get`(IN userUid VARCHAR(50))
BEGIN
	SELECT
		AP.profile_id,
        AP.profile_uid,
        AP.external_profile_id,
        A.account_id,
        A.account_uid,
        PT.name as profile_type,
        AP.display_name,
        AP.screen_name,
        AP.profile_description,
        AP.access_token,
        AP.access_token_secret,
        AP.location,
        AP.profile_image_url,
        AP.consumer_key,
        AP.consumer_secret
    FROM account_profiles as AP
    INNER JOIN profile_types as PT ON PT.profile_type_id = AP.profile_type_id
    INNER JOIN accounts as A ON A.account_id = AP.account_id
    WHERE A.account_uid = userUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `profile_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `profile_get`(IN userUid VARCHAR(50), IN profileType VARCHAR(100))
BEGIN
	SELECT
		AP.profile_id,
        AP.profile_uid,
        AP.external_profile_id,
        A.account_id,
        A.account_uid,
        PT.name as profile_type,
        AP.display_name,
        AP.screen_name,
        AP.profile_description,
        AP.access_token,
        AP.access_token_secret,
        AP.consumer_key,
        AP.consumer_secret,
        AP.location,
        AP.profile_image_url
    FROM account_profiles as AP
    INNER JOIN profile_types as PT ON PT.profile_type_id = AP.profile_type_id
    INNER JOIN accounts as A ON A.account_id = AP.account_id
    WHERE A.account_uid = userUid
    AND PT.name = profileType;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `profile_get_by_campaign_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `profile_get_by_campaign_uid`(IN campaignUid VARCHAR(50), IN profileType VARCHAR(100))
BEGIN
	SELECT
		AP.profile_id,
        AP.profile_uid,
        AP.external_profile_id,
        A.account_id,
        A.account_uid,
        PT.name as profile_type,
        AP.display_name,
        AP.screen_name,
        AP.profile_description,
        AP.access_token,
        AP.access_token_secret,
        AP.consumer_key,
        AP.consumer_secret,
        AP.location,
        AP.profile_image_url
    FROM account_profiles as AP
    INNER JOIN profile_types as PT ON PT.profile_type_id = AP.profile_type_id
    INNER JOIN accounts as A ON A.account_id = AP.account_id
    INNER JOIN campaigns as C ON C.account_id = A.account_id
    WHERE C.campaign_uid = campaignUid
    AND PT.name = profileType;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `profile_get_by_rule_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `profile_get_by_rule_uid`(IN ruleUid VARCHAR(50), IN profileType VARCHAR(100))
BEGIN
	SELECT
		AP.profile_id,
        AP.profile_uid,
        AP.external_profile_id,
        A.account_id,
        A.account_uid,
        PT.name as profile_type,
        AP.display_name,
        AP.screen_name,
        AP.profile_description,
        AP.access_token,
        AP.access_token_secret,
        AP.consumer_key,
        AP.consumer_secret,
        AP.location,
        AP.profile_image_url
    FROM account_profiles as AP
    INNER JOIN profile_types as PT ON PT.profile_type_id = AP.profile_type_id
    INNER JOIN accounts as A ON A.account_id = AP.account_id
    INNER JOIN campaigns as C ON C.account_id = A.account_id
    INNER JOIN sweep as S ON S.campaign_id = C.campaign_id
    INNER JOIN sweep_rules as R on R.sweep_id = S.sweep_id
    WHERE PT.name = profileType
    AND R.rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `profile_remove` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `profile_remove`(
	IN accountUid VARCHAR(50),
    IN profileType VARCHAR(100))
BEGIN
    DECLARE accountId INT DEFAULT 0;
    DECLARE profileTypeId INT DEFAULT 0;

    SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found';
    END IF;
    
    SELECT @profileTypeId := profile_type_id FROM profile_types WHERE name = profileType;
    IF(@profileTypeId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Profile type not found';
    END IF;
    
    DELETE FROM account_profiles
    WHERE account_id = @accountId
    AND profile_type_id = @profileTypeId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `profile_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `profile_upsert`(
	IN profileUid VARCHAR(50),
    IN externalProfileId VARCHAR(100),
	IN accountUid VARCHAR(50), 
    IN profileType VARCHAR(50), 
    IN displayName VARCHAR(100), 
    IN screenName VARCHAR(100), 
    IN description VARCHAR(1000), 
    IN accessToken VARCHAR(1000), 
    IN accessTokenSecret VARCHAR(1000),
    IN consumerKey VARCHAR(1000),
    IN consumerSecret VARCHAR(1000),
    IN location VARCHAR(100), 
    IN profileImageUrl VARCHAR(1000))
BEGIN
	DECLARE accountId INT DEFAULT 0;
    DECLARE profileTypeId INT DEFAULT 0;
    
    SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
    SELECT @profileTypeId := profile_type_id FROM profile_types WHERE name = profileType;
    
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found.';
    END IF;
    IF(@profileTypeId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Profile type not found.';
	END IF;
    
    INSERT INTO account_profiles
    (
		profile_uid,
        external_profile_id,
        account_id,
        profile_type_id,
        display_name,
        screen_name,
        profile_description,
        access_token,
        access_token_secret,
        consumer_key,
        consumer_secret,
        location,
        profile_image_url,
        created_date
    )
    VALUES
    (
		profileUid,
        externalProfileId,
        @accountId,
        @profileTypeId,
        displayName,
        screenName,
        description,
        accessToken,
        accessTokenSecret,
        consumerKey,
        consumerSecret,
        location,
        profileImageUrl,
        CURRENT_TIMESTAMP()
    )
    ON DUPLICATE KEY UPDATE
	  external_profile_id = externalProfileId,
	  display_name = displayName,
	  screen_name = screen_name,
      profile_description = description,
      access_token = accessToken,
      access_token_secret = accessTokenSecret,
      consumer_key = consumerKey,
      consumer_secret = consumerSecret,
      location = location,
      profile_image_url = profileImageUrl,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_image_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_image_create`(
	IN scheduledUid VARCHAR(50),
    IN imageLocation VARCHAR(5000)
)
BEGIN
	DECLARE scheduledId INT DEFAULT 0;

    SELECT @scheduledId := scheduled_id FROM scheduled_sweep_rule_tweets WHERE scheduled_uid = scheduledUid;
    IF(@scheduledId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Schedule not found';
	END IF;
    
    INSERT INTO scheduled_sweep_rule_tweet_images
    (
		scheduled_id,
        image_location,
        created_date
    )
    VALUES
    (
		@scheduledId,
        imageLocation,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweets_get_by_sweep_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweets_get_by_sweep_uid`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		S.scheduled_id,
        S.scheduled_uid,
        SR.rule_id,
        SR.rule_uid,
        S.tweet_text,
        S.scheduled_time,
        S.scheduled_time_utc,
        S.status,
        S.created_date,
        AP.profile_id,
        AP.profile_uid,
        AP.access_token,
        AP.access_token_secret
    FROM scheduled_sweep_rule_tweets AS S
    INNER JOIN sweep_rules AS SR ON SR.rule_id = S.rule_id
    INNER JOIN sweep as SW ON SW.sweep_id = SR.sweep_id
    INNER JOIN account_profiles as AP on AP.profile_id = S.profile_id
    WHERE SW.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweets_get_not_sent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweets_get_not_sent`(IN utcDate DATETIME)
BEGIN
	SELECT
		S.scheduled_id,
        S.scheduled_uid,
        SR.rule_id,
        SR.rule_uid,
        S.tweet_text,
        S.scheduled_time,
        S.scheduled_time_utc,
        S.status,
        S.created_date,
        AP.profile_id,
        AP.profile_uid,
        AP.access_token,
        AP.access_token_secret
    FROM scheduled_sweep_rule_tweets AS S
    INNER JOIN sweep_rules AS SR ON SR.rule_id = S.rule_id
    INNER JOIN account_profiles as AP on AP.profile_id = S.profile_id
    WHERE scheduled_time_utc <= CURRENT_TIMESTAMP()
    AND status = 'New';
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweet_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweet_create`(
	IN profileUid VARCHAR(50),
	IN scheduledUid VARCHAR(50),
    IN ruleUid VARCHAR(50),
    IN tweetText VARCHAR(500),
    IN scheduledTime DATETIME,
    IN scheduledTimeUtc DATETIME,
    IN status VARCHAR(100)
)
BEGIN
	DECLARE ruleId INT DEFAULT 0;
    DECLARE profileId INT DEFAULT 0;

    SELECT @ruleId := rule_id FROM sweep_rules WHERE rule_uid = ruleUid;
    IF(@ruleId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Rule not found';
	END IF;
    
    SELECT @profileId := profile_id FROM account_profiles WHERE profile_uid = profileUid;
    IF(@profileId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Profile not found';
	END IF;
    
    INSERT INTO scheduled_sweep_rule_tweets
    (
		scheduled_uid,
        profile_id,
        rule_id,
        tweet_text,
        scheduled_time,
        scheduled_time_utc,
        status,
        created_date
    )
    VALUES
    (
		scheduledUid,
        @profileId,
        @ruleId,
        tweetText,
        scheduledTime,
        scheduledTimeUtc,
        status,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweet_delete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweet_delete`(IN scheduledUid VARCHAR(50))
BEGIN
	DELETE FROM scheduled_sweep_rule_tweets
    WHERE scheduled_uid = scheduledUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweet_images_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweet_images_get`(IN scheduledId INT)
BEGIN
	SELECT
		scheduled_image_id,
        scheduled_id,
        image_location,
        created_date
    FROM scheduled_sweep_rule_tweet_images
    WHERE scheduled_id = scheduledId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweet_image_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweet_image_update`(IN scheduledImageId INT, IN imageLocation VARCHAR(5000))
BEGIN
	UPDATE scheduled_sweep_rule_tweet_images
		SET image_location = imageLocation,
			updated_date = CURRENT_TIMESTAMP()
	WHERE scheduled_image_id = scheduledImageId; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweet_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweet_update`(
	IN scheduledUid VARCHAR(50), IN scheduledTime DATETIME, IN scheduledTimeUtc DATETIME, IN tweetText VARCHAR(500))
BEGIN
	UPDATE scheduled_sweep_rule_tweets
		SET tweet_text = tweetText,
			scheduled_time = scheduledTime,
            scheduled_time_utc = scheduledTimeUtc,
			updated_date = CURRENT_TIMESTAMP()
	WHERE scheduled_uid = scheduledUid; 
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweet_update_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_sweep_rule_tweet_update_status`(IN scheduledUid VARCHAR(50), IN newStatus VARCHAR(100))
BEGIN
	UPDATE scheduled_sweep_rule_tweets
		SET status = newStatus,
			updated_date = CURRENT_TIMESTAMP()
	WHERE scheduled_uid = scheduledUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_tweet_get_by_rule_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `scheduled_tweet_get_by_rule_id`(IN ruleId INT)
BEGIN
	SELECT
		S.scheduled_id,
        S.scheduled_uid,
        SR.rule_id,
        SR.rule_uid,
        S.tweet_text,
        S.scheduled_time,
        S.scheduled_time_utc,
        S.status,
        S.created_date,
        AP.profile_id,
        AP.profile_uid,
        AP.access_token,
        AP.access_token_secret
    FROM scheduled_sweep_rule_tweets AS S
    INNER JOIN sweep_rules AS SR ON SR.rule_id = S.rule_id
    INNER JOIN account_profiles as AP on AP.profile_id = S.profile_id
    WHERE S.rule_id = ruleId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_draw_rule_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_draw_rule_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		SDR.sweep_draw_rule_id,
        SDR.sweep_draw_rule_uid,
        SDR.winner_count,
        SDR.winner_backup_count,
        SDR.created_date,
        SDR.updated_date
    FROM sweep_draw_rules as SDR
    INNER JOIN sweep as S ON S.sweep_id = SDR.sweep_id
    WHERE S.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_draw_rule_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_draw_rule_get_by_uid`(IN drawRuleUid VARCHAR(50))
BEGIN
	SELECT
		sweep_draw_rule_id,
        sweep_draw_rule_uid,
        sweep_id,
        winner_count,
        winner_backup_count,
        created_date,
        updated_date
    FROM sweep_draw_rules
    WHERE sweep_draw_rule_uid = drawRuleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_get_by_accountUid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_get_by_accountUid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT
		s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        c.campaign_uid,
        c.campaign_name,
        ct.campaign_type_code,
        ct.campaign_type_name,
        c.active_ind,
        c.start_date as campaign_start_date,
        c.end_date as campaign_end_date,
        a.account_uid,
        a.email_address,
        a.phone_number,
        a.display_name
    FROM sweep as s
    INNER JOIN accounts as a ON a.account_id = s.account_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as ct on ct.campaign_type_id = c.campaign_type_id
	WHERE a.account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_get_by_campaignUid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_get_by_campaignUid`(IN campaignUid VARCHAR(50))
BEGIN
	SELECT
		s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        c.campaign_uid,
        c.campaign_name,
        ct.campaign_type_code,
        ct.campaign_type_name,
        c.active_ind,
        c.start_date as campaign_start_date,
        c.end_date as campaign_end_date,
        a.account_uid,
        a.email_address,
        a.phone_number,
        a.display_name
    FROM sweep as s
    INNER JOIN accounts as a ON a.account_id = s.account_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as ct on ct.campaign_type_id = c.campaign_type_id
    WHERE c.campaign_uid = campaignUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_going_live_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_going_live_get`(IN hourValue INT)
BEGIN
	SELECT
		SW.*,
		c.campaign_uid,
		c.campaign_name,
		ct.campaign_type_code,
		ct.campaign_type_name,
		c.active_ind,
		c.start_date as campaign_start_date,
		c.end_date as campaign_end_date,
		a.account_uid,
		a.email_address,
		a.phone_number,
		a.display_name
	FROM
	(
	SELECT
		s.sweep_id,
		s.sweep_uid,
		s.sweep_name,
		s.rules_url,
		s.start_date,
		s.start_date_utc,
		s.end_date,
		s.end_date_utc,
		s.draw_date,
		s.draw_date_utc,
		s.draw_attempted,
		s.not_enough_unique_entrants,
		s.notification_sent_going_live,
		s.campaign_id,
		TIMESTAMPDIFF(MINUTE, UTC_TIMESTAMP(), S.start_date_utc) as time_difference
	FROM sweep as s
	) as SW
	INNER JOIN campaigns as c ON c.campaign_id = SW.campaign_id
	INNER JOIN campaign_types as ct on ct.campaign_type_id = c.campaign_type_id
	INNER JOIN accounts as a ON a.account_id = c.account_id
	WHERE time_difference BETWEEN (hourValue * -60 - 1) AND (hourvalue * 60);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_live_not_notified` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_live_not_notified`()
BEGIN
    SELECT
		A.*,
        c.campaign_uid,
        c.campaign_name,
        ct.campaign_type_code,
        ct.campaign_type_name,
        c.active_ind,
        c.start_date as campaign_start_date,
        c.end_date as campaign_end_date,
        ac.account_uid,
        ac.email_address,
        ac.phone_number,
        ac.display_name
	FROM 
	(
		SELECT
			s.sweep_id,
			s.sweep_uid,
			s.sweep_name,
            s.campaign_id,
			s.rules_url,
			s.start_date,
			s.start_date_utc,
			s.end_date,
			s.end_date_utc,
			s.draw_date,
			s.draw_date_utc,
			s.notification_sent_going_live,
			SN.not_enough_unique_entrants,
			SN.sweeps_going_live,
			SN.sweeps_live,
			SN.draw_attempted,
			SN.draw_happened,
			TIMESTAMPDIFF(minute, UTC_TIMESTAMP(), s.start_date_utc) as time_difference
		FROM sweep AS S
		LEFT JOIN sweep_notifications AS SN ON SN.sweep_id = S.sweep_id
		WHERE (sweeps_live = 0 OR sweeps_live IS NULL)

	) as A
    INNER JOIN campaigns as C on C.campaign_id = A.campaign_id
    INNER JOIN accounts as Ac ON Ac.account_id = C.account_id
    INNER JOIN campaign_types as ct on ct.campaign_type_id = C.campaign_type_id
	WHERE time_difference > -60 
	AND time_difference < 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_requiring_draw_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweeps_requiring_draw_get`()
BEGIN
	SELECT
		S.sweep_id,
		S.sweep_uid,
		S.sweep_name,
		S.rules_url as sweep_rule_url,
		S.start_date as sweep_start_date,
        S.start_date_utc as sweep_start_date_utc,
		S.end_date as sweep_end_date,
        S.end_date_utc as sweep_end_date_utc,
		S.draw_date as sweep_draw_date,
        S.draw_date_utc as sweep_draw_date_utc,
		S.drawn_ind as sweep_drawn_indicator,
		S.date_drawn as sweep_date_drawn,
        S.draw_attempted,
        S.not_enough_unique_entrants,
        S.notification_sent_going_live,
		C.campaign_id,
		C.campaign_uid,
		C.campaign_name,
		C.campaign_description,
		C.active_ind as campaign_active_ind,
		C.start_date as campaign_start_date,
		C.end_date as campaign_end_date,
		CT.campaign_type_code,
		CT.campaign_type_name,
		CT.campaign_type_description,
		SR.watch_value as sweep_rule_watch_value,
		SR.active_ind as sweep_rule_active_indidcator,
		SR.external_id,
		SR.must_be_following,
		SR.notify_winners,
		WT.watch_type_id,
		WT.watch_type_code,
		WT.watch_type_name,
		SDR.sweep_draw_rule_id,
		SDR.sweep_draw_rule_uid,
		SDR.winner_count,
		SDR.winner_backup_count,
        A.account_id,
        A.account_uid,
        A.email_address
	FROM sweep as S
	INNER JOIN campaigns as C ON C.campaign_id = S.campaign_id
	INNER JOIN campaign_types as CT ON CT.campaign_type_id = c.campaign_type_id
	INNER JOIN sweep_rules as SR ON SR.sweep_id = S.sweep_id
	INNER JOIN sweep_draw_rules as SDR ON SDR.sweep_id = S.sweep_id
	INNER JOIN watch_types as WT ON WT.watch_type_id = SR.watch_type_id
    INNER JOIN accounts as A ON A.account_id = C.account_id
	WHERE UTC_TIMESTAMP() > S.draw_date_utc
	AND S.drawn_ind = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_add_entry` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_add_entry`(
	IN sweepEntryUid VARCHAR(50), 
	IN sweepUid VARCHAR(50),
    IN ruleUid VARCHAR(50),
	IN userIdentifier VARCHAR(500), 
	IN userIdentifierType VARCHAR(100), 
	IN acceptedTerms BIT)
BEGIN
	DECLARE sweepId INT DEFAULT 0;
    DECLARE ruleId INT DEFAULT 0;
    
    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;
    SELECT @ruleId := rule_id FROM sweep_rules WHERE rule_uid = ruleUid;

    IF(@sweepId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found';
	END IF;
    
    IF(@ruleId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Rule not found';
	END IF;
    
    INSERT INTO sweep_entry
    (
		sweep_entry_uid,
        sweep_id,
        rule_id,
        user_identifier,
        user_identifier_type,
        accepted_terms,
        created_date
    )
    SELECT
		sweepEntryUid,
        @sweepId,
        @ruleId,
        userIdentifier,
        userIdentifierType,
        acceptedTerms,
        CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_backup_winners_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_backup_winners_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		swb.sweep_winner_backup_id,
        swb.sweep_winner_id,
        swb.sweep_entry_uid,
        swb.user_identifier,
        swb.user_identifier_type,
        swb.selected_ind,
        sw.sweep_winner_uid
    FROM sweep_winners_backup as swb
    INNER JOIN sweep_winners as sw ON sw.sweep_winner_id = swb.sweep_winner_id
    INNER JOIN sweep as S on S.sweep_id = sw.sweep_id
    WHERE S.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_backup_winner_promote` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_backup_winner_promote`(
	IN sweepWinnerUid VARCHAR(50), 
    IN sweepEntryUid VARCHAR(50),
    IN userIdentifier VARCHAR(100),
    IN userIdentifierType VARCHAR(100))
BEGIN
	UPDATE sweep_winners
		SET sweep_entry_uid = sweepEntryUid,
			user_identifier = userIdentifier,
            user_identifier_type = userIdentifierType,
            backup_win = 1,
            updated_date = CURRENT_TIMESTAMP()
	WHERE sweep_winner_uid = sweepWinnerUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_backup_winner_selected` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_backup_winner_selected`(IN sweepWinnerBackupId INT)
BEGIN
	UPDATE sweep_winners_backup
		SET selected_ind = 1,
			updated_date = CURRENT_TIMESTAMP()
	WHERE sweep_winner_backup_id = sweepWinnerBackupId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_create`(
	IN sweepUid VARCHAR(50), 
    IN campaignUid VARCHAR(50), 
    IN accountUid VARCHAR(50), 
    IN sweepName VARCHAR(100), 
    IN startDate DATETIME, 
    IN startDateUtc DATETIME,
    IN endDate DATETIME, 
    IN endDateUtc DATETIME,
    IN drawDate DATETIME,
    IN drawDateUtc DATETIME)
BEGIN
	DECLARE campaignId INT DEFAULT 0;
    DECLARE accountId INT DEFAULT 0;

    SELECT campaign_id INTO @campaignId FROM campaigns WHERE campaign_uid = campaignUid;
    IF(@campaignId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
    
    SELECT account_id INTO @accountId FROM accounts WHERE account_uid = accountUid;
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found';
    END IF;
    
	INSERT INTO sweep
	(
		sweep_uid,
        campaign_id,
        account_id,
        sweep_name,
        start_date,
        start_date_utc,
        end_date,
        end_date_utc,
        draw_date,
        draw_date_utc,
        created_date
    )
    VALUES
    (
		sweepUid,
        @campaignId,
        @accountId,
        sweepName,
        startDate,
        startDateUtc,
        endDate,
        endDateUtc,
        drawDate,
        drawDateUtc,
		CURRENT_TIMESTAMP()
	);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_draw_rule_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_draw_rule_create`(IN sweepDrawRuleUid VARCHAR(50), IN sweepUid VARCHAR(50), IN winnerCount INT, IN winnerBackupCount INT)
BEGIN
	DECLARE sweepId INT DEFAULT 0;
    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;
    
    IF(@sweepId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found.';
    END IF;
    
    INSERT INTO sweep_draw_rules
    (
		sweep_draw_rule_uid,
        sweep_id,
        winner_count,
        winner_backup_count,
        created_date
    )
    VALUES
    (
		sweepDrawRuleUid,
        @sweepId,
        winnerCount,
        winnerBackupCount,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_draw_rule_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_draw_rule_update`(IN drawRuleUid VARCHAR(50), IN winnerCount INT, in winnerBackupCount INT)
BEGIN
	UPDATE sweep_draw_rules
		SET winner_count = winnerCount,
			winner_backup_count = winnerBackupCount,
            updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_draw_rule_uid = drawRuleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entries_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entries_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		se.sweep_entry_id,
        se.sweep_entry_uid,
        se.user_identifier,
        se.user_identifier_type,
        se.accepted_terms,
        se.created_date,
        se.updated_date,
        s.sweep_uid,
        s.sweep_id,
        s.sweep_name,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        s.created_date as sweep_created_date
    FROM sweep_entry as se
    INNER JOIN sweep as s ON se.sweep_id = s.sweep_id
    WHERE s.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entries_get_by_account_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entries_get_by_account_uid`(IN accountUid VARCHAR(50))
BEGIN
	SELECT
		se.sweep_entry_id,
        se.sweep_entry_uid,
        se.user_identifier,
        se.user_identifier_type,
        se.accepted_terms,
        se.created_date,
        se.updated_date,
        s.sweep_uid,
        s.sweep_id,
        s.sweep_name,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        s.created_date as sweep_created_date
    FROM sweep_entry as se
    INNER JOIN sweep as s ON se.sweep_id = s.sweep_id
    INNER JOIN campaigns as C ON C.campaign_id = s.campaign_id
    INNER JOIN accounts as A ON A.account_id = C.account_id
    WHERE A.account_uid = accountUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entry_report_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entry_report_get`(
    IN sweepUid VARCHAR(50), 
    IN userIdentifierType VARCHAR(100), 
    IN externalUserId VARCHAR(100), 
    IN screenName VARCHAR(100))
BEGIN
	SELECT RSE.*
    FROM reporting_sweep_entries as RSE
    INNER JOIN sweep as S on S.sweep_id = RSE.sweep_id
    where S.sweep_uid = sweepUid
    AND user_identifier_type = userIdentifierType
    AND external_user_id = externalUserId
    AND screen_name = screenName;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entry_report_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entry_report_upsert`(IN sweepEntryReportUid VARCHAR(50), IN sweepUid VARCHAR(50), IN screenName VARCHAR(100), IN userIdentifierType VARCHAR(100), IN externalId VARCHAR(100), IN externalUserId VARCHAR(100), IN count INT)
BEGIN
	DECLARE sweepId INT DEFAULT 0;
    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;
    IF(@sweepId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found.';
    END IF;

	INSERT INTO sweep_entries
    (
		sweep_entry_uid,
        sweep_id,
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
        @sweepId,
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entry_rule_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entry_rule_create`(IN entryRuleUid VARCHAR(50), IN sweepUid VARCHAR(50), IN entryPerUser INT)
BEGIN

	DECLARE sweepId INT DEFAULT 0;
    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;
    
    IF(@sweepId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found.';
    END IF;

	INSERT INTO sweep_entry_rules
    (
		entry_rule_uid,
        sweep_id,
        entry_per_user,
        created_date
    )
    VALUES
    (
		entryRuleUid,
        @sweepId,
        entryPerUser,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entry_rule_get_by_sweep_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entry_rule_get_by_sweep_uid`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		SER.entry_rule_id,
        SER.entry_rule_uid,
        SER.entry_per_user,
        SER.created_date,
        SER.updated_date
    FROM sweep_entry_rules as SER
    INNER JOIN sweep as S ON S.sweep_id = SER.sweep_id
    WHERE S.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entry_rule_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_entry_rule_update`(IN entryRuleUid VARCHAR(50), IN sweepUid VARCHAR(50), IN sweepId INT, IN entryPerUser INT)
BEGIN
	UPDATE sweep_entry_rules
		SET entry_per_user = entryPerUser,
			updated_date = CURRENT_TIMESTAMP()
	WHERE entry_rule_uid = entryRuleUid
    AND sweep_id = sweepId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_get_by_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_get_by_uid`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        c.campaign_uid,
        c.campaign_name,
        ct.campaign_type_code,
        ct.campaign_type_name,
        c.active_ind,
        c.start_date as campaign_start_date,
        c.end_date as campaign_end_date,
        a.account_uid,
        a.email_address,
        a.phone_number,
        a.display_name
    FROM sweep as s
    INNER JOIN accounts as a ON a.account_id = s.account_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as ct ON ct.campaign_type_id = c.campaign_type_id
    WHERE s.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_mark_as_draw_attempted` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_mark_as_draw_attempted`(IN sweepUid VARCHAR(50))
BEGIN
	UPDATE sweep
		SET draw_attempted = 1,
			updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_mark_as_not_enough_unique_entrants` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_mark_as_not_enough_unique_entrants`(IN sweepUid VARCHAR(50))
BEGIN
	UPDATE sweep
		SET not_enough_unique_entrants = 1,
			updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_notification_draw_attempted_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_notification_draw_attempted_upsert`(IN sweepId INT)
BEGIN
	
	INSERT INTO sweep_notifications
	(
		sweep_id,
		draw_attempted,
		created_date
	)
	VALUES
	(
		sweepId,
		1,
		CURRENT_TIMESTAMP()
	)
    ON DUPLICATE KEY UPDATE
	  draw_attempted = 1,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_notification_draw_happened_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_notification_draw_happened_upsert`(IN sweepId INT)
BEGIN
	INSERT INTO sweep_notifications
	(
		sweep_id,
		draw_happened,
		created_date
	)
	VALUES
	(
		sweepId,
		1,
		CURRENT_TIMESTAMP()
	)
    ON DUPLICATE KEY UPDATE
	  draw_happened = 1,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_notification_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_notification_get`(IN sweepId INT)
BEGIN
	SELECT *
    FROM sweep_notifications
    WHERE sweep_id = sweepId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_notification_not_enough_unique_entrants_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_notification_not_enough_unique_entrants_upsert`(IN sweepId INT)
BEGIN
	INSERT INTO sweep_notifications
	(
		sweep_id,
		not_enough_unique_entrants,
		created_date
	)
	VALUES
	(
		sweepId,
		1,
		CURRENT_TIMESTAMP()
	)
    ON DUPLICATE KEY UPDATE
	  not_enough_unique_entrants = 1,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_notification_sweep_going_live_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_notification_sweep_going_live_upsert`(IN sweepId INT)
BEGIN
	INSERT INTO sweep_notifications
	(
		sweep_id,
		sweeps_going_live,
		created_date
	)
	VALUES
	(
		sweepId,
		1,
		CURRENT_TIMESTAMP()
	)
    ON DUPLICATE KEY UPDATE
	  sweeps_going_live = 1,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_notification_sweep_live_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_notification_sweep_live_upsert`(IN sweepId INT)
BEGIN
	INSERT INTO sweep_notifications
	(
		sweep_id,
		sweeps_live,
		created_date
	)
	VALUES
	(
		sweepId,
		1,
		CURRENT_TIMESTAMP()
	)
    ON DUPLICATE KEY UPDATE
	  sweeps_live = 1,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prizes_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_create`(
	IN prizeUid VARCHAR(50), 
    IN sweepUid VARCHAR(50), 
    IN enabledInd BIT, 
    IN prizeName VARCHAR(100), 
    IN prizeDescription VARCHAR(500),
	IN prizeValue DECIMAL(18, 2), 
    IN locale VARCHAR(45), 
    IN prizePosition INT)
BEGIN
	DECLARE sweepId INT DEFAULT 0;

    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;
    IF(@sweepId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found';
	END IF;

	INSERT INTO sweep_prizes
    (
		prize_uid,
        sweep_id,
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
        @sweepId,
        enabledInd,
        prizePosition,
        prizeName,
        prizeDescription,
        prizeValue,
        locale,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prizes_delete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_delete`(IN prizeUid VARCHAR(50))
BEGIN
	DELETE FROM sweep_prizes WHERE prize_uid = prizeUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prizes_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		SP.prize_id,
        SP.prize_uid,
        SP.prize_position,
        SP.enabled_ind,
        SP.prize_name,
        SP.prize_description,
        SP.prize_value,
        SP.locale,
        SP.created_date,
        SP.updated_date,
        S.sweep_id,
        S.sweep_name,
        S.sweep_uid,
        S.rules_url,
        S.start_date,
        S.start_date_utc,
        S.end_date,
        S.end_date_utc,
        S.draw_date,
        S.draw_date_utc,
        S.drawn_ind,
        S.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live
    FROM sweep_prizes as SP
    INNER JOIN sweep as S ON S.sweep_id = SP.sweep_id
    WHERE S.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prizes_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_update`(IN prizeUid VARCHAR(50), IN prizeName VARCHAR(100), IN prizeDescription VARCHAR(5000), IN prizeValue DECIMAL(18, 2))
BEGIN
	UPDATE sweep_prizes
		SET prize_name = prizeName,
			prize_description = prizeDescription,
            prize_value = prizeValue,
            updated_date = CURRENT_TIMESTAMP()
	WHERE prize_uid = prizeUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prize_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prize_get`(IN prizeUid VARCHAR(50))
BEGIN
	SELECT
		SP.prize_id,
        SP.prize_uid,
        SP.prize_position,
        SP.sweep_id,
        SP.enabled_ind,
        SP.prize_name,
        SP.prize_description,
        SP.prize_value,
        SP.locale,
        S.sweep_uid
    FROM sweep_prizes AS SP
    INNER JOIN sweep AS S ON S.sweep_id = SP.sweep_id
    WHERE prize_uid = prizeUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rules_get`(IN currentUtc DATETIME)
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.start_date_utc as campaign_start_date_utc,
        c.end_date as campaign_end_date,
        c.end_date_utc as campaign_end_date_utc,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.notify_winners,
        r.external_id,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date,
        a.account_uid,
        a.email_address,
        a.display_name,
        a.first_name,
        a.last_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as A on A.account_id = c.account_id
    WHERE (CURRENT_TIMESTAMP() BETWEEN s.start_date_utc AND s.end_date_utc)
    AND (CURRENT_TIMESTAMP() < s.draw_date_utc)
    AND s.drawn_ind != 1
    AND r.external_id IS NOT NULL;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_get_by_sweepUid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rules_get_by_sweepUid`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.start_date_utc as campaign_start_date_utc,
        c.end_date as campaign_end_date,
        c.end_date_utc as campaign_end_date_utc,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.created_date,
        r.updated_date,
        r.external_id,
        r.must_be_following,
        r.notify_winners,
        wt.watch_type_id,
        wt.watch_type_code,
        wt.watch_type_name,
        a.account_uid,
        a.email_address,
        a.display_name,
        a.first_name,
        a.last_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON wt.watch_type_id = r.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as A on A.account_id = c.account_id
    WHERE s.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_get_by_watchvalue` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rules_get_by_watchvalue`(IN watchValue VARCHAR(500))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.start_date_utc as campaign_start_date_utc,
        c.end_date as campaign_end_date,
        c.end_date_utc as campaign_end_date_utc,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
		r.external_id,
        wv.watch_type_id,
        wv.watch_type_code,
        wv.watch_type_name,
        r.must_be_following,
        r.notify_winners,
        r.created_date,
        r.updated_date,
        a.account_uid,
        a.email_address,
        a.display_name,
        a.first_name,
        a.last_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wv ON wv.watch_type_id = r.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as A on A.account_id = c.account_id
    WHERE r.watch_value = watchValue;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_get_by_watch_type_code` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rules_get_by_watch_type_code`(IN watchTypeCode VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.start_date_utc as campaign_start_date_utc,
        c.end_date as campaign_end_date,
        c.end_date_utc as campaign_end_date_utc,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
		r.external_id,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.notify_winners,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date,
        a.account_uid,
        a.email_address,
        a.display_name,
        a.first_name,
        a.last_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as A on A.account_id = c.account_id
    WHERE wt.watch_type_code = watchTypeCode;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_get_not_added_for_tracking` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rules_get_not_added_for_tracking`(IN watchTypeCode VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
		r.rule_uid,
		c.campaign_id,
		c.campaign_uid,
		c.campaign_name,
		c.campaign_description,
		c.active_ind as campaign_active_ind,
		c.start_date as campaign_start_date,
		c.end_date as campaign_end_date,
		ct.campaign_type_code,
		ct.campaign_type_name,
		ct.campaign_type_description,
		s.sweep_id,
		s.sweep_uid,
		s.sweep_name,
		s.rules_url,
		s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
		s.drawn_ind,
		s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
		r.watch_value,
		r.active_ind,
		r.scheduled_ind,
		r.external_id,
		wv.watch_type_id,
		wv.watch_type_code,
		wv.watch_type_name,
		r.must_be_following,
        r.notify_winners,
		r.created_date,
		r.updated_date
	FROM sweep_rules as r
	INNER JOIN watch_types as wv ON r.watch_type_id = wv.watch_type_id
	INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
	INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
	INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    WHERE wt.watch_type_code = watchTypeCode;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_unadded_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rules_unadded_get`()
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.end_date as campaign_end_date,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.notify_winners,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date
    FROM rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    WHERE r.added_for_tracking = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_activate` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_activate`(IN ruleUid VARCHAR(50))
BEGIN
	UPDATE sweep_rules as S
		SET S.active_ind = 1,
			S.updated_date = current_timestamp()
    WHERE S.rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_create`(
	IN ruleUid VARCHAR(50), 
    IN sweepUid VARCHAR(50), 
    IN watchTypeCode VARCHAR(50), 
    IN externalId VARCHAR(100), 
    IN watchValue VARCHAR(500), 
    IN activeInd BIT,
    IN mustBeFollowing BIT,
    IN scheduledInd BIT)
BEGIN
	DECLARE watchTypeId INT DEFAULT 0;
    DECLARE sweepId INT DEFAULT 0;
	
    SELECT @watchTypeId := watch_type_id FROM watch_types WHERE watch_type_code = watchTypeCode;
    SELECT @sweepId := sweep_id FROM sweep WHERE sweep_uid = sweepUid;

    IF(@watchTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Watch type not found';
	END IF;
    
    IF(@sweepId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep not found.';
    END IF;
    
    INSERT INTO sweep_rules
    (
		rule_uid,
        sweep_id,
        watch_type_id,
        watch_value,
        active_ind,
        scheduled_ind,
        external_id,
        must_be_following,
        created_date
    )
    SELECT
		ruleUid,
        @sweepId,
        @watchTypeId,
        watchValue,
        activeInd,
        scheduledInd,
        externalId,
        mustBeFollowing,
        CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_deactivate` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_deactivate`(IN ruleUid VARCHAR(50))
BEGIN
	UPDATE sweep_rules as S
		SET S.active_ind = 0,
			S.updated_date = current_timestamp()
    WHERE S.rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_delete` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_delete`(IN ruleUid VARCHAR(50))
BEGIN
	DELETE FROM sweep_rules WHERE rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_get`(IN ruleUid VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.start_date_utc as campaign_start_date_utc,
        c.end_date as campaign_end_date,
        c.end_date_utc as campaign_end_date_utc,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.notify_winners,
        r.external_id,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date,
        a.account_uid,
        a.email_address,
        a.display_name,
        a.first_name,
        a.last_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON wt.watch_type_id = r.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as A on A.account_id = c.account_id
    WHERE r.rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_get_matching` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_get_matching`(IN watchValue VARCHAR(50))
BEGIN
	SELECT
		r.rule_id,
        r.rule_uid,
        c.campaign_id,
        c.campaign_uid,
        c.campaign_name,
        c.campaign_description,
        c.active_ind as campaign_active_ind,
        c.start_date as campaign_start_date,
        c.start_date_utc as campaign_start_date_utc,
        c.end_date as campaign_end_date,
        c.end_date_utc as campaign_end_date_utc,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
		s.start_date,
        s.start_date_utc,
        s.end_date,
        s.end_date_utc,
        s.draw_date,
        s.draw_date_utc,
        s.drawn_ind,
        s.date_drawn,
        s.draw_attempted,
        s.not_enough_unique_entrants,
        s.notification_sent_going_live,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.notify_winners,
        r.external_id,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date,
        a.account_uid,
        a.email_address,
        a.display_name,
        a.first_name,
        a.last_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    INNER JOIN accounts as A on A.account_id = c.account_id
    WHERE r.watch_value = watchValue;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rule_update_external_id` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_rule_update_external_id`(IN ruleUid VARCHAR(50), IN externalId VARCHAR(100))
BEGIN
	UPDATE sweep_rules
		SET external_id = externalId,
			updated_date = CURRENT_TIMESTAMP()
	WHERE rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_toggle_draw_attempted` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_toggle_draw_attempted`(IN sweepUid VARCHAR(50), IN newValue TINYINT)
BEGIN
	UPDATE sweep
		SET draw_attempted = newValue,
			updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_toggle_notification_going_live_sent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_toggle_notification_going_live_sent`(IN sweepUid VARCHAR(50), IN notificationSent TINYINT)
BEGIN
	UPDATE sweep
		SET notification_sent_going_live = notificationSent,
			updated_date = CURRENT_TIMESTAMP()
	WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_toggle_not_enough_entries` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_toggle_not_enough_entries`(IN sweepUid VARCHAR(50), IN newValue TINYINT)
BEGIN
	UPDATE sweep
		SET not_enough_unique_entrants = newValue,
			updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_update`(
	IN sweepUid VARCHAR(50), 
    IN sweepName VARCHAR(100), 
    IN startDate DATETIME, 
    IN startDateUtc DATETIME,
    IN endDate DATETIME, 
    IN endDateUtc DATETIME,
    IN drawDate DATETIME,
    IN drawDateUtc DATETIME)
BEGIN
	UPDATE sweep
		SET sweep_name = sweepName,
			start_date = startDate,
            start_date_utc = startDateUtc,
            end_date = endDate,
            end_date_utc = endDateUtc,
            draw_date = drawDate,
            draw_date_utc = drawDateUtc,
            updated_date = CURRENT_TIMESTAMP()
    WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_update_as_drawn` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_update_as_drawn`(IN sweepUid VARCHAR(50))
BEGIN
	UPDATE sweep
		SET drawn_ind = 1,
			date_drawn = CURRENT_TIMESTAMP(),
            updated_date = CURRENT_TIMESTAMP()
	WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winners_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winners_create`(
	IN sweepWinnerUid VARCHAR(50), IN sweepId VARCHAR(50), IN sweepEntryUid VARCHAR(50), IN assignedId INT, IN userIdentifier VARCHAR(100), IN userIdentifierType VARCHAR(100))
BEGIN
	INSERT INTO sweep_winners
    (
		sweep_winner_uid,
        sweep_id,
        sweep_entry_uid,
        assigned_id,
        user_identifier,
        user_identifier_type,
        fulfilled_ind,
        created_date
    )
    VALUES
    (
		sweepWinnerUid,
        sweepId,
        sweepEntryUid,
        assignedId,
        userIdentifier,
        userIdentifierType,
        0,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winners_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winners_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT
		SW.sweep_winner_id,
        SW.sweep_winner_uid,
        SW.sweep_entry_uid,
        S.sweep_id,
        SW.assigned_id,
        SW.fulfilled_ind,
        SW.user_identifier,
        SW.user_identifier_type,
        SW.created_date,
        SW.updated_date
    FROM sweep_winners as SW
    INNER JOIN sweep as S ON S.sweep_id = SW.sweep_id
    WHERE S.sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winner_backups_get_by_sweep_winner_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winner_backups_get_by_sweep_winner_uid`(IN sweepWinnerUid VARCHAR(50))
BEGIN
	SELECT
		swb.sweep_winner_backup_id,
        swb.sweep_winner_id,
        swb.sweep_entry_uid,
        S.sweep_uid,
        S.sweep_id,
        swb.user_identifier,
        swb.user_identifier_type,
        swb.selected_ind,
        swb.created_date,
        swb.updated_date
    FROM sweep_winners_backup as swb
    INNER JOIN sweep_winners as sw ON sw.sweep_winner_id = swb.sweep_winner_id
    INNER JOIN sweep as S on sw.sweep_id = S.sweep_id
    WHERE sw.sweep_winner_uid = sweepWinnerUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winner_backup_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winner_backup_create`(IN sweepWinnerUid VARCHAR(50), IN sweepId INT, IN sweepEntryUid VARCHAR(50), IN userIdentifier VARCHAR(100), 
	IN userIdentifierType VARCHAR(100), IN assignedId INT)
BEGIN
	DECLARE sweepWinnerId INT DEFAULT 0;
	
    SELECT @sweepWinnerId := sweep_winner_id FROM sweep_winners WHERE sweep_winner_uid = sweepWinnerUid;

    IF(@sweepWinnerId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Sweep Winner not found';
	END IF;
    
    INSERT INTO sweep_winners_backup
    (
		sweep_winner_id,
        sweep_id,
        sweep_entry_uid,
        user_identifier,
        user_identifier_type,
        selected_ind,
        assigned_id,
        created_date
    )
    VALUES
    (
		@sweepWinnerId,
        sweepId,
        sweepEntryUid,
        userIdentifier,
        useridentifierType,
        0,
        assignedId,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winner_fulfill` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winner_fulfill`(IN sweepWinnerUid VARCHAR(50), IN prizeId INT)
BEGIN
	UPDATE sweep_winners
		SET fulfilled_ind = 1,
			fulfilled_prize_id = prizeId,
            updated_date = CURRENT_TIMESTAMP()
	WHERE sweep_winner_uid = sweepWinnerUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winner_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winner_get`(IN sweepWinnerUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_winners
    WHERE sweep_winner_uid = sweepWinnerUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_winner_unfulfilled_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winner_unfulfilled_create`(IN sweepWinnerUid VARCHAR(50), IN reason VARCHAR(100))
BEGIN
	INSERT INTO sweep_winners_unfulfilled
    (
		sweep_winner_unfulfilled_uid,
        sweep_id,
        sweep_entry_uid,
        assigned_id,
        user_identifier,
        user_identifier_type,
        reason_status,
        created_date
    )
    SELECT
		SW.sweep_winner_uid,
        SW.sweep_id,
        SW.sweep_entry_uid,
        SW.assigned_id,
        SW.user_identifier,
        SW.user_identifier_type,
        reason,
        CURRENT_TIMESTAMP()
    FROM sweep_winners as SW
    WHERE SW.sweep_winner_uid = sweepWinnerUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `tweet_entries_get_by_campaign_and_user` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `tweet_entries_get_by_campaign_and_user`(IN accountUid VARCHAR(50), IN campaignUid VARCHAR(50))
BEGIN
	SELECT
		COUNT(SE.sweep_entry_id) as count,
		S.sweep_name,
        S.sweep_uid,
		FROM_UNIXTIME(FLOOR(UNIX_TIMESTAMP(SE.created_date)/900)*900) AS the_time
	FROM accounts as A
	INNER JOIN campaigns as C ON A.account_id = C.account_id
	INNER JOIN sweep as S on S.campaign_id = C.campaign_id
	INNER JOIN sweep_entry as SE on SE.sweep_id = S.sweep_id
	WHERE A.account_uid = accountUid
    AND C.campaign_uid = campaignUid
    AND SE.created_date <= CURRENT_TIMESTAMP()
	/*AND ph.created_date BETWEEN previousTime AND CURRENT_TIMESTAMP()*/
	GROUP BY S.sweep_name, S.sweep_uid, the_time;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `watch_types_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `watch_types_get`()
BEGIN
	SELECT *
    FROM watch_types
    WHERE active_ind = 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `watch_type_campaign_types_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `watch_type_campaign_types_get`()
BEGIN
	SELECT
		ct.id,
        wt.watch_type_code,
        wt.watch_type_name,
        ct.campaign_type_uid
    FROM watch_type_campaign_types as ct
    INNER JOIN watch_types as wt ON ct.watch_type_id = wt.watch_type_id;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `watch_type_providers_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `watch_type_providers_get`()
BEGIN
	SELECT
		watch_type_provider_id,
        watch_type_provider_uid,
        provider_code,
        provider_name,
        active_ind,
        image_url
    FROM watch_type_providers;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-02-09 17:44:06
