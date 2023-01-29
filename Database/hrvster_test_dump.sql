-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: localhost    Database: hrvster_test
-- ------------------------------------------------------
-- Server version	8.0.30

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
-- Table structure for table `account_profiles`
--

DROP TABLE IF EXISTS `account_profiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_profiles` (
  `profile_id` int unsigned NOT NULL AUTO_INCREMENT,
  `profile_uid` varchar(50) NOT NULL,
  `external_profile_id` varchar(100) NOT NULL DEFAULT '1',
  `account_id` int unsigned NOT NULL,
  `profile_type_id` int unsigned NOT NULL,
  `display_name` varchar(100) NOT NULL,
  `screen_name` varchar(1000) NOT NULL,
  `profile_description` varchar(1000) NOT NULL,
  `access_token` varchar(1000) NOT NULL,
  `access_token_secret` varchar(1000) NOT NULL,
  `consumer_key` varchar(1000) DEFAULT NULL,
  `consumer_secret` varchar(1000) DEFAULT NULL,
  `location` varchar(100) DEFAULT NULL,
  `profile_image_url` varchar(1000) DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`profile_id`),
  UNIQUE KEY `profile_id_UNIQUE` (`profile_id`) /*!80000 INVISIBLE */,
  UNIQUE KEY `profile_uid_UNIQUE` (`profile_uid`),
  UNIQUE KEY `account_profile_type_unique` (`account_id`,`profile_type_id`),
  KEY `fk_account_profile_idx` (`account_id`),
  KEY `fk_profile_type_profile_idx` (`profile_type_id`),
  CONSTRAINT `fk_account_profile` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_profile_type_profile` FOREIGN KEY (`profile_type_id`) REFERENCES `profile_types` (`profile_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=239 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_profiles`
--

LOCK TABLES `account_profiles` WRITE;
/*!40000 ALTER TABLE `account_profiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_profiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_subscriptions`
--

DROP TABLE IF EXISTS `account_subscriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_subscriptions` (
  `subscription_id` int unsigned NOT NULL AUTO_INCREMENT,
  `subscription_uid` varchar(50) NOT NULL,
  `subscription_status` varchar(100) NOT NULL DEFAULT 'PendingPayment',
  `account_id` int unsigned NOT NULL,
  `plan_id` int unsigned NOT NULL,
  `start_date` datetime NOT NULL,
  `start_date_utc` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `end_date_utc` datetime NOT NULL,
  `is_discounted` tinyint NOT NULL DEFAULT '0',
  `discounted_by` decimal(10,0) DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`subscription_id`,`subscription_uid`),
  UNIQUE KEY `subscription_id_UNIQUE` (`subscription_id`),
  UNIQUE KEY `subscription_uid_UNIQUE` (`subscription_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=553 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_subscriptions`
--

LOCK TABLES `account_subscriptions` WRITE;
/*!40000 ALTER TABLE `account_subscriptions` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_subscriptions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_subscriptions_history`
--

