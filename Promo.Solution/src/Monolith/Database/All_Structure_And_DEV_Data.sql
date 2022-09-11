-- MySQL dump 10.13  Distrib 8.0.27, for Win64 (x86_64)
--
-- Host: localhost    Database: hrvster
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
-- Current Database: `hrvster`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `hrvster` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `hrvster`;

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
  CONSTRAINT `fk_account_profile` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`),
  CONSTRAINT `fk_profile_type_profile` FOREIGN KEY (`profile_type_id`) REFERENCES `profile_types` (`profile_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=78 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_profiles`
--

LOCK TABLES `account_profiles` WRITE;
/*!40000 ALTER TABLE `account_profiles` DISABLE KEYS */;
INSERT INTO `account_profiles` VALUES (19,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','1254844223863558144',2,1,'Fruggal','fruggalapp','Joint Savings Made Simple!','1254844223863558144-7VscfIP1gd9NMhiBCUHfSBWn0klPoE','X9t1AyFg0VNHzSKYXd0nBRvNWz9wwHaP07uv8x7zY8ZZa','a8QkT2vtL9uFITULH43AlrMsK','1JQLFgrSqyOg7uB1hLuSe9lSyGxhLmYP38AHpk5FIWd0cgg7LL','Canada, Nigeria, Philippines ','https://pbs.twimg.com/profile_images/1344837609164472321/qiU8CxXg_400x400.jpg','2022-06-05 17:51:54','2022-08-17 16:01:26');
/*!40000 ALTER TABLE `account_profiles` ENABLE KEYS */;
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
  `email_address` varchar(400) NOT NULL,
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
  `created_date` timestamp NOT NULL,
  `updated_date` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`account_id`,`account_uid`,`email_address`,`phone_number`),
  UNIQUE KEY `user_id_UNIQUE` (`account_id`),
  UNIQUE KEY `user_uid_UNIQUE` (`account_uid`),
  UNIQUE KEY `email_address_UNIQUE` (`email_address`),
  UNIQUE KEY `phone_number_UNIQUE` (`phone_number`),
  UNIQUE KEY `display_name_UNIQUE` (`display_name`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','gamerwalt@gmail.com','+12048917575','gamerwaltgmail',NULL,NULL,NULL,_binary '\0',_binary '\0','123456','gamerwalt',_binary '','2022-02-26 17:57:49',NULL),(2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','gamerwalt@hotmail.com','+12048917675','gamerwalthotmail',NULL,NULL,NULL,_binary '\0',_binary '\0','123456','gamerwalt',_binary '','2022-02-27 13:15:47',NULL),(4,'cb940d4e-f3e0-4213-aa17-b11901f36244','example1@gmail.com','+12045093528','example',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=kKtcHXB86-CRt7t9as2qrG5QOliR4KaW84geBiKOJxAAAAGCDS3saw&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '\0',_binary '\0','123456',NULL,_binary '\0','2022-07-17 17:21:30',NULL),(5,'431006b3-c59e-43e7-8566-6360be87b79a','tester@tester1.com','+12048857748','tester1',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=r4xqj1Uqg6n-uOidMU8jf47uY68dwLjCZP8eX1mFaPIAAAGCUXxHHA&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '\0',_binary '\0','123456',NULL,_binary '','2022-07-30 23:41:14',NULL);
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
INSERT INTO `campaign_types` VALUES (1,'ad21aa80-6832-4713-b668-784d76fbdf11','AutoReplyTweet','Auto Reply Tweet','Campaign that only supports automatically replying to a specific Tweet id.',_binary '\0','2021-12-12 00:00:00',NULL),(2,'27e64712-3663-45c6-a32f-3e7a598c3b09','AutoReplyDM','Auto Reply DM','Campaign that only supports automatically replying to a specific DM Id.',_binary '\0','2021-12-12 00:00:00',NULL),(3,'4d5b1fdc-763a-48dd-89c6-09fe075bf1ee','AutoReplyHashtag','Auto Reply Hashtag','Campaign that only supports automatically replying to a specific Hashtag.',_binary '\0','2021-12-12 00:00:00',NULL),(4,'a1fcf285-96c9-4810-a9c5-eafed80f9d70','AutoReplyLikedTweet','Auto Reply Liked Tweet','Campaign that only supports automatically tweeting someone once they like a tweet.',_binary '\0','2021-12-12 00:00:00',NULL),(5,'5465b768-33a5-498a-8c8f-f8ff424e7098','AutoEnterSweepsFromTweet','Auto Enter Sweeps From Tweet','Campaign that supports automatically entering a sweepstakes when someone replies, likes or interacts with a tweet.',_binary '\0','2021-12-12 00:00:00',NULL),(6,'a445f2c8-dc06-4367-8597-c550fc02d7e4','AutoEnterSweepsFromHashtag','Auto Enter Sweeps From Hashtag','Campaign that supports automatically entering a sweepstakes when tweets a hashtag.',_binary '\0','2021-12-12 00:00:00',NULL),(7,'3b73d6c4-ebe3-4f68-9c73-845589d13d9f','AutoAll','Auto All','A la carte',_binary '\0','2021-12-12 00:00:00',NULL),(15,'3e5431d6-ba17-41f4-8f21-fd2d70245845','AutoEnterSweepsFromTweetReplyHashtag','Auto Enter Sweeps From Tweet Reply Hashtag','Campaign that supports automatically entering a sweepstakes when tweets a hashtag by replying a tweet',_binary '','2021-12-12 00:00:00',NULL);
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
  `active_ind` bit(1) NOT NULL DEFAULT b'1',
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`campaign_id`),
  UNIQUE KEY `campaign_id_UNIQUE` (`campaign_id`),
  UNIQUE KEY `campaign_uid_UNIQUE` (`campaign_uid`),
  UNIQUE KEY `account_to_campaign_unique` (`account_id`,`campaign_name`,`campaign_type_id`),
  KEY `campaign_type_campaign_idx` (`campaign_type_id`),
  CONSTRAINT `account_campaigns_fk` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`),
  CONSTRAINT `campaign_type_campaign` FOREIGN KEY (`campaign_type_id`) REFERENCES `campaign_types` (`campaign_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campaigns`
--

LOCK TABLES `campaigns` WRITE;
/*!40000 ALTER TABLE `campaigns` DISABLE KEYS */;
INSERT INTO `campaigns` VALUES (43,1,3,'55b3d165-b128-4df0-9aaf-fd58f589c77c','YYYYYYYYYYYDfdsf','sweeps campaign',_binary '\0','2022-02-27 00:00:00','2022-03-25 00:00:00','2022-02-26 20:07:50',NULL),(45,1,15,'1a602af3-b514-4e1c-b8bb-ba8885dbd6e3','Promo1','Description',_binary '','2022-02-28 00:00:00','2022-06-30 00:00:00','2022-02-26 20:08:28',NULL),(46,2,15,'1cb1c35a-53e1-4e81-bf01-6a1d6cbdf397','Promotest','Promotest Description Promotest Description Promotest Description',_binary '','2022-08-31 12:00:00','2022-11-30 11:59:00','2022-02-27 12:41:54',NULL),(48,1,15,'a7d7bf52-881a-4781-92a6-bc72a4c82e7d','lkdsafasdfsadf','sdafasdfadsf',_binary '','2022-05-08 19:08:00','2022-05-28 19:10:00','2022-05-08 13:09:07',NULL),(49,2,15,'3f917ad4-6f54-4ce0-8024-822a6108060e','New Promotion Test','New Promotion Test',_binary '','2022-09-30 04:00:00','2022-11-30 04:00:00','2022-05-08 22:01:11',NULL),(50,2,15,'66805f8b-771f-4fd7-a550-f227146f6ed3','Test','sdf asdfsadf',_binary '','2022-08-30 12:00:00','2022-11-30 23:59:00','2022-05-15 13:10:38',NULL),(55,2,6,'3aa651df-3f2e-4bd7-a31b-2b4f6b53c163','dfgdfg',NULL,_binary '','2022-08-24 00:00:00','2022-08-30 23:59:00','2022-08-18 12:02:04',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cron_services`
--

LOCK TABLES `cron_services` WRITE;
/*!40000 ALTER TABLE `cron_services` DISABLE KEYS */;
INSERT INTO `cron_services` VALUES (4,'ScheduledTweetService','hrvster.Services.ScheduledTweetService',1),(5,'TwitterHarvestingService','hrvster.Services.TwitterHarvestingService',1),(6,'SweepstakesDrawService','hrvster.Services.SweepstakesDrawService',1),(7,'EmailNotificationService','hrvster.Services.EmailNotificationService',1);
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emails`
--

LOCK TABLES `emails` WRITE;
/*!40000 ALTER TABLE `emails` DISABLE KEYS */;
INSERT INTO `emails` VALUES (9,'115b0202-b75b-422e-a00f-4ba08d06005a','gamerwalt@gmail.com','{\"PasswordResetLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=resetPassword&oobCode=bJBATF7jfL25nzPZg8pxMNooqjyi85NJMQ-lhUeduB4AAAGCUgZHJg&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','PasswordResetEmail','Sent',_binary '','2022-07-30 20:11:58','2022-08-19 08:00:51'),(11,'9ec997a6-993f-4b13-8e1f-5c616f04aee0','gamerwalt@hotmail.com','{\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=2sX3IFlXB_cw6pjgDjCpZUZuT518xj-TppH04jBjIskAAAGCthz_-A&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-08-19 06:38:49','2022-08-19 08:00:51');
/*!40000 ALTER TABLE `emails` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=39 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheduled_sweep_rule_tweet_images`
--

LOCK TABLES `scheduled_sweep_rule_tweet_images` WRITE;
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweet_images` DISABLE KEYS */;
INSERT INTO `scheduled_sweep_rule_tweet_images` VALUES (36,23,'https://firebasestorage.googleapis.com/v0/b/tweetuploads/o/c96686d0-7942-4b89-8fa5-dc0b0755ad6c%2FCreateAnAccount1.jpg?alt=media&token=65bf98d9-9cbd-48c0-a482-1a06daf0801c','2022-09-05 16:16:55',NULL),(37,23,'https://firebasestorage.googleapis.com/v0/b/tweetuploads/o/c96686d0-7942-4b89-8fa5-dc0b0755ad6c%2FCreateAnAccount1.jpg%2FCreateAnAccount2.jpg?alt=media&token=f543f62e-9798-4c61-a433-4acfc99eb9ca','2022-09-05 16:16:55',NULL),(38,23,'https://firebasestorage.googleapis.com/v0/b/tweetuploads/o/c96686d0-7942-4b89-8fa5-dc0b0755ad6c%2FCreateAnAccount1.jpg%2FCreateAnAccount2.jpg%2Fpexels-alexander-suhorucov-6457539.jpg?alt=media&token=31c386b1-00ad-44eb-8a26-baf24d3d4b4b','2022-09-05 16:16:55',NULL);
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
  `tweet_text` varchar(500) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `scheduled_sweep_rule_tweets`
--

LOCK TABLES `scheduled_sweep_rule_tweets` WRITE;
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweets` DISABLE KEYS */;
INSERT INTO `scheduled_sweep_rule_tweets` VALUES (15,'b586809a-852d-4532-9a74-9c43d2691971',19,70,'#TestScheduled updated','2022-08-31 11:50:00','2022-08-31 17:50:00','Sent','2022-08-21 15:01:22','2022-09-05 20:00:45'),(23,'cb2cfb83-81e3-4af6-afe0-a2f97c7ddabc',19,80,'#testupload1','2022-09-05 17:30:00','2022-09-05 23:30:00','Sent','2022-09-05 16:16:55','2022-09-05 20:01:06');
/*!40000 ALTER TABLE `scheduled_sweep_rule_tweets` ENABLE KEYS */;
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
  `end_date` datetime NOT NULL,
  `draw_date` datetime DEFAULT NULL,
  `drawn_ind` bit(1) NOT NULL DEFAULT b'0',
  `date_drawn` datetime DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_id`),
  UNIQUE KEY `sweep_id_UNIQUE` (`sweep_id`),
  UNIQUE KEY `sweep_uid_UNIQUE` (`sweep_uid`),
  UNIQUE KEY `sweep_name_campaign_unique` (`sweep_name`,`campaign_id`,`account_id`),
  KEY `fk_account_sweeps_idx` (`account_id`),
  KEY `fk_campaign_sweeps_idx` (`campaign_id`),
  CONSTRAINT `fk_account_sweeps` FOREIGN KEY (`account_id`) REFERENCES `accounts` (`account_id`),
  CONSTRAINT `fk_campaign_sweeps` FOREIGN KEY (`campaign_id`) REFERENCES `campaigns` (`campaign_id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep`
--

LOCK TABLES `sweep` WRITE;
/*!40000 ALTER TABLE `sweep` DISABLE KEYS */;
INSERT INTO `sweep` VALUES (18,'9a8c8a2e-5282-4b07-90cb-fbbd8eda2b54',1,45,'Sweeps 2',NULL,'2022-02-28 00:00:00','2022-06-30 00:00:00','2022-07-10 00:00:00',_binary '\0',NULL,'2022-02-27 07:14:51',NULL),(20,'68427326-a446-4b06-b8d3-db0709906279',1,45,'Sweeps 1',NULL,'2022-02-28 00:00:00','2022-06-30 00:00:00','2022-07-10 00:00:00',_binary '\0',NULL,'2022-02-27 07:15:14',NULL),(26,'0ce3d60e-99ee-4558-8551-cbb285043f3f',2,46,'My First Sweeps Campaign',NULL,'2022-09-01 12:00:00','2022-11-27 12:00:00','2022-11-29 23:59:00',_binary '\0',NULL,'2022-02-27 12:42:13','2022-09-03 23:36:49'),(27,'e190c342-a225-4039-9b03-123f141e3ec3',2,46,'Promotest-Sweepstakes',NULL,'2022-03-01 00:00:00','2022-07-28 00:00:00','2022-07-30 00:00:00',_binary '\0',NULL,'2022-05-08 14:02:42',NULL),(28,'5fff1eb8-80e6-481a-b9d5-14c95d786840',2,49,'New Promotion Test-Sweepstakes',NULL,'2022-05-10 04:00:00','2022-05-30 16:00:00','2022-06-10 16:00:00',_binary '\0',NULL,'2022-05-08 22:01:16',NULL),(29,'a6c1e928-bbd3-464c-8995-e8b87a2af3e4',2,50,'Test-Sweepstakes',NULL,'2022-05-16 19:10:00','2022-05-14 19:10:00','2022-05-25 19:10:00',_binary '\0',NULL,'2022-05-15 13:10:54',NULL),(30,'67d41301-bcbe-4bf4-9cf7-4fa7fe85d9d4',2,46,'Promotest-Sweepstakes 2',NULL,'2022-09-01 00:00:00','2022-10-31 00:00:00','2022-11-03 00:00:00',_binary '\0',NULL,'2022-08-14 12:29:45',NULL),(32,'9502fd4d-2a12-4404-95f6-b6f1e11bfcd0',2,50,'Test-Sweepstakes 3',NULL,'2022-08-31 12:00:00','2022-11-29 23:59:00','2022-11-30 23:59:00',_binary '\0',NULL,'2022-08-21 11:28:40',NULL),(33,'00000000-0000-0000-0000-000000000000',2,46,'Sweep 1',NULL,'2022-09-05 14:13:00','2022-10-04 14:13:00','2022-10-14 14:13:00',_binary '\0',NULL,'2022-09-04 14:14:12',NULL),(34,'aee6f1dd-6d99-4c42-bb81-79dbd15fa1e2',2,46,'Promotest-Sweepstakes 4',NULL,'2022-09-05 12:00:00','2022-11-29 11:59:00','2022-11-30 11:59:00',_binary '\0',NULL,'2022-09-04 14:14:41',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_draw_rules`
--

LOCK TABLES `sweep_draw_rules` WRITE;
/*!40000 ALTER TABLE `sweep_draw_rules` DISABLE KEYS */;
INSERT INTO `sweep_draw_rules` VALUES (7,'f00dcae8-c2a2-4553-bb65-eafebc0fb220',26,4,2,'2022-05-14 22:36:26','2022-05-18 06:01:23'),(8,'dd8d087c-f5b1-44a4-9da3-d20429a6065a',29,5,1,'2022-05-15 13:16:46',NULL),(9,'a2248f02-585c-4615-8850-105ceb76ddcb',27,2,1,'2022-06-26 09:40:36',NULL),(14,'cc1b9071-5dda-46a1-89eb-a86a8a391d3c',30,5,2,'2022-09-04 12:33:06',NULL),(22,'38fbf423-67b0-4e4b-b60c-4b56caa6587e',32,6,1,'2022-09-04 12:46:16','2022-09-04 12:55:57');
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
  CONSTRAINT `sweep_sweep_entry` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`)
) ENGINE=InnoDB AUTO_INCREMENT=158 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entry`
--

LOCK TABLES `sweep_entry` WRITE;
/*!40000 ALTER TABLE `sweep_entry` DISABLE KEYS */;
INSERT INTO `sweep_entry` VALUES (142,'726720c3-b1bd-4588-9862-16c3006beed4',26,60,'@fruggalapp','TwitterHandle',_binary '','2022-07-10 13:10:27',NULL),(143,'7d0f9d86-a049-44d9-adf1-e0e46de7489e',26,60,'@fruggalapp','TwitterHandle',_binary '','2022-07-10 13:11:04',NULL),(144,'56db3144-14d7-41fe-85c0-729ffe40606b',26,60,'@gamerwalt','TwitterHandle',_binary '','2022-07-10 13:12:57',NULL),(145,'c72282ea-adb5-4013-b6ff-ff4c3ee6f4e8',26,60,'@gamerwalt','TwitterHandle',_binary '','2022-07-10 13:42:11',NULL),(146,'405e04dc-a220-4c44-bb7c-459ab37f4d47',26,60,'@gamerwalt','TwitterHandle',_binary '','2022-07-10 13:52:30',NULL),(147,'c237193c-2725-42ca-8e51-2b3f7a7f9c1a',26,60,'@gamerwalt','TwitterHandle',_binary '','2022-07-10 13:52:53',NULL),(148,'4eb7bf98-98a2-48c1-95b8-372541e5c30f',26,60,'@gamerwalt','TwitterHandle',_binary '','2022-07-10 13:54:31',NULL),(149,'5251f2cf-1a71-4a40-88bd-ec9ffbe98e57',26,60,'@gamerwalt','TwitterHandle',_binary '','2022-07-10 13:55:07',NULL),(150,'3c062f57-51a6-4511-ae77-e03ba2d37f16',26,60,'@Mrssharlet002','TwitterHandle',_binary '','2022-07-10 17:09:02',NULL),(151,'17c1a757-e737-4b5e-9e23-d9d67e31a45a',26,60,'@hellohrvster','TwitterHandle',_binary '','2022-07-16 09:01:56',NULL),(152,'c101461a-8657-4906-995b-8c170c5ab919',26,60,'@hellohrvster','TwitterHandle',_binary '','2022-07-16 09:13:22',NULL),(153,'4cb73e9b-0682-4937-b8c0-379f507465b9',27,61,'@hellohrvster','TwitterHandle',_binary '','2022-07-23 13:52:09',NULL),(154,'6059f2d4-a63d-4628-b578-6324ad6edc42',27,61,'@hellohrvster','TwitterHandle',_binary '','2022-07-23 13:54:04',NULL),(155,'08343db6-a7fc-48a7-8ce3-83fd00603368',27,61,'@hellohrvster','TwitterHandle',_binary '','2022-07-23 13:54:33',NULL),(156,'a47ccbe7-a477-434b-b987-20dcc59246b3',26,60,'@hellohrvster','TwitterHandle',_binary '','2022-07-23 21:18:44',NULL),(157,'3b0e660d-8975-431f-aa1b-9e1e58f93eb5',26,60,'@hellohrvster','TwitterHandle',_binary '','2022-07-23 21:18:52',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entry_rules`
--

LOCK TABLES `sweep_entry_rules` WRITE;
/*!40000 ALTER TABLE `sweep_entry_rules` DISABLE KEYS */;
INSERT INTO `sweep_entry_rules` VALUES (9,'612e0fb3-cd69-4aca-b598-6f2cae2a0b38',29,15,'2022-05-15 13:16:29',NULL),(11,'180311c8-aa3b-425c-baa9-5f1d35d5149c',26,15,'2022-05-28 09:14:00','2022-07-10 13:38:11'),(12,'0ec93d71-67b6-4ec8-ad66-a3bd595dc508',27,15,'2022-07-09 17:02:18',NULL),(14,'133bbf97-70eb-4d49-b896-d00a4e3014fb',32,15,'2022-09-04 12:47:18','2022-09-04 12:55:49');
/*!40000 ALTER TABLE `sweep_entry_rules` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_prizes`
--

LOCK TABLES `sweep_prizes` WRITE;
/*!40000 ALTER TABLE `sweep_prizes` DISABLE KEYS */;
INSERT INTO `sweep_prizes` VALUES (13,'1e23f8ea-afc9-42bf-ba1d-2c58421629d8',1,26,_binary '','Prize 1 updated','Prize1 Description updated',599999.99,'English (Canada)','2022-06-11 18:36:31','2022-09-04 23:12:23'),(18,'400eaa04-97ae-401e-a099-bb2878913dfd',1,27,_binary '','Prize 1','Prize 1',20000.00,'English (Canada)','2022-06-26 09:58:54',NULL),(23,'8b870154-3cff-41b5-a8e4-68cb8936f86f',2,27,_binary '','Prize 2','Prize 2',5000.00,'English (Canada)','2022-07-03 23:22:12',NULL),(25,'88a6e1bb-9887-437d-834c-6f9738679220',2,26,_binary '','adfsad','sadf sadfasdf s',50000.00,'English (Canada)','2022-09-04 11:00:33',NULL),(26,'5cea6498-5d42-4ab0-a7ca-3c259953dc52',3,26,_binary '','sadf adsf','sdafdasf dasf ads fdasf',500000.00,'English (Canada)','2022-09-04 11:01:36',NULL);
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
  `notify_winners` tinyint DEFAULT '0',
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
  CONSTRAINT `sweep_id_rules` FOREIGN KEY (`sweep_id`) REFERENCES `sweep` (`sweep_id`),
  CONSTRAINT `watch_types_rules` FOREIGN KEY (`watch_type_id`) REFERENCES `watch_types` (`watch_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=81 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_rules`
--

LOCK TABLES `sweep_rules` WRITE;
/*!40000 ALTER TABLE `sweep_rules` DISABLE KEYS */;
INSERT INTO `sweep_rules` VALUES (60,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c',26,2,'#testsweeps1',1,0,_binary '\0',_binary '',1545907043814481921,_binary '\0',NULL,'2022-07-09 17:05:35',NULL),(61,'00207b98-b99c-4d89-9646-afb79b219e32',27,2,'#anotherhrvstertest',0,0,_binary '\0',_binary '',1548363904849309697,_binary '\0',NULL,'2022-07-16 11:48:16',NULL),(70,'1ff75966-1e45-4c9b-9b40-79d8ede5158c',32,2,'#TestScheduled',0,0,_binary '',_binary '',1566969625065275392,_binary '\0',NULL,'2022-08-21 15:01:22','2022-09-05 20:00:45'),(72,'302f9f38-f314-418b-87d3-c21effcc9ab4',32,2,'#testdeactivate',0,0,_binary '\0',_binary '',1562308928897429504,_binary '\0',NULL,'2022-08-23 23:20:48','2022-08-23 23:44:12'),(80,'23d1f16c-85e0-4d4b-bca8-4798334dfdf1',26,2,'#testupload1',0,0,_binary '',_binary '',1566969710872338432,_binary '\0',NULL,'2022-09-05 16:16:55','2022-09-05 20:01:06');
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
  `fulfilled_ind` bit(1) NOT NULL DEFAULT b'0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_winner_id`),
  UNIQUE KEY `sweep_winner_id_UNIQUE` (`sweep_winner_id`),
  UNIQUE KEY `sweep_winner_uid_UNIQUE` (`sweep_winner_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `selected_ind` bit(1) NOT NULL DEFAULT b'0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_winner_backup_id`),
  UNIQUE KEY `sweep_winner_backup_id_UNIQUE` (`sweep_winner_backup_id`),
  KEY `sweep_winners_backup_fk_idx` (`sweep_winner_id`),
  CONSTRAINT `sweep_winners_backup_fk` FOREIGN KEY (`sweep_winner_id`) REFERENCES `sweep_winners` (`sweep_winner_id`)
) ENGINE=InnoDB AUTO_INCREMENT=54 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners_backup`
--

LOCK TABLES `sweep_winners_backup` WRITE;
/*!40000 ALTER TABLE `sweep_winners_backup` DISABLE KEYS */;
/*!40000 ALTER TABLE `sweep_winners_backup` ENABLE KEYS */;
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
-- Dumping routines for database 'hrvster'
--
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
/*!50003 DROP PROCEDURE IF EXISTS `campaigns_get_by_account_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date,
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
        ct.active_ind as campaign_type_active_ind
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_create`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(50), IN campaignDescription VARCHAR(500), IN startDate DATETIME, IN endDate DATETIME, IN campaignTypeUid VARCHAR(50), IN accountUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
    DECLARE accountId INT DEFAULT 0;

    SELECT @campaignTypeId := campaign_type_id FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;
    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
    
    SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
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
        end_date,
        created_date
    )
    SELECT
		@accountId,
        @campaignTypeId,
        campaignUid,
        campaignName,
        campaignDescription,
        startDate,
        endDate,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date,
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
        ct.active_ind as campaign_type_active_ind
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date,
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
        ct.active_ind as campaign_type_active_ind
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_update_via_uid`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(50), IN campaignDescription VARCHAR(500), IN startDate DATETIME, IN endDate DATETIME, IN activeInd BIT, IN campaignTypeUid VARCHAR(50))
BEGIN
	DECLARE campaignTypeId INT DEFAULT 0;
    
	SELECT @campaignTypeId := campaign_type_id FROM campaign_types WHERE campaign_type_uid = campaignTypeUid;

    IF(@campaignTypeId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
	

	UPDATE campaigns
		SET campaign_name = campaignName,
			campaign_description = campaignDescription,
            start_date = startDate,
            end_date = endDate,
            active_ind = activeInd,
            campaign_type_id = @campaignTypeId
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
        AP.profile_image_url
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
/*!50003 DROP PROCEDURE IF EXISTS `scheduled_sweep_rule_tweets_get_not_sent` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
    WHERE scheduled_time_utc <= utcDate
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        s.end_date,
        s.draw_date,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        s.end_date,
        s.draw_date,
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
/*!50003 DROP PROCEDURE IF EXISTS `sweeps_requiring_draw_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        S.end_date as sweep_end_date,
        S.draw_date as sweep_draw_date,
        S.drawn_ind as sweep_drawn_indicator,
        S.date_drawn as sweep_date_drawn,
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
        SDR.winner_backup_count
    FROM sweep as S
    INNER JOIN campaigns as C ON C.campaign_id = S.campaign_id
    INNER JOIN campaign_types as CT ON CT.campaign_type_id = c.campaign_type_id
    INNER JOIN sweep_rules as SR ON SR.sweep_id = S.sweep_id
    INNER JOIN sweep_draw_rules as SDR ON SDR.sweep_id = S.sweep_id
    INNER JOIN watch_types as WT ON WT.watch_type_id = SR.watch_type_id
    WHERE S.start_date < CURDATE()
    AND S.end_date <= CURDATE()
    AND S.draw_date <= CURDATE()
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
/*!50003 DROP PROCEDURE IF EXISTS `sweep_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_create`(IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50), IN accountUid VARCHAR(50), IN sweepName VARCHAR(100), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
BEGIN
	DECLARE campaignId INT DEFAULT 0;
    DECLARE accountId INT DEFAULT 0;

    SELECT @campaignId := campaign_id FROM campaigns WHERE campaign_uid = campaignUid;
    IF(@campaignId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
    
    SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
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
        end_date,
        draw_date,
        created_date
    )
    VALUES
    (
		sweepUid,
        @campaignId,
        @accountId,
        sweepName,
        startDate,
        endDate,
        drawDate,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        s.end_date,
        s.draw_date,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        s.end_date,
        s.draw_date,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        S.end_date,
        S.draw_date,
        S.drawn_ind,
        S.date_drawn
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date as campaign_end_date,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.end_date,
        s.draw_date,
        s.drawn_ind,
        s.date_drawn,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.external_id,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    WHERE (currentUtc BETWEEN c.start_date AND c.end_date)
	AND (currentUtc BETWEEN s.start_date AND s.end_date)
    AND (currentUtc < s.draw_date)
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date as campaign_end_date,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.end_date,
        s.draw_date,
        s.drawn_ind,
        s.date_drawn,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.created_date,
        r.updated_date,
        r.external_id,
        r.must_be_following,
        wt.watch_type_id,
        wt.watch_type_code,
        wt.watch_type_name
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON wt.watch_type_id = r.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date as campaign_end_date,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.end_date,
        s.draw_date,
        s.drawn_ind,
        s.date_drawn,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
		r.external_id,
        wv.watch_type_id,
        wv.watch_type_code,
        wv.watch_type_name,
        r.must_be_following,
        r.created_date,
        r.updated_date
    FROM sweep_rules as r
    INNER JOIN watch_types as wv ON wv.watch_type_id = r.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date as campaign_end_date,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.end_date,
        s.draw_date,
        s.drawn_ind,
        s.date_drawn,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
		r.external_id,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON r.watch_type_id = wt.watch_type_id
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
/*!50003 DROP PROCEDURE IF EXISTS `sweep_rules_get_not_added_for_tracking` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
		s.end_date,
		s.draw_date,
		s.drawn_ind,
		s.date_drawn,
		r.watch_value,
		r.active_ind,
		r.scheduled_ind,
		r.external_id,
		wv.watch_type_id,
		wv.watch_type_code,
		wv.watch_type_name,
		r.must_be_following,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        s.end_date,
        s.draw_date,
        s.drawn_ind,
        s.date_drawn,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
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
        c.end_date as campaign_end_date,
        ct.campaign_type_code,
        ct.campaign_type_name,
        ct.campaign_type_description,
        s.sweep_id,
        s.sweep_uid,
        s.sweep_name,
        s.rules_url,
        s.start_date,
        s.end_date,
        s.draw_date,
        s.drawn_ind,
        s.date_drawn,
        r.watch_value,
        r.active_ind,
        r.scheduled_ind,
        r.added_for_tracking,
        r.date_added_for_tracking,
        r.must_be_following,
        r.external_id,
        wt.watch_type_code,
        wt.watch_type_name,
        r.created_date,
        r.updated_date
    FROM sweep_rules as r
    INNER JOIN watch_types as wt ON wt.watch_type_id = r.watch_type_id
    INNER JOIN sweep as s ON s.sweep_id = r.sweep_id
    INNER JOIN campaigns as c ON c.campaign_id = s.campaign_id
    INNER JOIN campaign_types as CT on CT.campaign_type_id = c.campaign_type_id
    WHERE r.rule_uid = ruleUid;
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
/*!50003 DROP PROCEDURE IF EXISTS `sweep_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_update`(IN sweepUid VARCHAR(50), IN sweepName VARCHAR(100), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
BEGIN
	UPDATE sweep
		SET sweep_name = sweepName,
			start_date = startDate,
            end_date = endDate,
            draw_date = drawDate,
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_winner_backups_get_by_sweep_winner_uid`(IN sweepWinnerUid VARCHAR(50))
BEGIN
	SELECT
		swb.sweep_winner_backup_id,
        swb.sweep_winner_id,
        swb.sweep_entry_uid,
        swb.sweep_uid,
        swb.user_identifier,
        swb.user_identifier_type,
        swb.selected_ind,
        swb.created_date,
        swb.updated_date
    FROM sweep_winners_backup as swb
    INNER JOIN sweep_winners as sw ON sw.sweep_winner_id = swb.sweep_winner_id
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

--
-- Current Database: `promo.harvester`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `promo.harvester` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `promo.harvester`;

--
-- Table structure for table `blacklisted_sweeps_users`
--

DROP TABLE IF EXISTS `blacklisted_sweeps_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `blacklisted_sweeps_users` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweeps_uid` varchar(50) NOT NULL,
  `tweet_user_id` bigint NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `blacklisted_sweeps_users`
--

LOCK TABLES `blacklisted_sweeps_users` WRITE;
/*!40000 ALTER TABLE `blacklisted_sweeps_users` DISABLE KEYS */;
INSERT INTO `blacklisted_sweeps_users` VALUES (2,'df90f3e2-ca9c-4af2-aa3f-d449633865a4',1476226447916163155);
/*!40000 ALTER TABLE `blacklisted_sweeps_users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `harvested`
--

DROP TABLE IF EXISTS `harvested`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `harvested` (
  `harvested_id` int unsigned NOT NULL AUTO_INCREMENT,
  `rule_uid` varchar(45) NOT NULL,
  `harvested_uid` varchar(50) NOT NULL,
  `harvested_value` varchar(8000) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`harvested_id`),
  UNIQUE KEY `id_UNIQUE` (`harvested_id`),
  UNIQUE KEY `harvested_uid_UNIQUE` (`harvested_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=269 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `harvested`
--

LOCK TABLES `harvested` WRITE;
/*!40000 ALTER TABLE `harvested` DISABLE KEYS */;
INSERT INTO `harvested` VALUES (238,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','063dac72-bf47-4f8c-b6ef-58efeaabf96b','{\"created_at\":\"Sun Jul 10 18:40:05 +0000 2022\",\"id\":1546202615993835520,\"id_str\":\"1546202615993835520\",\"text\":\"@fruggalapp Test9 #testsweeps1\",\"display_text_range\":[12,30],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2776,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17472,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[18,30]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657478405365\"}','2022-07-10 12:40:14',NULL),(241,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','c62a3278-97f6-4895-9a5d-8af17bd6dbaa','{\"created_at\":\"Sun Jul 10 18:53:47 +0000 2022\",\"id\":1546206065427189761,\"id_str\":\"1546206065427189761\",\"text\":\"RT @fruggalapp: This is a test for #testsweeps1 https:\\/\\/t.co\\/qMshHM0RTZ\",\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/iphone\\\" rel=\\\"nofollow\\\"\\u003eTwitter for iPhone\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":2873509995,\"id_str\":\"2873509995\",\"name\":\"taoslaw\",\"screen_name\":\"tolakpe\",\"location\":\"Laguna, CA\",\"url\":null,\"description\":\"Bussiness developer..logistic..Haulage..Music..religious and education activist..\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":154,\"friends_count\":671,\"listed_count\":4,\"favourites_count\":4414,\"statuses_count\":2731,\"created_at\":\"Wed Nov 12 09:10:54 +0000 2014\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"C0DEED\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme1\\/bg.png\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1489107541904830464\\/gJRhQUu-_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1489107541904830464\\/gJRhQUu-_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/2873509995\\/1582731425\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"retweeted_status\":{\"created_at\":\"Sat Jul 09 23:05:35 +0000 2022\",\"id\":1545907043814481921,\"id_str\":\"1545907043814481921\",\"text\":\"This is a test for #testsweeps1 https:\\/\\/t.co\\/qMshHM0RTZ\",\"display_text_range\":[0,31],\"source\":\"\\u003ca href=\\\"http:\\/\\/hrvster.com\\/\\\" rel=\\\"nofollow\\\"\\u003ehrvsterService\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":null,\"in_reply_to_status_id_str\":null,\"in_reply_to_user_id\":null,\"in_reply_to_user_id_str\":null,\"in_reply_to_screen_name\":null,\"user\":{\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"name\":\"Fruggal\",\"screen_name\":\"fruggalapp\",\"location\":\"Canada, Nigeria, Philippines \",\"url\":null,\"description\":\"Joint Savings Made Simple!\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":29,\"friends_count\":21,\"listed_count\":0,\"favourites_count\":25,\"statuses_count\":127,\"created_at\":\"Mon Apr 27 18:47:19 +0000 2020\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1344837609164472321\\/qiU8CxXg_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1344837609164472321\\/qiU8CxXg_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/1254844223863558144\\/1609469247\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":16,\"retweet_count\":1,\"favorite_count\":2,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[19,31]}],\"urls\":[],\"user_mentions\":[],\"symbols\":[],\"media\":[{\"id\":1545907032817078273,\"id_str\":\"1545907032817078273\",\"indices\":[32,55],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"url\":\"https:\\/\\/t.co\\/qMshHM0RTZ\",\"display_url\":\"pic.twitter.com\\/qMshHM0RTZ\",\"expanded_url\":\"https:\\/\\/twitter.com\\/fruggalapp\\/status\\/1545907043814481921\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"large\":{\"w\":1365,\"h\":2048,\"resize\":\"fit\"},\"medium\":{\"w\":800,\"h\":1200,\"resize\":\"fit\"},\"small\":{\"w\":453,\"h\":680,\"resize\":\"fit\"}}}]},\"extended_entities\":{\"media\":[{\"id\":1545907032817078273,\"id_str\":\"1545907032817078273\",\"indices\":[32,55],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"url\":\"https:\\/\\/t.co\\/qMshHM0RTZ\",\"display_url\":\"pic.twitter.com\\/qMshHM0RTZ\",\"expanded_url\":\"https:\\/\\/twitter.com\\/fruggalapp\\/status\\/1545907043814481921\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"large\":{\"w\":1365,\"h\":2048,\"resize\":\"fit\"},\"medium\":{\"w\":800,\"h\":1200,\"resize\":\"fit\"},\"small\":{\"w\":453,\"h\":680,\"resize\":\"fit\"}}}]},\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"filter_level\":\"low\",\"lang\":\"en\"},\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[35,47]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[3,14]}],\"symbols\":[],\"media\":[{\"id\":1545907032817078273,\"id_str\":\"1545907032817078273\",\"indices\":[48,71],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"url\":\"https:\\/\\/t.co\\/qMshHM0RTZ\",\"display_url\":\"pic.twitter.com\\/qMshHM0RTZ\",\"expanded_url\":\"https:\\/\\/twitter.com\\/fruggalapp\\/status\\/1545907043814481921\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"large\":{\"w\":1365,\"h\":2048,\"resize\":\"fit\"},\"medium\":{\"w\":800,\"h\":1200,\"resize\":\"fit\"},\"small\":{\"w\":453,\"h\":680,\"resize\":\"fit\"}},\"source_status_id\":1545907043814481921,\"source_status_id_str\":\"1545907043814481921\",\"source_user_id\":1254844223863558144,\"source_user_id_str\":\"1254844223863558144\"}]},\"extended_entities\":{\"media\":[{\"id\":1545907032817078273,\"id_str\":\"1545907032817078273\",\"indices\":[48,71],\"media_url\":\"http:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"media_url_https\":\"https:\\/\\/pbs.twimg.com\\/media\\/FXQqQXrXEAEp8eo.jpg\",\"url\":\"https:\\/\\/t.co\\/qMshHM0RTZ\",\"display_url\":\"pic.twitter.com\\/qMshHM0RTZ\",\"expanded_url\":\"https:\\/\\/twitter.com\\/fruggalapp\\/status\\/1545907043814481921\\/photo\\/1\",\"type\":\"photo\",\"sizes\":{\"thumb\":{\"w\":150,\"h\":150,\"resize\":\"crop\"},\"large\":{\"w\":1365,\"h\":2048,\"resize\":\"fit\"},\"medium\":{\"w\":800,\"h\":1200,\"resize\":\"fit\"},\"small\":{\"w\":453,\"h\":680,\"resize\":\"fit\"}},\"source_status_id\":1545907043814481921,\"source_status_id_str\":\"1545907043814481921\",\"source_user_id\":1254844223863558144,\"source_user_id_str\":\"1254844223863558144\"}]},\"favorited\":false,\"retweeted\":false,\"possibly_sensitive\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657479227774\"}','2022-07-10 12:53:51',NULL),(243,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','736cd919-ce74-4e64-9d8d-3a96aa923be4','{\"created_at\":\"Sun Jul 10 19:11:00 +0000 2022\",\"id\":1546210396654096385,\"id_str\":\"1546210396654096385\",\"text\":\"Test13 #testsweeps1\",\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"name\":\"Fruggal\",\"screen_name\":\"fruggalapp\",\"location\":\"Canada, Nigeria, Philippines \",\"url\":null,\"description\":\"Joint Savings Made Simple!\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":29,\"friends_count\":21,\"listed_count\":0,\"favourites_count\":25,\"statuses_count\":129,\"created_at\":\"Mon Apr 27 18:47:19 +0000 2020\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1344837609164472321\\/qiU8CxXg_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1344837609164472321\\/qiU8CxXg_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/1254844223863558144\\/1609469247\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[7,19]}],\"urls\":[],\"user_mentions\":[],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657480260419\"}','2022-07-10 13:11:04',NULL),(244,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','f16119aa-b7b3-4b98-84a6-9360ade94e35','{\"created_at\":\"Sun Jul 10 19:12:53 +0000 2022\",\"id\":1546210871713533952,\"id_str\":\"1546210871713533952\",\"text\":\"@fruggalapp This is a test #testsweeps1\",\"display_text_range\":[12,39],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17475,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[27,39]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657480373682\"}','2022-07-10 13:12:57',NULL),(245,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','c7e50815-3893-428c-84cb-c80ef716c3bb','{\"created_at\":\"Sun Jul 10 19:16:56 +0000 2022\",\"id\":1546211892154159104,\"id_str\":\"1546211892154159104\",\"text\":\"@fruggalapp This is another test #testsweeps1\",\"display_text_range\":[12,45],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17476,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[33,45]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657480616974\"}','2022-07-10 13:17:01',NULL),(246,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','ad5f84f7-3237-4750-b334-39edef6779f1','{\"created_at\":\"Sun Jul 10 19:18:29 +0000 2022\",\"id\":1546212278462099456,\"id_str\":\"1546212278462099456\",\"text\":\"@fruggalapp Testing another #testsweeps1\",\"display_text_range\":[12,40],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17477,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[28,40]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657480709077\"}','2022-07-10 13:18:33',NULL),(247,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','b2a1d75c-16a8-4cea-89d8-13396364445f','{\"created_at\":\"Sun Jul 10 19:20:20 +0000 2022\",\"id\":1546212743824388096,\"id_str\":\"1546212743824388096\",\"text\":\"@fruggalapp Another #testsweeps1\",\"display_text_range\":[12,32],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17478,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[20,32]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657480820028\"}','2022-07-10 13:20:23',NULL),(248,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','25dffe97-325a-4507-9b17-111a33848d8d','{\"created_at\":\"Sun Jul 10 19:26:51 +0000 2022\",\"id\":1546214385793634305,\"id_str\":\"1546214385793634305\",\"text\":\"@fruggalapp Another test #testsweeps1\",\"display_text_range\":[12,37],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17479,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[25,37]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657481211504\"}','2022-07-10 13:26:55',NULL),(249,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','25455c5c-c119-4ea9-8dcd-017c86e845cb','{\"created_at\":\"Sun Jul 10 19:39:07 +0000 2022\",\"id\":1546217470981054464,\"id_str\":\"1546217470981054464\",\"text\":\"@fruggalapp Another test here #testsweeps1\",\"display_text_range\":[12,42],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17480,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[30,42]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657481947070\"}','2022-07-10 13:39:11',NULL),(250,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','e3d09055-2420-48cd-b83d-a9035cc85fe0','{\"created_at\":\"Sun Jul 10 19:39:40 +0000 2022\",\"id\":1546217612912144384,\"id_str\":\"1546217612912144384\",\"text\":\"@fruggalapp Testing again #testsweeps1\",\"display_text_range\":[12,38],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17481,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[26,38]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657481980909\"}','2022-07-10 13:39:44',NULL),(251,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','59e96871-5bda-41af-8289-f5612ee0f19c','{\"created_at\":\"Sun Jul 10 19:52:12 +0000 2022\",\"id\":1546220764713537537,\"id_str\":\"1546220764713537537\",\"text\":\"@fruggalapp Test 30 #testsweeps1\",\"display_text_range\":[12,32],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":19227042,\"id_str\":\"19227042\",\"name\":\"Wale \\ud83c\\uddf3\\ud83c\\uddec \\ud83c\\uddf5\\ud83c\\udded \\ud83c\\udde8\\ud83c\\udde6\",\"screen_name\":\"gamerwalt\",\"location\":\"Canada\",\"url\":null,\"description\":\"Husband to @shellyjo002.\\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \\nI speak C#, PHP, ETL\\n\\nBuilding @hrvsterHQ\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":891,\"friends_count\":2777,\"listed_count\":7,\"favourites_count\":20830,\"statuses_count\":17485,\"created_at\":\"Tue Jan 20 10:23:51 +0000 2009\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":true,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"000000\",\"profile_background_image_url\":\"http:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_image_url_https\":\"https:\\/\\/abs.twimg.com\\/images\\/themes\\/theme2\\/bg.gif\",\"profile_background_tile\":false,\"profile_link_color\":\"1B95E0\",\"profile_sidebar_border_color\":\"000000\",\"profile_sidebar_fill_color\":\"000000\",\"profile_text_color\":\"000000\",\"profile_use_background_image\":false,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1519644986597249024\\/Yorv7EQb_normal.jpg\",\"profile_banner_url\":\"https:\\/\\/pbs.twimg.com\\/profile_banners\\/19227042\\/1646712780\",\"default_profile\":false,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[20,32]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657482732357\"}','2022-07-10 13:52:20',NULL),(253,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','38dea35b-d35d-47bf-9f7a-76c14ff17025','{\"created_at\":\"Sun Jul 10 23:03:23 +0000 2022\",\"id\":1546268877348229125,\"id_str\":\"1546268877348229125\",\"text\":\"@fruggalapp This is test1 #testsweeps1 of not following Fruggal\",\"display_text_range\":[12,63],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1485682131762401280,\"id_str\":\"1485682131762401280\",\"name\":\"Wally\",\"screen_name\":\"WallyAdewoyin\",\"location\":null,\"url\":null,\"description\":null,\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":1,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":10,\"created_at\":\"Mon Jan 24 18:34:28 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[26,38]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657494203303\"}','2022-07-10 17:03:28',NULL),(254,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','b91b9661-f7ae-405b-bb0c-807c181f5055','{\"created_at\":\"Sun Jul 10 23:05:27 +0000 2022\",\"id\":1546269397618020353,\"id_str\":\"1546269397618020353\",\"text\":\"@fruggalapp This is another test 2 #testsweeps1 of not following Fruggal\",\"display_text_range\":[12,72],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1485682131762401280,\"id_str\":\"1485682131762401280\",\"name\":\"Wally\",\"screen_name\":\"WallyAdewoyin\",\"location\":null,\"url\":null,\"description\":null,\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":1,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":11,\"created_at\":\"Mon Jan 24 18:34:28 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[35,47]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657494327345\"}','2022-07-10 17:05:32',NULL),(255,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','44c56eee-cbe3-4cae-8ed5-3acb7176347c','{\"created_at\":\"Sun Jul 10 23:08:17 +0000 2022\",\"id\":1546270111912173569,\"id_str\":\"1546270111912173569\",\"text\":\"@fruggalapp Testing #testsweeps1\",\"display_text_range\":[12,32],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1498513306323013632,\"id_str\":\"1498513306323013632\",\"name\":\"sharletlifestyle_ngca\",\"screen_name\":\"Mrssharlet002\",\"location\":null,\"url\":null,\"description\":\"Just living and sharing!\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":1,\"friends_count\":4,\"listed_count\":0,\"favourites_count\":19,\"statuses_count\":3,\"created_at\":\"Tue Mar 01 04:20:04 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1498514386880581637\\/lRQMZFpJ_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1498514386880581637\\/lRQMZFpJ_normal.jpg\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[20,32]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657494497646\"}','2022-07-10 17:08:22',NULL),(256,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','eef5a676-fc4b-449a-8462-cbf1e6834672','{\"created_at\":\"Sun Jul 10 23:08:57 +0000 2022\",\"id\":1546270278157627392,\"id_str\":\"1546270278157627392\",\"text\":\"@fruggalapp #testsweeps1 testing\",\"display_text_range\":[12,32],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1498513306323013632,\"id_str\":\"1498513306323013632\",\"name\":\"sharletlifestyle_ngca\",\"screen_name\":\"Mrssharlet002\",\"location\":null,\"url\":null,\"description\":\"Just living and sharing!\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":1,\"friends_count\":5,\"listed_count\":0,\"favourites_count\":19,\"statuses_count\":4,\"created_at\":\"Tue Mar 01 04:20:04 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/pbs.twimg.com\\/profile_images\\/1498514386880581637\\/lRQMZFpJ_normal.jpg\",\"profile_image_url_https\":\"https:\\/\\/pbs.twimg.com\\/profile_images\\/1498514386880581637\\/lRQMZFpJ_normal.jpg\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[12,24]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657494537282\"}','2022-07-10 17:09:01',NULL),(257,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','89344430-0653-4848-bdbb-377b14f2699e','{\"created_at\":\"Sat Jul 16 15:01:02 +0000 2022\",\"id\":1548321816816406531,\"id_str\":\"1548321816816406531\",\"text\":\"@fruggalapp A test made here for #testsweeps1 1\",\"display_text_range\":[12,47],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":18,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":1,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[33,45]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657983662214\"}','2022-07-16 09:01:06',NULL),(258,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','f5884121-2a49-45c5-a581-64c5846e1cbd','{\"created_at\":\"Sat Jul 16 15:01:52 +0000 2022\",\"id\":1548322027991212033,\"id_str\":\"1548322027991212033\",\"text\":\"@fruggalapp Another testing #testsweeps1 2\",\"display_text_range\":[12,42],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":2,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[28,40]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657983712562\"}','2022-07-16 09:01:56',NULL),(259,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','b846108c-4080-468a-bf37-f9bb19606336','{\"created_at\":\"Sat Jul 16 15:13:18 +0000 2022\",\"id\":1548324903635390464,\"id_str\":\"1548324903635390464\",\"text\":\"@fruggalapp Another test #testsweeps1 2\",\"display_text_range\":[12,39],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":3,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[25,37]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1657984398169\"}','2022-07-16 09:13:22',NULL),(260,'00207b98-b99c-4d89-9646-afb79b219e32','68262b8f-7005-4c3a-9aec-dea316133655','{\"created_at\":\"Sun Jul 17 19:09:57 +0000 2022\",\"id\":1548746848172756993,\"id_str\":\"1548746848172756993\",\"text\":\"@fruggalapp Hi there #anotherhrvstertest 1\",\"display_text_range\":[12,42],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1548363904849309697,\"in_reply_to_status_id_str\":\"1548363904849309697\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":4,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"anotherhrvstertest\",\"indices\":[21,40]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658084997586\"}','2022-07-17 13:10:03',NULL),(261,'00207b98-b99c-4d89-9646-afb79b219e32','15fcd0bb-455d-425b-a8cf-a31b77167be1','{\"created_at\":\"Sat Jul 23 19:14:12 +0000 2022\",\"id\":1550922244943134731,\"id_str\":\"1550922244943134731\",\"text\":\"@fruggalapp #anotherhrvstertest this is a test 1\",\"display_text_range\":[12,48],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1548363904849309697,\"in_reply_to_status_id_str\":\"1548363904849309697\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":5,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"anotherhrvstertest\",\"indices\":[12,31]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658603652593\"}','2022-07-23 13:14:18',NULL),(262,'00207b98-b99c-4d89-9646-afb79b219e32','71713f0a-5292-4ca9-9211-a06c4a866e0f','{\"created_at\":\"Sat Jul 23 19:42:59 +0000 2022\",\"id\":1550929489651171331,\"id_str\":\"1550929489651171331\",\"text\":\"@fruggalapp #anotherhrvstertest this is test 2\",\"display_text_range\":[12,46],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1548363904849309697,\"in_reply_to_status_id_str\":\"1548363904849309697\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":6,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"anotherhrvstertest\",\"indices\":[12,31]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658605379866\"}','2022-07-23 13:43:05',NULL),(264,'00207b98-b99c-4d89-9646-afb79b219e32','3be52d7c-9d34-48fa-81e3-e2c2ef4bd7df','{\"created_at\":\"Sat Jul 23 19:52:03 +0000 2022\",\"id\":1550931769792385025,\"id_str\":\"1550931769792385025\",\"text\":\"@fruggalapp #anotherhrvstertest this is test 4\",\"display_text_range\":[12,46],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1548363904849309697,\"in_reply_to_status_id_str\":\"1548363904849309697\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":8,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"anotherhrvstertest\",\"indices\":[12,31]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658605923494\"}','2022-07-23 13:52:09',NULL),(265,'00207b98-b99c-4d89-9646-afb79b219e32','3928a92f-ed77-4254-aa6e-be7276e0dc88','{\"created_at\":\"Sat Jul 23 19:53:58 +0000 2022\",\"id\":1550932252430843911,\"id_str\":\"1550932252430843911\",\"text\":\"@fruggalapp #anotherhrvstertest test 5\",\"display_text_range\":[12,38],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1548363904849309697,\"in_reply_to_status_id_str\":\"1548363904849309697\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":9,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"anotherhrvstertest\",\"indices\":[12,31]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658606038564\"}','2022-07-23 13:54:04',NULL),(266,'00207b98-b99c-4d89-9646-afb79b219e32','b2434f63-8439-4e34-aafe-0384ecefa19b','{\"created_at\":\"Sat Jul 23 19:54:27 +0000 2022\",\"id\":1550932374023798786,\"id_str\":\"1550932374023798786\",\"text\":\"@fruggalapp #anotherhrvstertest test 6\",\"display_text_range\":[12,38],\"source\":\"\\u003ca href=\\\"http:\\/\\/twitter.com\\/download\\/android\\\" rel=\\\"nofollow\\\"\\u003eTwitter for Android\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1548363904849309697,\"in_reply_to_status_id_str\":\"1548363904849309697\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":10,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"anotherhrvstertest\",\"indices\":[12,31]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658606067554\"}','2022-07-23 13:54:33',NULL),(267,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','15f5a169-e126-4540-8595-f19cedfce85c','{\"created_at\":\"Sun Jul 24 03:18:38 +0000 2022\",\"id\":1551044155845083136,\"id_str\":\"1551044155845083136\",\"text\":\"@fruggalapp #testsweeps1 testing the timeline graph 1\",\"display_text_range\":[12,53],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":11,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[12,24]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658632718417\"}','2022-07-23 21:18:44',NULL),(268,'fda238d6-3ac7-45e1-85c6-9f13bb91f83c','7cf7c739-082a-48f0-8b3e-a6a7010f05fb','{\"created_at\":\"Sun Jul 24 03:18:47 +0000 2022\",\"id\":1551044192830459908,\"id_str\":\"1551044192830459908\",\"text\":\"@fruggalapp #testsweeps1 testing the timeline graph 2\",\"display_text_range\":[12,53],\"source\":\"\\u003ca href=\\\"https:\\/\\/mobile.twitter.com\\\" rel=\\\"nofollow\\\"\\u003eTwitter Web App\\u003c\\/a\\u003e\",\"truncated\":false,\"in_reply_to_status_id\":1545907043814481921,\"in_reply_to_status_id_str\":\"1545907043814481921\",\"in_reply_to_user_id\":1254844223863558144,\"in_reply_to_user_id_str\":\"1254844223863558144\",\"in_reply_to_screen_name\":\"fruggalapp\",\"user\":{\"id\":1548124411529572353,\"id_str\":\"1548124411529572353\",\"name\":\"hello@hrvster\",\"screen_name\":\"hellohrvster\",\"location\":null,\"url\":null,\"description\":\"Customer support @hrvster\",\"translator_type\":\"none\",\"protected\":false,\"verified\":false,\"followers_count\":0,\"friends_count\":19,\"listed_count\":0,\"favourites_count\":0,\"statuses_count\":12,\"created_at\":\"Sat Jul 16 01:56:45 +0000 2022\",\"utc_offset\":null,\"time_zone\":null,\"geo_enabled\":false,\"lang\":null,\"contributors_enabled\":false,\"is_translator\":false,\"profile_background_color\":\"F5F8FA\",\"profile_background_image_url\":\"\",\"profile_background_image_url_https\":\"\",\"profile_background_tile\":false,\"profile_link_color\":\"1DA1F2\",\"profile_sidebar_border_color\":\"C0DEED\",\"profile_sidebar_fill_color\":\"DDEEF6\",\"profile_text_color\":\"333333\",\"profile_use_background_image\":true,\"profile_image_url\":\"http:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"profile_image_url_https\":\"https:\\/\\/abs.twimg.com\\/sticky\\/default_profile_images\\/default_profile_normal.png\",\"default_profile\":true,\"default_profile_image\":false,\"following\":null,\"follow_request_sent\":null,\"notifications\":null,\"withheld_in_countries\":[]},\"geo\":null,\"coordinates\":null,\"place\":null,\"contributors\":null,\"is_quote_status\":false,\"quote_count\":0,\"reply_count\":0,\"retweet_count\":0,\"favorite_count\":0,\"entities\":{\"hashtags\":[{\"text\":\"testsweeps1\",\"indices\":[12,24]}],\"urls\":[],\"user_mentions\":[{\"screen_name\":\"fruggalapp\",\"name\":\"Fruggal\",\"id\":1254844223863558144,\"id_str\":\"1254844223863558144\",\"indices\":[0,11]}],\"symbols\":[]},\"favorited\":false,\"retweeted\":false,\"filter_level\":\"low\",\"lang\":\"en\",\"timestamp_ms\":\"1658632727235\"}','2022-07-23 21:18:52',NULL);
/*!40000 ALTER TABLE `harvested` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `incoming_tweets`
--

DROP TABLE IF EXISTS `incoming_tweets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `incoming_tweets` (
  `incoming_tweet_id` int unsigned NOT NULL AUTO_INCREMENT,
  `incoming_tweet_uid` varchar(50) NOT NULL,
  `tweet` longtext NOT NULL,
  `incoming_status` varchar(45) NOT NULL DEFAULT 'New',
  `processed_ind` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`incoming_tweet_id`,`incoming_tweet_uid`),
  UNIQUE KEY `id_UNIQUE` (`incoming_tweet_id`),
  UNIQUE KEY `incoming_tweet_uid_UNIQUE` (`incoming_tweet_uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `incoming_tweets`
--

LOCK TABLES `incoming_tweets` WRITE;
/*!40000 ALTER TABLE `incoming_tweets` DISABLE KEYS */;
/*!40000 ALTER TABLE `incoming_tweets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tweets`
--

DROP TABLE IF EXISTS `tweets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tweets` (
  `tweet_id` int unsigned NOT NULL AUTO_INCREMENT,
  `harvested_id` int unsigned NOT NULL,
  `user_id` int unsigned NOT NULL,
  `tweet_uid` varchar(50) NOT NULL,
  `external_id` varchar(100) NOT NULL,
  `body` text NOT NULL,
  `retweeted` bit(1) NOT NULL DEFAULT b'0',
  `external_created_at` datetime NOT NULL,
  `created_date` datetime NOT NULL,
  PRIMARY KEY (`tweet_id`),
  UNIQUE KEY `tweet_id_UNIQUE` (`tweet_id`),
  UNIQUE KEY `tweet_uid_UNIQUE` (`tweet_uid`),
  UNIQUE KEY `external_id_UNIQUE` (`external_id`),
  KEY `users_tweets_idx` (`user_id`),
  KEY `harvested_tweets_idx` (`harvested_id`),
  CONSTRAINT `harvested_tweets` FOREIGN KEY (`harvested_id`) REFERENCES `harvested` (`harvested_id`),
  CONSTRAINT `users_tweets` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=254 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tweets`
--

LOCK TABLES `tweets` WRITE;
/*!40000 ALTER TABLE `tweets` DISABLE KEYS */;
INSERT INTO `tweets` VALUES (223,238,224,'677f03c3-fbd5-4640-bc3a-528761d4390c','1546202615993835520','@fruggalapp Test9 #testsweeps1',_binary '\0','2022-07-10 18:40:05','2022-07-10 12:40:14'),(226,241,227,'af487114-b993-4986-ab80-74d9a88a186b','1546206065427189761','RT @fruggalapp: This is a test for #testsweeps1 https://t.co/qMshHM0RTZ',_binary '\0','2022-07-10 18:53:47','2022-07-10 12:53:51'),(228,243,229,'0da12a30-9b0a-43bd-8509-0ad3cbc94303','1546210396654096385','Test13 #testsweeps1',_binary '\0','2022-07-10 19:11:00','2022-07-10 13:11:04'),(229,244,224,'1de87f1b-7298-44ac-b555-4509c1702e08','1546210871713533952','@fruggalapp This is a test #testsweeps1',_binary '\0','2022-07-10 19:12:53','2022-07-10 13:12:57'),(230,245,224,'5a32744f-12a0-4209-96ba-b3b360e9f9a3','1546211892154159104','@fruggalapp This is another test #testsweeps1',_binary '\0','2022-07-10 19:16:56','2022-07-10 13:17:01'),(231,246,224,'0d28736f-45e9-42eb-b6ab-e320451322b9','1546212278462099456','@fruggalapp Testing another #testsweeps1',_binary '\0','2022-07-10 19:18:29','2022-07-10 13:18:33'),(232,247,224,'beba2c4c-467a-4095-9da7-94f378f47f36','1546212743824388096','@fruggalapp Another #testsweeps1',_binary '\0','2022-07-10 19:20:20','2022-07-10 13:20:23'),(233,248,224,'52aa1767-a322-4252-8f35-7dfde0a81b4c','1546214385793634305','@fruggalapp Another test #testsweeps1',_binary '\0','2022-07-10 19:26:51','2022-07-10 13:26:55'),(234,249,224,'c0b0dcc5-0b59-4cc5-8862-e6c03c7a6e5d','1546217470981054464','@fruggalapp Another test here #testsweeps1',_binary '\0','2022-07-10 19:39:07','2022-07-10 13:39:11'),(235,250,224,'f22112a8-5297-4abd-ad8c-cec5e8b8d9ae','1546217612912144384','@fruggalapp Testing again #testsweeps1',_binary '\0','2022-07-10 19:39:40','2022-07-10 13:39:44'),(236,251,224,'2e1f47ca-62b5-4a01-bf4c-e493a372125d','1546220764713537537','@fruggalapp Test 30 #testsweeps1',_binary '\0','2022-07-10 19:52:12','2022-07-10 13:52:20'),(238,253,239,'2e459355-842a-40b0-bff2-5620d12367eb','1546268877348229125','@fruggalapp This is test1 #testsweeps1 of not following Fruggal',_binary '\0','2022-07-10 23:03:23','2022-07-10 17:03:28'),(239,254,239,'68d3cb6f-2b42-40d9-b644-ac342f00fb20','1546269397618020353','@fruggalapp This is another test 2 #testsweeps1 of not following Fruggal',_binary '\0','2022-07-10 23:05:27','2022-07-10 17:05:32'),(240,255,241,'59aa2706-ee71-4b37-b46f-2c58e7899650','1546270111912173569','@fruggalapp Testing #testsweeps1',_binary '\0','2022-07-10 23:08:17','2022-07-10 17:08:22'),(241,256,241,'475780da-cffc-433b-90c7-64606ebff413','1546270278157627392','@fruggalapp #testsweeps1 testing',_binary '\0','2022-07-10 23:08:57','2022-07-10 17:09:02'),(242,257,243,'5661c902-82a5-4f9d-93dd-8739cc3723e7','1548321816816406531','@fruggalapp A test made here for #testsweeps1 1',_binary '\0','2022-07-16 15:01:02','2022-07-16 09:01:06'),(243,258,243,'d6d7053f-0f3c-4052-8b1e-6d9ec630e244','1548322027991212033','@fruggalapp Another testing #testsweeps1 2',_binary '\0','2022-07-16 15:01:52','2022-07-16 09:01:56'),(244,259,243,'c5b84748-32d3-461e-8471-b7884c3431a0','1548324903635390464','@fruggalapp Another test #testsweeps1 2',_binary '\0','2022-07-16 15:13:18','2022-07-16 09:13:22'),(245,260,243,'3a769a07-8447-4e95-be3a-da13fd4769fe','1548746848172756993','@fruggalapp Hi there #anotherhrvstertest 1',_binary '\0','2022-07-17 19:09:57','2022-07-17 13:10:03'),(246,261,243,'0e75173d-da51-49fe-a939-287c7ca87ace','1550922244943134731','@fruggalapp #anotherhrvstertest this is a test 1',_binary '\0','2022-07-23 19:14:12','2022-07-23 13:14:18'),(247,262,243,'2ac01708-e0cd-41d9-9a08-126b05af84fe','1550929489651171331','@fruggalapp #anotherhrvstertest this is test 2',_binary '\0','2022-07-23 19:42:59','2022-07-23 13:43:05'),(249,264,243,'871e1358-bcc3-435c-b00c-59ce5769999a','1550931769792385025','@fruggalapp #anotherhrvstertest this is test 4',_binary '\0','2022-07-23 19:52:03','2022-07-23 13:52:09'),(250,265,243,'0f95904d-98e4-4430-aa10-efe36f69f54d','1550932252430843911','@fruggalapp #anotherhrvstertest test 5',_binary '\0','2022-07-23 19:53:58','2022-07-23 13:54:04'),(251,266,243,'db627eee-d1db-4b33-b3dc-449922acb117','1550932374023798786','@fruggalapp #anotherhrvstertest test 6',_binary '\0','2022-07-23 19:54:27','2022-07-23 13:54:33'),(252,267,243,'a918e439-ac88-41a6-8366-5a0e4f0f696b','1551044155845083136','@fruggalapp #testsweeps1 testing the timeline graph 1',_binary '\0','2022-07-24 03:18:38','2022-07-23 21:18:44'),(253,268,243,'f5b1f02d-57f2-4b53-a80a-8d547a0f0c21','1551044192830459908','@fruggalapp #testsweeps1 testing the timeline graph 2',_binary '\0','2022-07-24 03:18:47','2022-07-23 21:18:52');
/*!40000 ALTER TABLE `tweets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `user_id` int unsigned NOT NULL AUTO_INCREMENT,
  `user_uid` varchar(50) DEFAULT NULL,
  `external_id` varchar(100) DEFAULT NULL,
  `profile_image_url` varchar(5000) DEFAULT NULL,
  `verified_ind` bit(1) DEFAULT b'0',
  `location` varchar(150) DEFAULT NULL,
  `description` varchar(500) DEFAULT NULL,
  `screen_name` varchar(100) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`),
  UNIQUE KEY `user_uid_UNIQUE` (`user_uid`),
  UNIQUE KEY `external_id_UNIQUE` (`external_id`)
) ENGINE=InnoDB AUTO_INCREMENT=255 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (224,'52c20ba2-a1d1-4f47-ae67-f5ef2a99ff59','19227042','https://pbs.twimg.com/profile_images/1519644986597249024/Yorv7EQb.jpg',_binary '\0','Canada','Husband to @shellyjo002.\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems \nI speak C#, PHP, ETL\n\nBuilding @hrvsterHQ','gamerwalt','2022-07-10 12:40:14','2022-07-10 13:52:20'),(227,'1c16420c-05e3-42a7-9aa9-8da0fe9d0181','2873509995','https://pbs.twimg.com/profile_images/1489107541904830464/gJRhQUu-.jpg',_binary '\0','Laguna, CA','Bussiness developer..logistic..Haulage..Music..religious and education activist..','tolakpe','2022-07-10 12:53:51',NULL),(229,'64efd8b7-e957-499a-98e6-f39d5d56a51c','1254844223863558144','https://pbs.twimg.com/profile_images/1344837609164472321/qiU8CxXg.jpg',_binary '\0','Canada, Nigeria, Philippines ','Joint Savings Made Simple!','fruggalapp','2022-07-10 13:11:04',NULL),(239,'0a94f924-4633-4861-a856-9ad5c13b0cc6','1485682131762401280','https://abs.twimg.com/sticky/default_profile_images/default_profile.png',_binary '\0',NULL,NULL,'WallyAdewoyin','2022-07-10 17:03:28','2022-07-10 17:05:32'),(241,'574db86a-0471-4049-b488-61739e2d027c','1498513306323013632','https://pbs.twimg.com/profile_images/1498514386880581637/lRQMZFpJ.jpg',_binary '\0',NULL,'Just living and sharing!','Mrssharlet002','2022-07-10 17:08:22','2022-07-10 17:09:02'),(243,'b06aebed-d136-4861-8525-679e2b13f3c9','1548124411529572353','https://abs.twimg.com/sticky/default_profile_images/default_profile.png',_binary '\0',NULL,'Customer support @hrvster','hellohrvster','2022-07-16 09:01:06','2022-07-23 21:18:52');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'promo.harvester'
--
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
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `account_create`(IN accountUid VARCHAR(50), IN emailAddress VARCHAR(500), IN phoneNumber VARCHAR(50), IN displayName VARCHAR(100))
BEGIN
	INSERT INTO accounts
    (
		account_uid,
        email_address,
        phone_number,
        display_name
    )
    VALUES
    (
		accountUid,
        emailAddress,
        phoneNumber,
        displayName
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `blacklisted_sweeps_users_get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `blacklisted_sweeps_users_get`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT *
    FROM blacklisted_sweeps_users
    WHERE sweeps_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `blacklisted_sweeps_user_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `blacklisted_sweeps_user_create`(IN externalUserId VARCHAR(100), IN sweepUid VARCHAR(50))
BEGIN
	INSERT INTO blacklisted_sweeps_users
    (
		sweeps_uid,
        tweet_user_id
    )
    VALUES
    (
		sweepUid,
        externalUserId
    );
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_create`(IN accountUid VARCHAR(50), IN campaignName VARCHAR(100), IN campaignUid VARCHAR(50), IN campaignTypeCode VARCHAR(100), IN activeInd BIT, IN startDate DATETIME, IN endDate DATETIME)
BEGIN
	DECLARE accountId INT DEFAULT 0;
    
	SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
    IF(@accountId = 0) THEN
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Account not found';
    END IF;
    
	INSERT INTO campaigns
    (
		account_id,
        campaign_name,
        campaign_uid,
        campaign_type_code,
        active_ind,
        start_date,
        end_date
    )
    VALUES
    (
		@accountId,
        campaignName,
        campaignUid,
        campaignTypeCode,
        activeInd,
        startDate,
        endDate
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `campaign_update` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `campaign_update`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(100), IN campaignTypeCode VARCHAR(100), IN activeInd BIT, IN startDate DateTime, IN endDate DateTime)
BEGIN
	UPDATE campaigns
		SET campaign_name = campaignName,
            start_date = startDate,
            end_date = endDate,
            active_ind = activeInd,
            campaign_type_code = campaignTypeCode
    WHERE campaign_uid = campaignUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `harvested_record_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `harvested_record_create`(IN harvestedUid VARCHAR(50), IN ruleUid VARCHAR(50), IN harvestedValue VARCHAR(8000))
BEGIN
	INSERT INTO harvested
    (
		harvested_uid,
        rule_uid,
        harvested_value,
        created_date
    )
    SELECT
		harvestedUid,
        ruleUid,
        harvestedValue,
        CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `incoming_tweets_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `incoming_tweets_create`(IN incomingTweetUid VARCHAR(50), IN tweet LONGTEXT, IN incomingStatus VARCHAR(50))
BEGIN
	INSERT INTO incoming_tweets
    (
		incoming_tweet_uid,
        tweet,
        incoming_status,
        processed_ind,
        created_date
    )
    VALUES
    (
		incomingTweetUid,
        tweet,
        incomingStatus,
        0,
        CURRENT_TIMESTAMP
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `incoming_tweets_get_by_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `incoming_tweets_get_by_status`(IN incomingStatus VARCHAR(50))
BEGIN
	SELECT 
		*
    FROM incoming_tweets
    WHERE incoming_status = incomingStatus;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `incoming_tweets_get_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `incoming_tweets_get_uid`(IN incomingTweetUid VARCHAR(50))
BEGIN
	SELECT *
    FROM incoming_tweets
    WHERE incoming_tweet_uid = incomingTweetUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `incoming_tweets_update_as_processed` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `incoming_tweets_update_as_processed`(IN incomingTweetUid VARCHAR(50), IN incomingStatus VARCHAR(50))
BEGIN
	UPDATE incoming_tweets
		SET processed_ind = 1,
			incoming_status = incomingStatus,
			updated_date = CURRENT_TIMESTAMP()
	WHERE incoming_tweet_uid = incomingTweetUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `incoming_tweets_update_status` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `incoming_tweets_update_status`(IN incomingTweetUid VARCHAR(50), IN incomingStatus VARCHAR(50))
BEGIN
	UPDATE incoming_tweets
		SET incoming_status = incomingStatus,
			updated_date = CURRENT_TIMESTAMP()
	WHERE incoming_tweet_uid = incomingTweetUid;
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
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sweep_create`(IN sweepUid VARCHAR(50), IN campaignUid VARCHAR(50), IN accountUid VARCHAR(50), IN sweepName VARCHAR(100), IN rulesUrl VARCHAR(1000), IN startDate DATETIME, IN endDate DATETIME, IN drawDate DATETIME)
BEGIN
	DECLARE campaignId INT DEFAULT 0;
    DECLARE accountId INT DEFAULT 0;

    SELECT @campaignId := campaign_id FROM campaigns WHERE campaign_uid = campaignUid;
    IF(@campaignId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Campaign not found';
	END IF;
    
    SELECT @accountId := account_id FROM accounts WHERE account_uid = accountUid;
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
        rules_url,
        start_date,
        end_date,
        draw_date
    )
    SELECT
		sweepUid,
        @campaignId,
        @accountId,
        sweepName,
        rulesUrl,
        startDate,
        endDate,
        drawDate;
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
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_create`(IN sweepUid VARCHAR(50), IN prizeUid VARCHAR(50), IN prizePosition INT, IN enabledInd BIT, IN prizeName VARCHAR(100), IN prizeValue DECIMAL(18, 2))
BEGIN
	INSERT INTO sweep_prizes
    (
		prize_uid,
        prize_position,
        sweep_uid,
        enabled_ind,
        prize_name,
        prize_value,
        created_date
    )
    VALUES 
    (
		prizeUid,
        prizePosition,
        sweepUid,
        enabledInd,
        prizeName,
        prizeValue,
        UTC_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prizes_get_by_prize_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_get_by_prize_uid`(IN prizeUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_prizes
    WHERE prize_uid = prizeUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_prizes_get_by_sweep_uid` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `sweep_prizes_get_by_sweep_uid`(IN sweepUid VARCHAR(50))
BEGIN
	SELECT *
    FROM sweep_prizes
    WHERE sweep_uid = sweepUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `tweets_get_harvested` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`hrvster_user`@`%` PROCEDURE `tweets_get_harvested`(IN ruleUid VARCHAR(50))
BEGIN
	SELECT
		H.harvested_id,
        H.rule_uid,
        H.harvested_uid,
        T.tweet_id,
        T.tweet_uid,
        T.external_id as tweet_external_id,
        T.body as tweet_text,
        T.retweeted,
        U.user_id,
        U.user_uid,
        U.external_id as user_external_id,
        U.profile_image_url,
        U.verified_ind,
        U.location,
        U.description,
        U.screen_name
    FROM harvested as H
    INNER JOIN tweets as T ON H.harvested_id = T.harvested_id
    INNER JOIN users as U ON U.user_id = T.user_id
    WHERE H.rule_uid = ruleUid;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `tweet_create` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `tweet_create`(IN tweetUid VARCHAR(50), IN harvestedUid VARCHAR(50), IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN body TEXT, IN retweeted BIT, IN externalCreatedAt DATETIME)
BEGIN
   
	DECLARE harvestedId INT DEFAULT 0;
    DECLARE userId INT DEFAULT 0;
    
    SELECT @harvestedId := harvested_id FROM harvested WHERE harvested_uid = harvestedUid;
    SELECT @userId := user_id FROM users  WHERE user_uid = userUid;
    
    IF(@harvestedId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Harvested data not found';
	END IF;
    IF(@userId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'User not found';
	END IF;
    
    INSERT INTO tweets
    (
		harvested_id,
        user_id,
        tweet_uid,
        external_id,
        body,
        retweeted,
        external_created_at,
        created_date
    )
    VALUES
    (
		@harvestedId,
        @userId,
        tweetUid,
        externalId,
        body,
        retweeted,
        externalCreatedAt,
        CURRENT_TIMESTAMP()
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `tweet_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `tweet_upsert`(IN tweetUid VARCHAR(50), IN harvestedUid VARCHAR(50), IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN body TEXT, IN retweeted BIT, IN externalCreatedAt DATETIME)
BEGIN
	
	DECLARE harvestedId INT DEFAULT 0;
    DECLARE userId INT DEFAULT 0;
    
    SELECT @harvestedId := harvested_id FROM harvested WHERE harvested_uid = harvestedUid;
    SELECT @userId := user_id FROM users  WHERE user_uid = userUid;
    
    IF(@harvestedId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'Harvested data not found';
	END IF;
    IF(@userId = 0) THEN 
		SIGNAL SQLSTATE '45000'
			SET MESSAGE_TEXT = 'User not found';
	END IF;
    
    INSERT IGNORE  INTO tweets
    (
		harvested_id,
        user_id,
        tweet_uid,
        external_id,
        body,
        retweeted,
        external_created_at,
        created_date
    )
    VALUES
    (
		@harvestedId,
        @userId,
        tweetUid,
        externalId,
        body,
        retweeted,
        externalCreatedAt,
        CURRENT_TIMESTAMP()
    );
		
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `tweet_user_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `tweet_user_upsert`(IN userUid VARCHAR(50), IN externalId VARCHAR(100), IN profileImageUrl VARCHAR(5000), IN verifiedInd BIT, IN location VARCHAR(150), IN description VARCHAR(500), IN screenName VARCHAR(100))
BEGIN
	INSERT INTO users
    (
		user_uid,
        external_id,
        profile_image_url,
        verified_ind,
        location,
        description,
        screen_name,
        created_date
    )
    VALUES
    (
		userUid,
        externalId,
        profileImageUrl,
        verifiedInd,
        location,
        description,
        screenName,
        CURRENT_TIMESTAMP()
    )
    ON DUPLICATE KEY UPDATE
	  user_uid = userUid,
	  profile_image_url = profileImageUrl,
      verified_ind = verifiedInd,
      location = location,
      description = description,
      screen_name = screenName,
      updated_date = CURRENT_TIMESTAMP();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `users_get_by_screen_name` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `users_get_by_screen_name`(IN userIdentifier VARCHAR(100))
BEGIN
	SELECT *
    FROM users
    WHERE screen_name = userIdentifier;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Current Database: `promo.reporting`
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `promo.reporting` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `promo.reporting`;

--
-- Table structure for table `sweep_entries`
--

DROP TABLE IF EXISTS `sweep_entries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sweep_entries` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `sweep_uid` varchar(50) NOT NULL,
  `campaign_uid` varchar(50) NOT NULL,
  `screen_name` varchar(100) NOT NULL,
  `user_identifier_type` varchar(100) NOT NULL,
  `external_id` varchar(100) NOT NULL,
  `external_user_id` varchar(100) NOT NULL,
  `entries` int NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `record_unique` (`campaign_uid`,`sweep_uid`,`external_user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=125 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entries`
--

LOCK TABLES `sweep_entries` WRITE;
/*!40000 ALTER TABLE `sweep_entries` DISABLE KEYS */;
INSERT INTO `sweep_entries` VALUES (17,'f65398ff-58b2-4bfc-9b3e-d13fd3098233','e23436f6-398a-4aea-8505-155d7df5cf84','885d03c9-884c-406e-8c23-fe7420b26044','fruggalapp','TwitterHandle','1479520742781947916','1254844223863558144',1,'2022-01-07 12:30:17',NULL),(18,'0ff207c9-d67b-454f-9d7b-b2977f1b2b18','e23436f6-398a-4aea-8505-155d7df5cf84','885d03c9-884c-406e-8c23-fe7420b26044','mazimario','TwitterHandle','1479520811748843521','138024915',1,'2022-01-07 12:30:30',NULL),(19,'a9b24f47-1ae6-4782-806a-feca725b666a','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','fruggalapp','TwitterHandle','1479691754131578881','1254844223863558144',7,'2022-01-07 23:49:49','2022-01-26 08:18:45'),(20,'9b0e11b0-2a53-4236-a28c-6b13d0550a1e','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','gamerwalt','TwitterHandle','1485119067866337281','19227042',3,'2022-01-22 23:17:47','2022-01-24 07:31:46'),(25,'9f3e5401-4ad7-4916-81bf-e4b17fffb0b3','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','MissCocoa_','TwitterHandle','1485644824581746697','1389302028363567105',2,'2022-01-24 10:05:11','2022-01-25 17:01:05'),(26,'0dc4e08b-906c-44a9-8b78-f37cd8f58d5a','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','bolawole','TwitterHandle','1485662444680892417','22257888',2,'2022-01-24 11:15:10','2022-01-24 11:15:12'),(28,'49916c88-1a5e-4ca7-a4b2-39a83cfafc60','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','Douglas06519127','TwitterHandle','1485663620918915074','1354190287892078609',5,'2022-01-24 11:19:50','2022-01-25 13:39:12'),(31,'0b21dca2-184e-430a-b25c-07666ef5270c','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','TinuolaOderinde','TwitterHandle','1485678909639254017','1560105944',1,'2022-01-24 12:20:35',NULL),(32,'87262dd1-1616-4811-bbd9-0d72ad9fd3a7','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','WallyAdewoyin','TwitterHandle','1485682791438241800','1485682131762401280',9,'2022-01-24 12:36:01','2022-01-26 22:43:35'),(38,'024859b1-e2ce-4b30-a4fd-3972b4cdd68a','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','thevalue_team','TwitterHandle','1485693469523947522','1412699277206298626',1,'2022-01-24 13:18:26',NULL),(39,'b917d25c-33b8-449f-882b-bc3a2f9c4caa','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','Joshibirogba','TwitterHandle','1485699297521799169','902528827233931265',1,'2022-01-24 13:41:36',NULL),(40,'abb95957-3fc3-4f95-8ac5-916941a1c1ca','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','awsazurelinux','TwitterHandle','1485701600274440194','627489052',10,'2022-01-24 13:50:45','2022-01-25 02:44:53'),(41,'9c92a468-28e3-4a38-b35a-40d53ab0146f','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','PeaceSandy3','TwitterHandle','1485703910186790918','1273899606770954241',2,'2022-01-24 13:59:55','2022-01-24 18:28:37'),(43,'ba44408f-4008-48f7-b6be-aaaecbb60cd2','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','SamKlapton','TwitterHandle','1485706996607246338','221316880',1,'2022-01-24 14:12:12',NULL),(45,'f01488a9-3fca-45f3-8629-97a218639113','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','merryster1','TwitterHandle','1485724613950980097','1133287002974183424',1,'2022-01-24 15:22:12',NULL),(46,'c6844ce6-1163-4788-98f7-5e95bb5ae2c4','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','OderindeTinuola','TwitterHandle','1485735217084129285','1460901764446756864',1,'2022-01-24 16:04:19',NULL),(47,'9eab68a0-a09e-490d-b28e-91d56a7787f3','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','Mayree187','TwitterHandle','1485763990630191104','1450441614485245955',1,'2022-01-24 17:58:40',NULL),(51,'7d14148b-363e-412f-8be8-e3b2790cc4d0','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','Handmaids38','TwitterHandle','1485868990102708225','966407608243539968',2,'2022-01-25 00:55:54','2022-01-25 00:55:56'),(70,'9efe7405-4c18-48d1-8932-b36adc9fbda4','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','bolajiogidan','TwitterHandle','1485912776027561985','383352099',1,'2022-01-25 03:49:53',NULL),(71,'085cdde8-36ac-4120-8933-aa919b2bdf89','25be8314-9170-46a9-9cab-5ec311109282','34d03f31-3d9a-4fa4-a641-a023e1df5f17','gamerwalt','TwitterHandle','1485964900232957962','19227042',2,'2022-01-25 07:17:02','2022-01-27 10:29:25'),(72,'18118e66-9984-4822-8070-3d5ef69dfb24','25be8314-9170-46a9-9cab-5ec311109282','34d03f31-3d9a-4fa4-a641-a023e1df5f17','Noma_O','TwitterHandle','1485993644242911232','28674620',1,'2022-01-25 09:11:14',NULL),(73,'55a83205-1240-4be4-aaec-98360560d4c1','25be8314-9170-46a9-9cab-5ec311109282','34d03f31-3d9a-4fa4-a641-a023e1df5f17','mazimario','TwitterHandle','1485996264256950275','138024915',1,'2022-01-25 09:21:38',NULL),(74,'89e4fb5c-58af-49b6-85b5-3dd49f8731e9','25be8314-9170-46a9-9cab-5ec311109282','34d03f31-3d9a-4fa4-a641-a023e1df5f17','hrvsterHQ','TwitterHandle','1486046456905703430','1476226447916163155',1,'2022-01-25 12:41:05',NULL),(75,'0d9de757-ac4a-4367-8e8a-d0b98e63716b','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','benud77','TwitterHandle','1486051067683123201','477907799',7,'2022-01-25 12:59:24','2022-01-25 13:16:30'),(77,'299ef914-df52-48aa-ba65-34ecc36ba2a9','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','adeyemi_ifeta','TwitterHandle','1486053042399109126','112185547',1,'2022-01-25 13:07:15',NULL),(82,'c81ab6f6-a843-4b83-8f49-c2b2252c4156','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','popoolababs','TwitterHandle','1486054947233968134','245474340',1,'2022-01-25 13:14:49',NULL),(86,'0b9371ed-0448-4642-8919-9d837ce123d4','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','OmaNnyen','TwitterHandle','1486066343824793610','1305267911943229440',1,'2022-01-25 14:00:06',NULL),(87,'27a774fa-d054-46cb-bb6d-86510665cc42','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','AbdulkoHakeem','TwitterHandle','1486066603347386377','712789766',10,'2022-01-25 14:01:08','2022-01-25 15:21:29'),(93,'cd18d227-748b-475e-828a-c634cb59df8f','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','RotimiLewis','TwitterHandle','1486072493416792067','560364377',1,'2022-01-25 14:24:33',NULL),(98,'0c286cdf-4eea-448f-a747-7fdc70ae72d8','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','herdeleke1','TwitterHandle','1486099803306090498','355399881',1,'2022-01-25 16:13:03',NULL),(102,'820c7d3d-db48-497a-9227-8284dbfe7dfb','df90f3e2-ca9c-4af2-aa3f-d449633865a4','5445e3b1-ed83-414f-9c3e-51f8f518d434','sexyanny12','TwitterHandle','1486310140974407682','248794801',4,'2022-01-26 06:08:52','2022-01-26 06:08:58'),(107,'03f7d11d-f591-46f2-826a-8e1cf199bac1','25be8314-9170-46a9-9cab-5ec311109282','34d03f31-3d9a-4fa4-a641-a023e1df5f17','oluwafemioyefes','TwitterHandle','1486385087679762436','575746818',1,'2022-01-26 11:06:41',NULL),(110,'5386abd4-cee4-40f8-9358-2041cb128e2c','0ce3d60e-99ee-4558-8551-cbb285043f3f','1cb1c35a-53e1-4e81-bf01-6a1d6cbdf397','fruggalapp','TwitterHandle','1546210396654096385','1254844223863558144',1,'2022-07-10 13:11:10',NULL),(111,'5efd1e62-3ea3-48cc-b4e2-373f8c9bfdf7','0ce3d60e-99ee-4558-8551-cbb285043f3f','1cb1c35a-53e1-4e81-bf01-6a1d6cbdf397','gamerwalt','TwitterHandle','1546210871713533952','19227042',6,'2022-07-10 13:12:57','2022-07-10 13:55:07'),(117,'5eb5bed0-33a5-469c-b30b-70b9a74deb65','0ce3d60e-99ee-4558-8551-cbb285043f3f','1cb1c35a-53e1-4e81-bf01-6a1d6cbdf397','Mrssharlet002','TwitterHandle','1546270278157627392','1498513306323013632',1,'2022-07-10 17:09:02',NULL),(118,'bd4729ac-a96f-4ebb-a425-ade608ad7eb7','0ce3d60e-99ee-4558-8551-cbb285043f3f','1cb1c35a-53e1-4e81-bf01-6a1d6cbdf397','hellohrvster','TwitterHandle','1548322027991212033','1548124411529572353',4,'2022-07-16 09:01:56','2022-07-23 21:18:52'),(120,'884e8c5a-27ed-4864-a19c-7add4771b007','e190c342-a225-4039-9b03-123f141e3ec3','1cb1c35a-53e1-4e81-bf01-6a1d6cbdf397','hellohrvster','TwitterHandle','1550931769792385025','1548124411529572353',3,'2022-07-23 13:52:09','2022-07-23 13:54:33');
/*!40000 ALTER TABLE `sweep_entries` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'promo.reporting'
--
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
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `sweep_entry_upsert` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
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

-- Dump completed on 2022-09-11  8:38:41
