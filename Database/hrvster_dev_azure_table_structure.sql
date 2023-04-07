-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: hrvster-dev-db.mysql.database.azure.com    Database: hrvster
-- ------------------------------------------------------
-- Server version	8.0.31

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
) ENGINE=InnoDB AUTO_INCREMENT=89 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  UNIQUE KEY `subscription_history_id_UNIQUE` (`subscription_history_id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  UNIQUE KEY `phone_number_UNIQUE` (`phone_number`),
  UNIQUE KEY `email_address_UNIQUE` (`email_address`),
  UNIQUE KEY `display_name_UNIQUE` (`display_name`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=103 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=713 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `flutterwave_account_subscriptions`
--

DROP TABLE IF EXISTS `flutterwave_account_subscriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `flutterwave_account_subscriptions` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `uid` varchar(50) NOT NULL,
  `subscription_id` int unsigned NOT NULL,
  `external_account_id` varchar(45) NOT NULL,
  `external_subscription_id` varchar(45) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`,`uid`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `uid_UNIQUE` (`uid`),
  KEY `client_subscription_account_subs_idx` (`subscription_id`),
  CONSTRAINT `client_subscription_account_subs` FOREIGN KEY (`subscription_id`) REFERENCES `account_subscriptions` (`subscription_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `amount` bigint NOT NULL DEFAULT '0',
  `charged_amount` bigint NOT NULL DEFAULT '0',
  `transaction_status` varchar(100) DEFAULT NULL,
  `currency` varchar(20) NOT NULL DEFAULT 'NGN',
  `payment_plan` bigint NOT NULL DEFAULT '0',
  `payment_page` bigint NOT NULL DEFAULT '0',
  `created_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`flutterwave_card_transaction_id`,`flutterwave_card_transaction_uid`,`id`,`transaction_reference`,`flow_reference`,`order_reference`),
  UNIQUE KEY `flutterwave_card_transaction_id_UNIQUE` (`flutterwave_card_transaction_id`),
  UNIQUE KEY `flutterwave_card_transaction_uid_UNIQUE` (`flutterwave_card_transaction_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=11958 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `prize_id` int unsigned DEFAULT NULL,
  `is_winnable` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`board_id`,`board_uid`),
  UNIQUE KEY `board_id_UNIQUE` (`board_id`),
  UNIQUE KEY `board_uid_UNIQUE` (`board_uid`),
  KEY `match_game_board_idx` (`match_game_id`),
  CONSTRAINT `match_game_board` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `ready_to_generate` tinyint NOT NULL DEFAULT '0',
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
  CONSTRAINT `match_game_campaign` FOREIGN KEY (`campaign_id`) REFERENCES `campaigns` (`campaign_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=74 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  CONSTRAINT `match_game_image` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `quantity` int DEFAULT '1',
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`prize_id`,`prize_uid`),
  UNIQUE KEY `prize_id_UNIQUE` (`prize_id`),
  UNIQUE KEY `prize_uid_UNIQUE` (`prize_uid`),
  KEY `match_game_prize_idx` (`match_game_id`),
  CONSTRAINT `match_game_prize` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  CONSTRAINT `match_tile_board` FOREIGN KEY (`board_id`) REFERENCES `match_boards` (`board_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `match_tile_image` FOREIGN KEY (`image_id`) REFERENCES `match_images` (`image_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `match_tile_prize` FOREIGN KEY (`prize_id`) REFERENCES `match_prizes` (`prize_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  UNIQUE KEY `plan_uid_UNIQUE` (`plan_uid`),
  UNIQUE KEY `plan_id_UNIQUE` (`plan_id`),
  UNIQUE KEY `plan_name_UNIQUE` (`plan_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `subscribers`
--

DROP TABLE IF EXISTS `subscribers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `subscribers` (
  `subscriber_id` bigint unsigned NOT NULL AUTO_INCREMENT,
  `email_address` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`subscriber_id`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=107 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=82 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=4333 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=69 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
  `draw_attempted` tinyint NOT NULL DEFAULT '0',
  `draw_happened` tinyint NOT NULL DEFAULT '0',
  `max_entries` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime DEFAULT NULL,
  `updated_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`sweep_notification_id`,`sweep_id`),
  UNIQUE KEY `sweep_notification_id_UNIQUE` (`sweep_notification_id`),
  UNIQUE KEY `sweep_id_UNIQUE` (`sweep_id`),
  KEY `sweep_notification_sweeps_idx` (`sweep_id`),
  CONSTRAINT `sweep_notification_sweeps` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=120 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=144 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=148 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=149 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
) ENGINE=InnoDB AUTO_INCREMENT=562 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-04-07 17:21:17