DROP TABLE IF EXISTS `account_subscriptions_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_subscriptions_history` (
  `subscription_history_id` int unsigned NOT NULL AUTO_INCREMENT,
  `subscription_history_uid` varchar(50) NOT NULL,
  `account_id` int unsigned NOT NULL,
  `subscription_status` varchar(100) NOT NULL,
  `plan_id` int unsigned NOT NULL,
  `start_date` datetime NOT NULL,
  `start_date_utc` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `end_date_utc` datetime NOT NULL,
  `is_discounted` tinyint NOT NULL DEFAULT '0',
  `discounted_by` decimal(10,0) DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`subscription_history_id`,`subscription_history_uid`),
  UNIQUE KEY `subscription_history_id_UNIQUE` (`subscription_history_id`),
  UNIQUE KEY `subscription_history_uid_UNIQUE` (`subscription_history_uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_subscriptions_history`
--

LOCK TABLES `account_subscriptions_history` WRITE;
/*!40000 ALTER TABLE `account_subscriptions_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_subscriptions_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account_update_history`
--

DROP TABLE IF EXISTS `account_update_history`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_update_history` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `uid` varchar(50) NOT NULL,
  `account_id` int unsigned NOT NULL,
  `old_value` varchar(500) NOT NULL,
  `new_value` varchar(500) NOT NULL,
  `created_date` datetime NOT NULL,
  PRIMARY KEY (`id`,`uid`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_update_history`
--

LOCK TABLES `account_update_history` WRITE;
/*!40000 ALTER TABLE `account_update_history` DISABLE KEYS */;
/*!40000 ALTER TABLE `account_update_history` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `accounts` (
  `account_id` int unsigned NOT NULL AUTO_INCREMENT,
  `account_uid` varchar(50) NOT NULL,
  `email_address` varchar(600) NOT NULL,
  `phone_number` varchar(20) NOT NULL,
  `display_name` varchar(50) DEFAULT NULL,
  `first_name` varchar(100) DEFAULT NULL,
  `last_name` varchar(100) DEFAULT NULL,
  `verification_link` varchar(1000) DEFAULT NULL,
  `email_address_verified` bit(1) DEFAULT NULL,
  `phone_number_verified` bit(1) DEFAULT NULL,
  `pin` varchar(100) DEFAULT NULL,
  `twitter_user_name` varchar(120) DEFAULT NULL,
  `terms_accepted` bit(1) DEFAULT NULL,
  `locale` varchar(45) DEFAULT 'Nigeria',
  `last_login_date` timestamp NULL DEFAULT NULL,
  `last_login_date_utc` datetime DEFAULT CURRENT_TIMESTAMP,
  `created_date` timestamp NOT NULL,
  `updated_date` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`account_id`,`account_uid`,`email_address`,`phone_number`),
  UNIQUE KEY `user_id_UNIQUE` (`account_id`),
  UNIQUE KEY `user_uid_UNIQUE` (`account_uid`),
  UNIQUE KEY `email_address_UNIQUE` (`email_address`),
  UNIQUE KEY `phone_number_UNIQUE` (`phone_number`),
  UNIQUE KEY `display_name_UNIQUE` (`display_name`)
) ENGINE=InnoDB AUTO_INCREMENT=3182 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `campaign_types`
--

DROP TABLE IF EXISTS `campaign_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `campaign_types` (
  `campaign_type_id` int unsigned NOT NULL AUTO_INCREMENT,
  `campaign_type_uid` varchar(50) NOT NULL,
  `campaign_type_code` varchar(60) NOT NULL,
  `campaign_type_name` varchar(50) NOT NULL,
  `campaign_type_description` varchar(200) DEFAULT NULL,
  `active_ind` bit(1) NOT NULL DEFAULT b'0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`campaign_type_id`),
  UNIQUE KEY `campaign_type_id_UNIQUE` (`campaign_type_id`),
  UNIQUE KEY `campaign_type_uid_UNIQUE` (`campaign_type_uid`),
  UNIQUE KEY `campaign_name_UNIQUE` (`campaign_type_code`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campaign_types`
--

LOCK TABLES `campaign_types` WRITE;
/*!40000 ALTER TABLE `campaign_types` DISABLE KEYS */;
INSERT INTO `campaign_types` VALUES (1,'ad21aa80-6832-4713-b668-784d76fbdf11','AutoReplyTweet','Auto Reply Tweet','Campaign that only supports automatically replying to a specific Tweet id.',_binary '\0','2021-12-12 00:00:00',NULL),(2,'27e64712-3663-45c6-a32f-3e7a598c3b09','AutoReplyDM','Auto Reply DM','Campaign that only supports automatically replying to a specific DM Id.',_binary '\0','2021-12-12 00:00:00',NULL),(3,'4d5b1fdc-763a-48dd-89c6-09fe075bf1ee','AutoReplyHashtag','Auto Reply Hashtag','Campaign that only supports automatically replying to a specific Hashtag.',_binary '\0','2021-12-12 00:00:00',NULL),(4,'a1fcf285-96c9-4810-a9c5-eafed80f9d70','AutoReplyLikedTweet','Auto Reply Liked Tweet','Campaign that only supports automatically tweeting someone once they like a tweet.',_binary '\0','2021-12-12 00:00:00',NULL),(5,'5465b768-33a5-498a-8c8f-f8ff424e7098','AutoEnterSweepsFromTweet','Auto Enter Sweeps From Tweet','Campaign that supports automatically entering a sweepstakes when someone replies, likes or interacts with a tweet.',_binary '\0','2021-12-12 00:00:00',NULL),(6,'a445f2c8-dc06-4367-8597-c550fc02d7e4','AutoEnterSweepsFromHashtag','Auto Enter Sweeps From Hashtag','Campaign that supports automatically entering a sweepstakes when tweets a hashtag.',_binary '\0','2021-12-12 00:00:00',NULL),(7,'3b73d6c4-ebe3-4f68-9c73-845589d13d9f','AutoAll','Auto All','A la carte',_binary '\0','2021-12-12 00:00:00',NULL),(15,'3e5431d6-ba17-41f4-8f21-fd2d70245845','AutoEnterSweepsFromTweetReplyHashtag','Auto Enter Sweeps From Tweet Reply Hashtag','Campaign that supports automatically entering a sweepstakes when a user replies a tweet with a specific hashtag.',_binary '','2021-12-12 00:00:00',NULL);
/*!40000 ALTER TABLE `campaign_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `campaigns`
--

DROP TABLE IF EXISTS `campaigns`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `campaigns` (
  `campaign_id` int unsigned NOT NULL AUTO_INCREMENT,
  `account_id` int unsigned NOT NULL,
  `campaign_type_id` int unsigned NOT NULL,
  `campaign_uid` varchar(50) NOT NULL,
  `campaign_name` varchar(50) NOT NULL,
  `campaign_description` varchar(500) DEFAULT NULL,
  `active_ind` tinyint NOT NULL DEFAULT '1',
  `start_date` datetime NOT NULL,
  `start_date_utc` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `end_date` datetime NOT NULL,
  `end_date_utc` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`campaign_id`),
  UNIQUE KEY `campaign_id_UNIQUE` (`campaign_id`),
  UNIQUE KEY `campaign_uid_UNIQUE` (`campaign_uid`),
  UNIQUE KEY `account_to_campaign_unique` (`account_id`,`campaign_name`,`campaign_type_id`),
  KEY `campaign_type_campaign_idx` (`campaign_type_id`),
  CONSTRAINT `account_campaigns_fk` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`),
  CONSTRAINT `campaign_type_campaign` FOREIGN KEY (`campaign_type_id`) REFERENCES `campaign_types` (`campaign_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2568 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campaigns`
--

LOCK TABLES `campaigns` WRITE;
/*!40000 ALTER TABLE `campaigns` DISABLE KEYS */;
/*!40000 ALTER TABLE `campaigns` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cron_services`
--

DROP TABLE IF EXISTS `cron_services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cron_services` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  `fqn` varchar(500) NOT NULL,
  `enabled` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cron_services`
--

LOCK TABLES `cron_services` WRITE;
/*!40000 ALTER TABLE `cron_services` DISABLE KEYS */;
INSERT INTO `cron_services` VALUES (4,'ScheduledTweetService','hrvster.Services.ScheduledTweetService',1),(5,'TwitterHarvestingService','hrvster.Services.TwitterHarvestingService',1),(6,'SweepstakesDrawService','hrvster.Services.SweepstakesDrawService',1),(7,'EmailNotificationService','hrvster.Services.EmailNotificationService',1),(8,'AccountVerificationService','hrvster.Services.AccountVerificationService',1),(9,'FlutterwaveEventService','hrvster.Services.FlutterwaveEventService',1),(10,'HarvesterRestarterService','hrvster.Services.HarvesterRestarterService',1),(11,'SweepsGoingLiveService','hrvster.Services.SweepsGoingLiveService',1);
/*!40000 ALTER TABLE `cron_services` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `emails`
--

DROP TABLE IF EXISTS `emails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `emails` (
  `email_id` int unsigned NOT NULL AUTO_INCREMENT,
  `email_uid` varchar(50) NOT NULL,
  `email_address_to` varchar(400) NOT NULL,
  `parameters` varchar(8000) NOT NULL,
  `template_name` varchar(45) NOT NULL,
  `status` varchar(45) NOT NULL,
  `sent` bit(1) NOT NULL DEFAULT b'0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`email_id`),
  UNIQUE KEY `email_id_UNIQUE` (`email_id`),
  UNIQUE KEY `email_uid_UNIQUE` (`email_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=1219 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emails`
--

LOCK TABLES `emails` WRITE;
/*!40000 ALTER TABLE `emails` DISABLE KEYS */;
/*!40000 ALTER TABLE `emails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `flutterwave_card_transactions`
--

DROP TABLE IF EXISTS `flutterwave_card_transactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flutterwave_card_transactions` (
  `flutterwave_card_transaction_id` int unsigned NOT NULL AUTO_INCREMENT,
  `flutterwave_card_transaction_uid` varchar(50) NOT NULL,
  `id` bigint NOT NULL,
  `transaction_reference` varchar(100) NOT NULL,
  `flow_reference` varchar(100) NOT NULL,
  `order_reference` varchar(100) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`flutterwave_card_transaction_id`,`flutterwave_card_transaction_uid`,`order_reference`,`flow_reference`,`transaction_reference`,`id`),
  UNIQUE KEY `flutterwave_card_transaction_id_UNIQUE` (`flutterwave_card_transaction_id`),
  UNIQUE KEY `flutterwave_card_transaction_uid_UNIQUE` (`flutterwave_card_transaction_uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flutterwave_card_transactions`
--

LOCK TABLES `flutterwave_card_transactions` WRITE;
/*!40000 ALTER TABLE `flutterwave_card_transactions` DISABLE KEYS */;
/*!40000 ALTER TABLE `flutterwave_card_transactions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `flutterwave_events`
--

DROP TABLE IF EXISTS `flutterwave_events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flutterwave_events` (
  `flutterwave_event_id` int unsigned NOT NULL AUTO_INCREMENT,
  `flutterwave_event_uid` varchar(50) NOT NULL,
  `payload_received` varchar(8000) NOT NULL,
  `event_status` varchar(100) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`flutterwave_event_id`,`flutterwave_event_uid`),
  UNIQUE KEY `flutterwave_event_id_UNIQUE` (`flutterwave_event_id`),
  UNIQUE KEY `flutterwave_event_uid_UNIQUE` (`flutterwave_event_uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flutterwave_events`
--

LOCK TABLES `flutterwave_events` WRITE;
/*!40000 ALTER TABLE `flutterwave_events` DISABLE KEYS */;
/*!40000 ALTER TABLE `flutterwave_events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `flutterwave_plans`
--

DROP TABLE IF EXISTS `flutterwave_plans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flutterwave_plans` (
  `flutterwave_plan_id` int unsigned NOT NULL AUTO_INCREMENT,
  `flutterwave_plan_uid` varchar(50) NOT NULL,
  `plan_id` int unsigned NOT NULL,
  `external_monthly_payment_link` varchar(5000) DEFAULT NULL,
  `external_yearly_payment_link` varchar(5000) DEFAULT NULL,
  PRIMARY KEY (`flutterwave_plan_id`,`flutterwave_plan_uid`),
  UNIQUE KEY `id_UNIQUE` (`flutterwave_plan_id`),
  UNIQUE KEY `flutterwave_plan_uid_UNIQUE` (`flutterwave_plan_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flutterwave_plans`
--

LOCK TABLES `flutterwave_plans` WRITE;
/*!40000 ALTER TABLE `flutterwave_plans` DISABLE KEYS */;
INSERT INTO `flutterwave_plans` VALUES (1,'23a97b0e-8861-4e93-a729-90a89d479801',1,'https://sandbox-flw-web-v3.herokuapp.com/pay/basicweeklycharge',NULL),(2,'23a97b0e-8861-4e93-a729-90a89d479802',2,'https://sandbox-flw-web-v3.herokuapp.com/pay/startermonthlypayment',NULL),(3,'23a97b0e-8861-4e93-a729-90a89d479803',3,'https://sandbox-flw-web-v3.herokuapp.com/pay/advancedmonthlypayment',NULL);
/*!40000 ALTER TABLE `flutterwave_plans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `harvester_service_command`
--

DROP TABLE IF EXISTS `harvester_service_command`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `harvester_service_command` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `uid` varchar(50) NOT NULL,
  `command` varchar(45) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`,`uid`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `uid_UNIQUE` (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `harvester_service_command`
--

LOCK TABLES `harvester_service_command` WRITE;
/*!40000 ALTER TABLE `harvester_service_command` DISABLE KEYS */;
INSERT INTO `harvester_service_command` VALUES (1,'24507f4b-44a9-4338-951c-8faa449e1fde','start','2023-01-27 09:36:02');
/*!40000 ALTER TABLE `harvester_service_command` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `match_boards`
--

DROP TABLE IF EXISTS `match_boards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `match_boards` (
  `board_id` int unsigned NOT NULL AUTO_INCREMENT,
  `board_uid` varchar(50) NOT NULL,
  `match_game_id` int unsigned NOT NULL,
  `is_winnable` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`board_id`,`board_uid`),
  UNIQUE KEY `board_id_UNIQUE` (`board_id`),
  UNIQUE KEY `board_uid_UNIQUE` (`board_uid`),
  KEY `match_game_board_idx` (`match_game_id`),
  CONSTRAINT `match_game_board` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `match_boards`
--

LOCK TABLES `match_boards` WRITE;
/*!40000 ALTER TABLE `match_boards` DISABLE KEYS */;
/*!40000 ALTER TABLE `match_boards` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `match_games`
--

DROP TABLE IF EXISTS `match_games`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `match_games` (
  `match_game_id` int unsigned NOT NULL AUTO_INCREMENT,
  `campaign_id` int unsigned NOT NULL,
  `match_game_uid` varchar(50) NOT NULL,
  `match_game_name` varchar(50) NOT NULL,
  `match_game_description` varchar(100) DEFAULT NULL,
  `number_of_boards` int DEFAULT '10',
  `tile_count_per_board` int DEFAULT '9',
  `number_of_winnable_boards` int DEFAULT '1',
  `win_probability_per_board` int DEFAULT '9',
  `is_generated` tinyint NOT NULL DEFAULT '0',
  `start_date` datetime DEFAULT NULL,
  `start_date_utc` datetime DEFAULT NULL,
  `end_date` datetime DEFAULT NULL,
  `end_date_utc` datetime DEFAULT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`match_game_id`,`match_game_uid`),
  UNIQUE KEY `match_game_id_UNIQUE` (`match_game_id`),
  UNIQUE KEY `match_game_uid_UNIQUE` (`match_game_uid`),
  UNIQUE KEY `match_game_unique_to_campaign` (`campaign_id`,`match_game_name`),
  CONSTRAINT `match_game_campaign` FOREIGN KEY (`campaign_id`) REFERENCES `campaigns` (`campaign_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `match_games`
--

LOCK TABLES `match_games` WRITE;
/*!40000 ALTER TABLE `match_games` DISABLE KEYS */;
/*!40000 ALTER TABLE `match_games` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `match_images`
--

DROP TABLE IF EXISTS `match_images`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `match_images` (
  `image_id` int unsigned NOT NULL AUTO_INCREMENT,
  `image_uid` varchar(50) NOT NULL,
  `match_game_id` int unsigned NOT NULL,
  `location_url` varchar(500) NOT NULL,
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`image_id`,`image_uid`),
  UNIQUE KEY `image_id_UNIQUE` (`image_id`),
  UNIQUE KEY `image_uid_UNIQUE` (`image_uid`),
  KEY `match_game_image_idx` (`match_game_id`),
  CONSTRAINT `match_game_image` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `match_images`
--

LOCK TABLES `match_images` WRITE;
/*!40000 ALTER TABLE `match_images` DISABLE KEYS */;
/*!40000 ALTER TABLE `match_images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `match_prizes`
--

DROP TABLE IF EXISTS `match_prizes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `match_prizes` (
  `prize_id` int unsigned NOT NULL AUTO_INCREMENT,
  `prize_uid` varchar(50) NOT NULL,
  `match_game_id` int unsigned NOT NULL,
  `prize_name` varchar(50) NOT NULL,
  `prize_description` varchar(100) DEFAULT NULL,
  `prize_value` decimal(18,2) NOT NULL,
  `image_id` int NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`prize_id`,`prize_uid`),
  UNIQUE KEY `prize_id_UNIQUE` (`prize_id`),
  UNIQUE KEY `prize_uid_UNIQUE` (`prize_uid`),
  KEY `match_game_prize_idx` (`match_game_id`),
  CONSTRAINT `match_game_prize` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `match_prizes`
--

LOCK TABLES `match_prizes` WRITE;
/*!40000 ALTER TABLE `match_prizes` DISABLE KEYS */;
/*!40000 ALTER TABLE `match_prizes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `match_tiles`
--

DROP TABLE IF EXISTS `match_tiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `match_tiles` (
  `tile_id` int unsigned NOT NULL AUTO_INCREMENT,
  `tile_uid` varchar(50) NOT NULL,
  `board_id` int unsigned NOT NULL,
  `image_id` int unsigned NOT NULL,
  `prize_id` int unsigned NOT NULL,
  `is_winnable` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`tile_id`,`tile_uid`),
  UNIQUE KEY `match_tile_id_UNIQUE` (`tile_id`),
  UNIQUE KEY `match_tile_uid_UNIQUE` (`tile_uid`),
  KEY `match_tile_board_idx` (`board_id`),
  KEY `match_tile_prize_idx` (`prize_id`),
  KEY `match_tile_image_idx` (`image_id`),
  CONSTRAINT `match_tile_board` FOREIGN KEY (`board_id`) REFERENCES `match_boards` (`board_id`),
  CONSTRAINT `match_tile_image` FOREIGN KEY (`image_id`) REFERENCES `match_images` (`image_id`),
  CONSTRAINT `match_tile_prize` FOREIGN KEY (`prize_id`) REFERENCES `match_prizes` (`prize_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `match_tiles`
--

LOCK TABLES `match_tiles` WRITE;
/*!40000 ALTER TABLE `match_tiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `match_tiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payment_services`
--

DROP TABLE IF EXISTS `payment_services`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payment_services` (
  `payment_service_id` int unsigned NOT NULL AUTO_INCREMENT,
  `payment_service_uid` varchar(50) NOT NULL,
  `payment_service_name` varchar(50) NOT NULL,
  `payment_service_namespace` varchar(100) NOT NULL,
  `enabled_ind` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`payment_service_id`,`payment_service_uid`,`payment_service_name`),
  UNIQUE KEY `payment_service_id_UNIQUE` (`payment_service_id`),
  UNIQUE KEY `payment_service_uid_UNIQUE` (`payment_service_uid`),
  UNIQUE KEY `payment_service_name_UNIQUE` (`payment_service_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_services`
--

LOCK TABLES `payment_services` WRITE;
/*!40000 ALTER TABLE `payment_services` DISABLE KEYS */;
/*!40000 ALTER TABLE `payment_services` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plans`
--

DROP TABLE IF EXISTS `plans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plans` (
  `plan_id` int unsigned NOT NULL AUTO_INCREMENT,
  `plan_uid` varchar(50) NOT NULL,
  `plan_name` varchar(100) NOT NULL,
  `plan_description` varchar(500) NOT NULL,
  `promotions_per_month` int NOT NULL,
  `sweeps_per_month` int NOT NULL,
  `sweep_hashtags_per_month` int NOT NULL DEFAULT '0',
  `sweepstakes_maximum_entries` int DEFAULT NULL,
  `price_per_month` decimal(10,0) NOT NULL,
  `price_per_year` decimal(10,0) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`plan_id`,`plan_uid`,`plan_name`),
  UNIQUE KEY `plan_id_UNIQUE` (`plan_id`),
  UNIQUE KEY `plan_uid_UNIQUE` (`plan_uid`),
  UNIQUE KEY `plan_name_UNIQUE` (`plan_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plans`
--

LOCK TABLES `plans` WRITE;
/*!40000 ALTER TABLE `plans` DISABLE KEYS */;
INSERT INTO `plans` VALUES (1,'e39503df-db14-4988-a9ad-adc6e938988e','Basic','A basic promotion plan',1,1,2,5000,50000,600000,'2022-12-11 00:00:00','2023-01-24 22:55:21'),(2,'f88e1f57-c2b9-47d0-95c4-3f8e93e619ed','Starter','For more seasoned members',0,1,3,12000,100000,1200000,'2022-12-11 00:00:00','2023-01-29 09:46:00'),(3,'8597b37d-99e4-403a-9b1c-7b75a5e78c50','Advanced','For more flexibility',0,0,5,30000,250000,3000000,'2022-12-11 00:00:00',NULL);
/*!40000 ALTER TABLE `plans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `profile_types`
--

DROP TABLE IF EXISTS `profile_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `profile_types` (
  `profile_type_id` int unsigned NOT NULL AUTO_INCREMENT,
  `profile_type_uid` varchar(50) NOT NULL,
  `name` varchar(50) NOT NULL,
  PRIMARY KEY (`profile_type_id`),
  UNIQUE KEY `profile_type_id_UNIQUE` (`profile_type_id`),
  UNIQUE KEY `profile_type_uid_UNIQUE` (`profile_type_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `profile_types`
--

LOCK TABLES `profile_types` WRITE;
/*!40000 ALTER TABLE `profile_types` DISABLE KEYS */;
INSERT INTO `profile_types` VALUES (1,'1b3b9e77-6a8c-42be-8266-e0d116ac203e','Twitter');
/*!40000 ALTER TABLE `profile_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `report_sweep_entries`
--

DROP TABLE IF EXISTS `report_sweep_entries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `report_sweep_entries` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `sweep_id` varchar(50) NOT NULL,
  `campaign_id` varchar(50) NOT NULL,
  `screen_name` varchar(100) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `external_id` varchar(100) NOT NULL,
  `external_user_id` varchar(100) NOT NULL,
  `entries` int NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `record_unique` (`campaign_id`,`sweep_id`,`external_user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=110 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `report_sweep_entries`
--

LOCK TABLES `report_sweep_entries` WRITE;
/*!40000 ALTER TABLE `report_sweep_entries` DISABLE KEYS */;
/*!40000 ALTER TABLE `report_sweep_entries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reporting_sweep_entries`
--

DROP TABLE IF EXISTS `reporting_sweep_entries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reporting_sweep_entries` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `sweep_id` int unsigned NOT NULL,
  `screen_name` varchar(100) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `external_id` varchar(100) NOT NULL,
  `external_user_id` varchar(100) NOT NULL,
  `entries` int NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `record_unique` (`sweep_id`,`external_user_id`),
  CONSTRAINT `sweep_reporting_entries_fk` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`)
) ENGINE=InnoDB AUTO_INCREMENT=110 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reporting_sweep_entries`
--

LOCK TABLES `reporting_sweep_entries` WRITE;
/*!40000 ALTER TABLE `reporting_sweep_entries` DISABLE KEYS */;
/*!40000 ALTER TABLE `reporting_sweep_entries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scheduled_sweep_rule_tweet_images`
--

DROP TABLE IF EXISTS `scheduled_sweep_rule_tweet_images`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheduled_sweep_rule_tweet_images` (
  `scheduled_image_id` int unsigned NOT NULL AUTO_INCREMENT,
  `scheduled_id` int unsigned NOT NULL,
  `image_location` varchar(1000) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`scheduled_image_id`),
  UNIQUE KEY `scheduled_images_id_UNIQUE` (`scheduled_image_id`),
  KEY `fk_scheduled_sweep_rule_idx` (`scheduled_id`),
  CONSTRAINT `fk_scheduled_sweep_rule` FOREIGN KEY (`scheduled_id`) REFERENCES `scheduled_sweep_rule_tweets` (`scheduled_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheduled_sweep_rule_tweet_images`
--

LOCK TABLES `scheduled_sweep_rule_tweet_images` WRITE;
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweet_images` DISABLE KEYS */;
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweet_images` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `scheduled_sweep_rule_tweets`
--

DROP TABLE IF EXISTS `scheduled_sweep_rule_tweets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `scheduled_sweep_rule_tweets` (
  `scheduled_id` int unsigned NOT NULL AUTO_INCREMENT,
  `scheduled_uid` varchar(50) NOT NULL,
  `profile_id` int NOT NULL,
  `rule_id` int unsigned NOT NULL,
  `tweet_text` varchar(500) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
  `scheduled_time` datetime NOT NULL,
  `scheduled_time_utc` datetime NOT NULL,
  `status` varchar(100) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`scheduled_id`),
  UNIQUE KEY `id_UNIQUE` (`scheduled_id`),
  UNIQUE KEY `scheduled_uid_UNIQUE` (`scheduled_uid`),
  KEY `scheduled_sweep_rule_tweets_ibfk_1` (`rule_id`),
  CONSTRAINT `scheduled_sweep_rule_tweets_ibfk_1` FOREIGN KEY (`rule_id`) REFERENCES `sweep_rules` (`rule_id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheduled_sweep_rule_tweets`
--

LOCK TABLES `scheduled_sweep_rule_tweets` WRITE;
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweets` DISABLE KEYS */;
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subscribers`
--

DROP TABLE IF EXISTS `subscribers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subscribers` (
  `subscriber_id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `email_address` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`subscriber_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subscribers`
--

LOCK TABLES `subscribers` WRITE;
/*!40000 ALTER TABLE `subscribers` DISABLE KEYS */;
/*!40000 ALTER TABLE `subscribers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep`
--

DROP TABLE IF EXISTS `sweep`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep` (
  `sweep_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_uid` varchar(50) NOT NULL,
  `account_id` int unsigned NOT NULL,
  `campaign_id` int unsigned NOT NULL,
  `sweep_name` varchar(100) NOT NULL,
  `rules_url` text,
  `start_date` datetime NOT NULL,
  `start_date_utc` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `end_date` datetime NOT NULL,
  `end_date_utc` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `draw_date` datetime NOT NULL,
  `draw_date_utc` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `draw_attempted` tinyint NOT NULL DEFAULT '0',
  `not_enough_unique_entrants` tinyint NOT NULL DEFAULT '0',
  `notification_sent_going_live` tinyint NOT NULL DEFAULT '0',
  `drawn_ind` tinyint NOT NULL DEFAULT '0',
  `date_drawn` datetime DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_id`),
  UNIQUE KEY `sweep_id_UNIQUE` (`sweep_id`),
  UNIQUE KEY `sweep_uid_UNIQUE` (`sweep_uid`),
  UNIQUE KEY `sweep_name_campaign_unique` (`sweep_name`,`campaign_id`,`account_id`),
  KEY `fk_account_sweeps_idx` (`account_id`),
  KEY `fk_campaign_sweeps_idx` (`campaign_id`),
  CONSTRAINT `fk_account_sweeps` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_campaign_sweeps` FOREIGN KEY (`campaign_id`) REFERENCES `campaigns` (`campaign_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2250 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep`
--

LOCK TABLES `sweep` WRITE;
/*!40000 ALTER TABLE `sweep` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_draw_rules`
--

DROP TABLE IF EXISTS `sweep_draw_rules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_draw_rules` (
  `sweep_draw_rule_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_draw_rule_uid` varchar(50) NOT NULL,
  `sweep_id` int NOT NULL,
  `winner_count` int NOT NULL,
  `winner_backup_count` int NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_draw_rule_id`),
  UNIQUE KEY `sweep_draw_rule_id_UNIQUE` (`sweep_draw_rule_id`),
  UNIQUE KEY `sweep_draw_rule_uid_UNIQUE` (`sweep_draw_rule_uid`),
  UNIQUE KEY `sweep_unique` (`sweep_id`)
) ENGINE=InnoDB AUTO_INCREMENT=194 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_draw_rules`
--

LOCK TABLES `sweep_draw_rules` WRITE;
/*!40000 ALTER TABLE `sweep_draw_rules` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_draw_rules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_entry`
--

DROP TABLE IF EXISTS `sweep_entry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_entry` (
  `sweep_entry_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `sweep_id` int unsigned NOT NULL,
  `rule_id` int unsigned DEFAULT NULL,
  `user_identifier` varchar(500) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `accepted_terms` bit(1) NOT NULL DEFAULT b'0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_entry_id`),
  UNIQUE KEY `sweep_entry_id_UNIQUE` (`sweep_entry_id`),
  UNIQUE KEY `sweep_entry_uid_UNIQUE` (`sweep_entry_uid`),
  KEY `sweep_sweep_entry_idx` (`sweep_id`),
  CONSTRAINT `sweep_sweep_entry` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4847 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entry`
--

LOCK TABLES `sweep_entry` WRITE;
/*!40000 ALTER TABLE `sweep_entry` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_entry` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_entry_rules`
--

DROP TABLE IF EXISTS `sweep_entry_rules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_entry_rules` (
  `entry_rule_id` int unsigned NOT NULL AUTO_INCREMENT,
  `entry_rule_uid` varchar(50) NOT NULL,
  `sweep_id` int NOT NULL,
  `entry_per_user` int NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`entry_rule_id`),
  UNIQUE KEY `entry_rule_id_UNIQUE` (`entry_rule_id`),
  UNIQUE KEY `entry_rule_uid_UNIQUE` (`entry_rule_uid`),
  UNIQUE KEY `sweep_unique` (`sweep_id`)
) ENGINE=InnoDB AUTO_INCREMENT=187 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entry_rules`
--

LOCK TABLES `sweep_entry_rules` WRITE;
/*!40000 ALTER TABLE `sweep_entry_rules` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_entry_rules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_notifications`
--

DROP TABLE IF EXISTS `sweep_notifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_notifications` (
  `sweep_notification_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_id` int unsigned NOT NULL,
  `not_enough_unique_entrants` tinyint NOT NULL DEFAULT '0',
  `sweeps_going_live` tinyint NOT NULL DEFAULT '0',
  `sweeps_live` tinyint NOT NULL DEFAULT '0',
  `draw_attempted` tinyint DEFAULT '0',
  `draw_happened` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime DEFAULT NULL,
  `updated_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`sweep_notification_id`,`sweep_id`),
  UNIQUE KEY `sweep_notification_id_UNIQUE` (`sweep_notification_id`),
  UNIQUE KEY `sweep_id_UNIQUE` (`sweep_id`),
  KEY `sweep_notification_sweeps_idx` (`sweep_id`),
  CONSTRAINT `sweep_notification_sweeps` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1161 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_notifications`
--

LOCK TABLES `sweep_notifications` WRITE;
/*!40000 ALTER TABLE `sweep_notifications` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_notifications` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_prizes`
--

DROP TABLE IF EXISTS `sweep_prizes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_prizes` (
  `prize_id` int unsigned NOT NULL AUTO_INCREMENT,
  `prize_uid` varchar(50) NOT NULL,
  `prize_position` int NOT NULL,
  `sweep_id` int NOT NULL,
  `enabled_ind` bit(1) NOT NULL DEFAULT b'0',
  `prize_name` varchar(100) NOT NULL,
  `prize_description` varchar(5000) DEFAULT NULL,
  `prize_value` decimal(18,2) NOT NULL,
  `locale` varchar(50) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`prize_id`),
  UNIQUE KEY `prize_id_UNIQUE` (`prize_id`),
  UNIQUE KEY `prize_uid_UNIQUE` (`prize_uid`),
  UNIQUE KEY `prize_name_sweeps_unique` (`prize_name`,`sweep_id`)
) ENGINE=InnoDB AUTO_INCREMENT=299 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_prizes`
--

LOCK TABLES `sweep_prizes` WRITE;
/*!40000 ALTER TABLE `sweep_prizes` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_prizes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_rules`
--

DROP TABLE IF EXISTS `sweep_rules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_rules` (
  `rule_id` int unsigned NOT NULL AUTO_INCREMENT,
  `rule_uid` varchar(50) NOT NULL,
  `sweep_id` int unsigned DEFAULT NULL,
  `watch_type_id` int unsigned NOT NULL,
  `watch_value` varchar(500) NOT NULL,
  `must_be_following` tinyint NOT NULL DEFAULT '0',
  `notify_winners` tinyint DEFAULT '1',
  `scheduled_ind` bit(1) NOT NULL DEFAULT b'0',
  `active_ind` bit(1) NOT NULL DEFAULT b'0',
  `external_id` bigint DEFAULT NULL,
  `added_for_tracking` bit(1) DEFAULT b'0',
  `date_added_for_tracking` datetime DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`rule_id`),
  UNIQUE KEY `rule_id_UNIQUE` (`rule_id`) /*!80000 INVISIBLE */,
  UNIQUE KEY `rule_uid_UNIQUE` (`rule_uid`),
  UNIQUE KEY `sweep_watch_value_unique` (`sweep_id`,`watch_type_id`,`watch_value`),
  KEY `watch_types_rules_idx` (`watch_type_id`),
  KEY `sweep_id_rules_idx` (`sweep_id`),
  CONSTRAINT `sweep_id_rules` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `watch_types_rules` FOREIGN KEY (`watch_type_id`) REFERENCES `watch_types` (`watch_type_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=491 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_rules`
--

LOCK TABLES `sweep_rules` WRITE;
/*!40000 ALTER TABLE `sweep_rules` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_rules` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_winner_prizes`
--

DROP TABLE IF EXISTS `sweep_winner_prizes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_winner_prizes` (
  `sweep_winner_prize_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_prize_uid` varchar(50) NOT NULL,
  `sweep_id` int NOT NULL,
  `sweep_winner_id` int unsigned NOT NULL,
  `sweep_prize_uid` varchar(50) NOT NULL,
  `winner_accepted_ind` bit(1) NOT NULL,
  `fulfilled_ind` bit(1) NOT NULL,
  `backup_proceeding_initiated` bit(1) NOT NULL,
  `sweep_winner_backup_id` int unsigned DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_winner_prize_id`),
  UNIQUE KEY `sweep_winner_prize_id_UNIQUE` (`sweep_winner_prize_id`),
  UNIQUE KEY `sweep_winner_prize_uid_UNIQUE` (`sweep_winner_prize_uid`),
  KEY `sweep_winners_prizes_fk_idx` (`sweep_winner_id`),
  KEY `sweep_winners_backup_prizes_fk_idx` (`sweep_winner_backup_id`),
  CONSTRAINT `sweep_winners_backup_prizes_fk` FOREIGN KEY (`sweep_winner_backup_id`) REFERENCES `sweep_winners_backup` (`sweep_winner_backup_id`),
  CONSTRAINT `sweep_winners_prizes_fk` FOREIGN KEY (`sweep_winner_id`) REFERENCES `sweep_winners` (`sweep_winner_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winner_prizes`
--

LOCK TABLES `sweep_winner_prizes` WRITE;
/*!40000 ALTER TABLE `sweep_winner_prizes` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_winner_prizes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_winners`
--

DROP TABLE IF EXISTS `sweep_winners`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_winners` (
  `sweep_winner_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_uid` varchar(50) NOT NULL,
  `sweep_id` int NOT NULL,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `assigned_id` int NOT NULL,
  `user_identifier` varchar(100) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `backup_win` tinyint DEFAULT '0',
  `fulfilled_ind` tinyint NOT NULL DEFAULT '0',
  `fulfilled_prize_id` int DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_winner_id`),
  UNIQUE KEY `sweep_winner_id_UNIQUE` (`sweep_winner_id`),
  UNIQUE KEY `sweep_winner_uid_UNIQUE` (`sweep_winner_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=513 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners`
--

LOCK TABLES `sweep_winners` WRITE;
/*!40000 ALTER TABLE `sweep_winners` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_winners` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_winners_backup`
--

DROP TABLE IF EXISTS `sweep_winners_backup`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_winners_backup` (
  `sweep_winner_backup_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_id` int unsigned NOT NULL,
  `sweep_id` int NOT NULL,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `assigned_id` int NOT NULL,
  `user_identifier` varchar(100) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `selected_ind` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_winner_backup_id`),
  UNIQUE KEY `sweep_winner_backup_id_UNIQUE` (`sweep_winner_backup_id`),
  KEY `sweep_winners_backup_fk_idx` (`sweep_winner_id`),
  CONSTRAINT `sweep_winners_backup_fk` FOREIGN KEY (`sweep_winner_id`) REFERENCES `sweep_winners` (`sweep_winner_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=1043 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners_backup`
--

LOCK TABLES `sweep_winners_backup` WRITE;
/*!40000 ALTER TABLE `sweep_winners_backup` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_winners_backup` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sweep_winners_unfulfilled`
--

DROP TABLE IF EXISTS `sweep_winners_unfulfilled`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_winners_unfulfilled` (
  `sweep_winner_unfulfilled_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_unfulfilled_uid` varchar(50) NOT NULL,
  `sweep_id` int NOT NULL,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `assigned_id` int NOT NULL,
  `user_identifier` varchar(100) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `reason_status` varchar(50) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_winner_unfulfilled_id`),
  UNIQUE KEY `sweep_winner_unfulfilled_id_UNIQUE` (`sweep_winner_unfulfilled_id`)
) ENGINE=InnoDB AUTO_INCREMENT=320 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners_unfulfilled`
--

LOCK TABLES `sweep_winners_unfulfilled` WRITE;
/*!40000 ALTER TABLE `sweep_winners_unfulfilled` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_winners_unfulfilled` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_activities`
--

DROP TABLE IF EXISTS `user_activities`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_activities` (
  `activity_id` int unsigned NOT NULL AUTO_INCREMENT,
  `activity_uid` varchar(50) NOT NULL,
  `account_id` int unsigned NOT NULL,
  `linked_uid` varchar(50) NOT NULL,
  `activity_type` varchar(50) NOT NULL,
  `activity_table_base_name` varchar(100) NOT NULL,
  `created_date` datetime NOT NULL,
  `created_date_utc` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`activity_id`,`activity_uid`),
  UNIQUE KEY `activity_id_UNIQUE` (`activity_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2324 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_activities`
--

LOCK TABLES `user_activities` WRITE;
/*!40000 ALTER TABLE `user_activities` DISABLE KEYS */;
/*!40000 ALTER TABLE `user_activities` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `watch_type_campaign_types`
--

DROP TABLE IF EXISTS `watch_type_campaign_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `watch_type_campaign_types` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `watch_type_id` int unsigned NOT NULL,
  `campaign_type_id` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `watch_type_campaign_types`
--

LOCK TABLES `watch_type_campaign_types` WRITE;
/*!40000 ALTER TABLE `watch_type_campaign_types` DISABLE KEYS */;
INSERT INTO `watch_type_campaign_types` VALUES (7,1,1),(8,1,4),(9,1,5),(10,2,3),(11,2,6),(12,3,2);
/*!40000 ALTER TABLE `watch_type_campaign_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `watch_type_providers`
--

DROP TABLE IF EXISTS `watch_type_providers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `watch_type_providers` (
  `watch_type_provider_id` int unsigned NOT NULL AUTO_INCREMENT,
  `watch_type_provider_uid` varchar(50) NOT NULL,
  `provider_code` varchar(50) NOT NULL,
  `provider_name` varchar(50) NOT NULL,
  `active_ind` tinyint DEFAULT '0',
  `image_url` varchar(5000) DEFAULT NULL,
  PRIMARY KEY (`watch_type_provider_id`),
  UNIQUE KEY `watch_type_provider_uid_UNIQUE` (`watch_type_provider_uid`),
  UNIQUE KEY `watch_type_provider_id_UNIQUE` (`watch_type_provider_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `watch_type_providers`
--

LOCK TABLES `watch_type_providers` WRITE;
/*!40000 ALTER TABLE `watch_type_providers` DISABLE KEYS */;
INSERT INTO `watch_type_providers` VALUES (1,'0718b2e4-6a86-485e-a9b8-525097eabdc3','TWITTER','Twitter',1,NULL),(2,'eb9c0884-9ebc-4849-ada5-bbd2a0ca9593','FB','Facebook',0,NULL),(3,'1d45cc9d-f714-4a8e-8a2d-03e7204461c7','INSTAGRAM','Instagram',0,NULL);
/*!40000 ALTER TABLE `watch_type_providers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `watch_types`
--

DROP TABLE IF EXISTS `watch_types`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `watch_types` (
  `watch_type_id` int unsigned NOT NULL AUTO_INCREMENT,
  `watch_type_code` varchar(50) NOT NULL,
  `watch_type_name` varchar(100) NOT NULL,
  `active_ind` tinyint NOT NULL DEFAULT '0',
  PRIMARY KEY (`watch_type_id`),
  UNIQUE KEY `watch_id_UNIQUE` (`watch_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `watch_types`
--

LOCK TABLES `watch_types` WRITE;
/*!40000 ALTER TABLE `watch_types` DISABLE KEYS */;
INSERT INTO `watch_types` VALUES (1,'Tweet','A simple tweet',0),(2,'Hashtag','A hashtag',1),(3,'DM','A direct message',0);
/*!40000 ALTER TABLE `watch_types` ENABLE KEYS */;
UNLOCK TABLES;

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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 DROP PROCEDURE IF EXISTS `payment_services_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb3 */ ;
/*!50003 SET character_set_results = utf8mb3 */ ;
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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
/*!50003 SET collation_connection  = utf8mb3_general_ci */ ;
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

-- Dump completed on 2023-01-29 12:47:07
