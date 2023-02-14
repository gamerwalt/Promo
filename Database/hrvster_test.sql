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
) ENGINE=InnoDB AUTO_INCREMENT=84 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_profiles`
--

LOCK TABLES `account_profiles` WRITE;
/*!40000 ALTER TABLE `account_profiles` DISABLE KEYS */;
INSERT INTO `account_profiles` VALUES (19,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','1254844223863558144',2,1,'Fruggal','fruggalapp','Joint Savings Made Simple!','1254844223863558144-7VscfIP1gd9NMhiBCUHfSBWn0klPoE','X9t1AyFg0VNHzSKYXd0nBRvNWz9wwHaP07uv8x7zY8ZZa','a8QkT2vtL9uFITULH43AlrMsK','1JQLFgrSqyOg7uB1hLuSe9lSyGxhLmYP38AHpk5FIWd0cgg7LL','Canada, Nigeria, Philippines ','https://pbs.twimg.com/profile_images/1344837609164472321/qiU8CxXg_400x400.jpg','2022-06-05 17:51:54','2022-08-17 16:01:26'),(80,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','19227042',1,1,'Wale ?? ?? ??','gamerwalt','Husband to @shellyjo002.\nConsultant @ Online Business Systems. Azure Cloud Dev, Databricks Dev, Distributed Systems. Speak C#, PHP, ETL\n\nBuilding @hrvsterHQ','19227042-SQJ66qYUDhrrb0oopOTL4lxbLOhjaifvQYpXvAg2D','QjcG91jqvIKTUJF9gjKAqNTOzoBDJXoruUctS8Dv1cAp6','a8QkT2vtL9uFITULH43AlrMsK','1JQLFgrSqyOg7uB1hLuSe9lSyGxhLmYP38AHpk5FIWd0cgg7LL','Canada','https://pbs.twimg.com/profile_images/1519644986597249024/Yorv7EQb_400x400.jpg','2022-09-20 15:29:42',NULL),(82,'791b17ee-e459-46ec-bc00-f3042011cc09','1254844223863558144',7,1,'Fruggal','fruggalapp','Joint Savings Made Simple!','1254844223863558144-7VscfIP1gd9NMhiBCUHfSBWn0klPoE','X9t1AyFg0VNHzSKYXd0nBRvNWz9wwHaP07uv8x7zY8ZZa','a8QkT2vtL9uFITULH43AlrMsK','1JQLFgrSqyOg7uB1hLuSe9lSyGxhLmYP38AHpk5FIWd0cgg7LL','Canada, Nigeria, Philippines ','https://pbs.twimg.com/profile_images/1344837609164472321/qiU8CxXg_400x400.jpg','2022-11-06 12:31:26',NULL),(83,'f6a2b116-4781-4447-b5c3-bfdd361dff34','1591846815934156802',8,1,'tester1 hrvster','hrvster1','Tester 1 for @hrvsterHQ','1591846815934156802-I6sBD3i9WqvOQkjfaRJVrwudeMwHKB','4imsRDLRJkN3FNrYOkaPpOXVvKGMOu6xubfllY1jKc4mv','a8QkT2vtL9uFITULH43AlrMsK','1JQLFgrSqyOg7uB1hLuSe9lSyGxhLmYP38AHpk5FIWd0cgg7LL','','https://pbs.twimg.com/profile_images/1591846863006736387/YrMD2ALU_400x400.png','2022-12-12 23:38:17',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_subscriptions`
--

LOCK TABLES `account_subscriptions` WRITE;
/*!40000 ALTER TABLE `account_subscriptions` DISABLE KEYS */;
INSERT INTO `account_subscriptions` VALUES (1,'bddf3bfc-6ff0-44df-b9dc-9b4c7ed8bf0b','PendingPayment',2,1,'2022-12-01 00:00:00','2022-12-01 00:00:00','2023-12-31 00:00:00','2023-12-31 00:00:00',0,0,'2022-12-29 00:00:00',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_update_history`
--

LOCK TABLES `account_update_history` WRITE;
/*!40000 ALTER TABLE `account_update_history` DISABLE KEYS */;
INSERT INTO `account_update_history` VALUES (9,'c1c890ee-fa2d-4d59-b35a-6986a0cfe582',2,'gamerwalt@yahoo.com','gamerwalt@hotmail.com','2022-09-20 00:33:33'),(10,'81b8a7e3-d159-4a93-82e1-d3e5997f0ec8',2,'gamerwalt@hotmail.com','gamerwalt@yahoo.com','2022-09-20 00:35:51'),(11,'669d7d4e-6b88-464b-9ebc-354d243a7a79',2,'gamerwalt@yahoo.com','gamerwalt@hotmail.com','2022-09-20 00:37:10'),(12,'f043dc62-e9b1-4bb8-8480-fb871048cacc',2,'+12048917675','+12048917676','2022-09-20 07:54:05'),(13,'98fa687f-70cd-41a5-b684-5759fb516f6b',2,'gamerwalt@hotmail.com','gamerwalt@hotmails.com','2023-01-02 12:12:33'),(14,'5cd2201a-f876-418f-8c49-48cb1c2afc4e',2,'+12048917676','+12048917672','2023-01-02 12:12:33'),(15,'ab06f7f6-d2c9-47b3-b90f-f1c3422318f3',2,'gamerwalt@hotmails.com','gamerwalt@hotmail.com','2023-01-02 12:14:28'),(16,'b6089d6f-5993-4877-8a98-11ed2401e42f',2,'+12048917672','+12048917676','2023-01-02 12:14:28'),(17,'f6633558-4162-40df-b458-e5f5aa6641f7',2,'gamerwalt@hotmail.com','gamerwalt@hotmails.com','2023-01-02 12:14:48'),(18,'53746c81-eb80-4d45-945d-17c3110971f1',2,'+12048917676','+12048917672','2023-01-02 12:14:48'),(19,'4bd8f3a5-a81a-4694-aad5-f6bc1c8b5c68',2,'gamerwalt@hotmails.com','gamerwalt@hotmail.com','2023-01-02 12:15:05'),(20,'32eb74cd-ccbd-4558-9b16-bfd575bfa2b2',2,'+12048917672','+12048917676','2023-01-02 12:15:05');
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
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','gamerwalt@gmail.com','+12048917575','gamerwaltgmail',NULL,NULL,NULL,_binary '',_binary '\0','123456','gamerwalt',_binary '','Nigeria','2023-02-12 12:55:00','2023-02-12 11:55:00','2022-02-26 17:57:49','2023-02-12 12:55:00'),(2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','gamerwalt@hotmail.com','+12048917676','gamerwalthotmail','Olawale','Adewoyin',NULL,_binary '',_binary '\0','123456','gamerwalt',_binary '','Nigeria','2023-02-12 19:37:47','2023-02-12 18:37:47','2022-02-27 13:15:47','2023-02-12 19:37:47'),(4,'cb940d4e-f3e0-4213-aa17-b11901f36244','example1@gmail.com','+12045093528','example',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=kKtcHXB86-CRt7t9as2qrG5QOliR4KaW84geBiKOJxAAAAGCDS3saw&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '\0',_binary '\0','123456',NULL,_binary '\0','Nigeria',NULL,'2022-12-19 08:21:03','2022-07-17 17:21:30',NULL),(5,'431006b3-c59e-43e7-8566-6360be87b79a','tester@tester1.com','+12048857748','tester1',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=r4xqj1Uqg6n-uOidMU8jf47uY68dwLjCZP8eX1mFaPIAAAGCUXxHHA&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '\0',_binary '\0','123456',NULL,_binary '','Nigeria',NULL,'2022-12-19 08:21:03','2022-07-30 23:41:14',NULL),(6,'a277862e-501d-4519-bc5c-6eabe193e0c9','tester@hrvster.com','+12048927675','tester',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=rRKR-mAm3IcPqJj78olXuz7fUbO4a2GBYUTtXWbv4ekAAAGETZkyxQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '\0',_binary '\0','123456',NULL,_binary '','Nigeria',NULL,'2022-12-19 08:21:03','2022-11-06 15:39:55',NULL),(7,'791b17ee-e459-46ec-bc00-f3042011cc09','admin@hrvster.com','+12048937675','Admin',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=JYBB6bcEgLNavqLBNy3JZjiPTwIpqfgUXUT9zDkhs4cAAAGETZpBGg&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '',_binary '\0','123456',NULL,_binary '','Nigeria',NULL,'2022-12-19 08:21:03','2022-11-06 15:41:04','2022-11-06 22:48:24'),(8,'f6a2b116-4781-4447-b5c3-bfdd361dff34','hrvstertester1@gmail.com','+12045052255','hrvstertester1',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=1Ae-PCYG5v_Zk25wFXYa67qyNXPGGkV_G62N8-SFqrgAAAGFCfpgRg&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '',_binary '\0','123456',NULL,_binary '','Nigeria','2022-12-13 05:36:20','2022-12-19 08:21:03','2022-12-13 05:34:41','2022-12-17 12:06:53'),(9,'422a5552-5451-4fe0-9c92-6f36d769979e','hrvstertester2@gmail.com','+12045523358','hrvstertester2',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=SLR8Clvk5d5qQgZyvjkpe-0Erf7wGCcAQwOszlBEHGkAAAGFH_mCTQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '',_binary '\0','123456',NULL,_binary '','Nigeria','2022-12-17 13:52:58','2022-12-19 08:21:03','2022-12-17 12:05:24','2022-12-17 13:49:18'),(10,'5e3fe7d9-f012-474f-b3f0-4418ea00351e','hrvstertester3@gmail.com','+12048896631','hrvstertester3',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=oJ3UezGfuGQuc6IBDVAMyqDQfms64dcnSJxnGC2mBLUAAAGFH_oeFQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '',_binary '\0','123456',NULL,_binary '','Nigeria',NULL,'2022-12-19 08:21:03','2022-12-17 12:06:04','2022-12-17 13:49:18'),(11,'6b815b8c-a9ea-4c09-b4ea-61ad86490101','hrvstertester4@gmail.com','+12052245599','hrvstertester4',NULL,NULL,'https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=zqac3WS5PjsR8jVMCBbSARja3uSW-NZ8RLa8etjlroEAAAGFH_qaGQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en',_binary '',_binary '\0','123456',NULL,_binary '','Nigeria',NULL,'2022-12-19 08:21:03','2022-12-17 12:06:36','2022-12-17 13:49:18');
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
) ENGINE=InnoDB AUTO_INCREMENT=87 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `campaigns`
--

LOCK TABLES `campaigns` WRITE;
/*!40000 ALTER TABLE `campaigns` DISABLE KEYS */;
INSERT INTO `campaigns` VALUES (83,2,15,'d346f403-5f7e-4571-8ab4-25fc66e01b9b','Promo 1','Promo 1',1,'2022-12-23 00:00:00','2022-12-23 05:00:00','2023-10-30 23:59:00','2023-10-31 04:59:00','2022-12-20 10:47:39','2023-01-31 22:35:42'),(84,2,15,'54e6c83a-ec04-4699-b2b9-5a5d8dfad8d5','Promo 2','Promo 2',1,'2022-12-23 00:00:44','2022-12-23 06:00:44','2022-12-27 23:59:44','2022-12-28 05:59:44','2022-12-21 17:34:48',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `emails`
--

LOCK TABLES `emails` WRITE;
/*!40000 ALTER TABLE `emails` DISABLE KEYS */;
INSERT INTO `emails` VALUES (26,'0fe2d9cc-46a1-4922-bbb1-686b4b6915bd','gamerwalt@yahoo.com','{\"DisplayName\":\"gamerwalthotmails\",\"OldEmailAddress\":\"gamerwalt@yahoo.com\",\"NewEmailAddress\":\"gamerwalt@yahoo.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2022-09-20 00:33:33','2023-01-21 07:34:58'),(27,'26faa880-8712-432b-98a5-39a12bcd049e','gamerwalt@hotmail.com','{\"DisplayName\":\"gamerwalthotmails\",\"OldEmailAddress\":\"gamerwalt@hotmail.com\",\"NewEmailAddress\":\"gamerwalt@yahoo.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2022-09-20 00:35:51','2023-01-21 07:34:58'),(28,'bf4acc68-1ab9-4e9f-b219-4addde620813','gamerwalt@yahoo.com','{\"DisplayName\":\"gamerwalthotmails\",\"OldEmailAddress\":\"gamerwalt@yahoo.com\",\"NewEmailAddress\":\"gamerwalt@hotmail.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2022-09-20 00:37:10','2023-01-21 07:34:58'),(29,'83395386-8fdf-4692-bf20-21f6049a000f','tester@hrvster.com','{\"DisplayName\":\"tester\",\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=rRKR-mAm3IcPqJj78olXuz7fUbO4a2GBYUTtXWbv4ekAAAGETZkyxQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-11-06 09:39:55','2023-01-21 07:34:58'),(30,'d3ab7912-d0c7-4517-b318-d2a6245f4ade','admin@hrvster.com','{\"DisplayName\":\"Admin\",\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=JYBB6bcEgLNavqLBNy3JZjiPTwIpqfgUXUT9zDkhs4cAAAGETZpBGg&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-11-06 09:41:04','2023-01-21 07:34:58'),(31,'f8b069d3-5593-4b20-9f0f-50bb4ad778a0','admin@hrvster.com','{\"DisplayName\":\"Admin\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','WelcomeEmail','Sent',_binary '','2022-11-06 16:48:24','2023-01-21 07:34:58'),(32,'6fce8fc3-d47d-4df3-9021-58c6d9f8a501','hrvstertester1@gmail.com','{\"DisplayName\":\"hrvstertester1\",\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=1Ae-PCYG5v_Zk25wFXYa67qyNXPGGkV_G62N8-SFqrgAAAGFCfpgRg&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-12-12 23:34:41','2023-01-21 07:34:58'),(33,'1d41acba-c5d6-4372-972b-472c2e180704','hrvstertester2@gmail.com','{\"DisplayName\":\"hrvstertester2\",\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=SLR8Clvk5d5qQgZyvjkpe-0Erf7wGCcAQwOszlBEHGkAAAGFH_mCTQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-12-17 06:05:24','2023-01-21 07:34:58'),(34,'3bbde1f7-eefa-4755-bcbe-5382eb33bb41','hrvstertester3@gmail.com','{\"DisplayName\":\"hrvstertester3\",\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=oJ3UezGfuGQuc6IBDVAMyqDQfms64dcnSJxnGC2mBLUAAAGFH_oeFQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-12-17 06:06:04','2023-01-21 07:34:58'),(35,'444b2e0f-1468-43b4-9cad-3d3504bc93f8','hrvstertester4@gmail.com','{\"DisplayName\":\"hrvstertester4\",\"DashboardLink\":\"https://app.hrvster/dashboard\",\"VerificationLink\":\"https://promo-22f6f.firebaseapp.com/__/auth/action?mode=verifyEmail&oobCode=zqac3WS5PjsR8jVMCBbSARja3uSW-NZ8RLa8etjlroEAAAGFH_qaGQ&apiKey=AIzaSyClYnPYWyVb43gV9lOsZpBy1eZ7ea4D_AU&lang=en\"}','VerificationEmail','Sent',_binary '','2022-12-17 06:06:36','2023-01-21 07:34:58'),(36,'702b05ab-ce39-4143-9a39-8bdd7b9f11d5','hrvstertester1@gmail.com','{\"DisplayName\":\"hrvstertester1\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','WelcomeEmail','Sent',_binary '','2022-12-17 06:06:53','2023-01-21 07:34:58'),(37,'9e55a1f7-f3eb-4f18-8b1b-9769078b388b','hrvstertester2@gmail.com','{\"DisplayName\":\"hrvstertester2\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','WelcomeEmail','Sent',_binary '','2022-12-17 07:49:18','2023-01-21 07:34:58'),(38,'9b8a194d-ba42-4f6d-b68a-88cf9308dd24','hrvstertester3@gmail.com','{\"DisplayName\":\"hrvstertester3\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','WelcomeEmail','Sent',_binary '','2022-12-17 07:49:18','2023-01-21 07:34:58'),(39,'e782107a-96f0-4434-a1a5-982b1531afc5','hrvstertester4@gmail.com','{\"DisplayName\":\"hrvstertester4\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','WelcomeEmail','Sent',_binary '','2022-12-17 07:49:18','2023-01-21 07:34:58'),(40,'fbc1605c-0496-4270-b686-48bee4acd3da','gamerwalt@hotmail.com','{\"DisplayName\":\"gamerwalthotmail\",\"OldEmailAddress\":\"gamerwalt@hotmail.com\",\"NewEmailAddress\":\"gamerwalt@hotmails.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2023-01-02 12:12:33','2023-01-21 07:34:58'),(41,'b2704daf-07d2-43e6-8809-5520c872b8ed','gamerwalt@hotmails.com','{\"DisplayName\":\"gamerwalthotmail\",\"OldEmailAddress\":\"gamerwalt@hotmails.com\",\"NewEmailAddress\":\"gamerwalt@hotmail.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2023-01-02 12:14:28','2023-01-21 07:34:58'),(42,'b6bf5b6c-6227-4e02-8090-908efa5337e5','gamerwalt@hotmail.com','{\"DisplayName\":\"gamerwalthotmail\",\"OldEmailAddress\":\"gamerwalt@hotmail.com\",\"NewEmailAddress\":\"gamerwalt@hotmails.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2023-01-02 12:14:48','2023-01-21 07:34:58'),(43,'99577974-d3f7-48ff-895e-6a96121be720','gamerwalt@hotmails.com','{\"DisplayName\":\"gamerwalthotmail\",\"OldEmailAddress\":\"gamerwalt@hotmails.com\",\"NewEmailAddress\":\"gamerwalt@hotmail.com\",\"DashboardLink\":\"https://localhost:5001/Dashboard\",\"SupportLink\":\"https://support.hrvster.com/\"}','EmailAddressUpdateRequest','Sent',_binary '','2023-01-02 12:15:05','2023-01-21 07:34:58'),(46,'f6f75326-48d8-4c62-ae4a-ab4ce9b2e422','gamerwalt@hotmail.com','{\"EmailAddress\":\"gamerwalt@hotmail.com\",\"SweepstakeName\":\"Promo 1-Sweepstakes\",\"StartDate\":\"December 24, 2022 12:00 PM\",\"EndDate\":\"January 15, 2023 11:59 AM\",\"HourValue\":\"1\",\"DisplayName\":\"gamerwalthotmail\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','SweepsGoingLiveSoon','Sent',_binary '','2023-01-03 23:35:33','2023-01-21 07:34:58'),(47,'7d3ff731-d3ba-4152-99ab-457c05a984f3','gamerwalt@hotmail.com','{\"EmailAddress\":\"gamerwalt@hotmail.com\",\"DisplayName\":\"gamerwalthotmail\",\"SweepstakeName\":\"Promo 1-Sweepstakes\",\"StartDate\":\"December 24, 2022 12:00 PM\",\"EndDate\":\"January 20, 2023 11:59 PM\",\"DrawDate\":\"January 21, 2023 2:10 PM\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','DrawCompleted','Sent',_binary '','2023-01-21 07:28:55','2023-01-21 07:34:58'),(48,'89634ff0-f728-41e7-9f87-717db19859b9','gamerwalt@hotmail.com','{\"EmailAddress\":\"gamerwalt@hotmail.com\",\"DisplayName\":\"gamerwalthotmail\",\"SweepstakeName\":\"Promo 2-Sweepstakes\",\"StartDate\":\"January 4, 2023 12:00 AM\",\"EndDate\":\"January 20, 2023 11:59 PM\",\"DrawDate\":\"January 21, 2023 2:00 PM\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','DrawCompleted','Sent',_binary '','2023-01-21 07:28:55','2023-01-21 07:34:58'),(49,'5353f3a1-de6d-448d-b0d7-1b2b0430217d','gamerwalt@hotmail.com','{\"EmailAddress\":\"gamerwalt@hotmail.com\",\"DisplayName\":\"gamerwalthotmail\",\"SweepstakeName\":\"Promo 1-Sweepstakes\",\"StartDate\":\"December 24, 2022 12:00 PM\",\"EndDate\":\"January 20, 2023 11:59 PM\",\"DrawDate\":\"January 21, 2023 2:10 PM\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','DrawCompleted','New',_binary '\0','2023-01-21 07:57:29',NULL),(50,'c7036474-d648-4bfd-9ff0-3b167ba12b02','gamerwalt@hotmail.com','{\"EmailAddress\":\"gamerwalt@hotmail.com\",\"DisplayName\":\"gamerwalthotmail\",\"SweepstakeName\":\"Promo 2-Sweepstakes\",\"StartDate\":\"January 4, 2023 12:00 AM\",\"EndDate\":\"January 20, 2023 11:59 PM\",\"DrawDate\":\"January 21, 2023 2:00 PM\",\"DashboardLink\":\"https://localhost:5001/Dashboard\"}','DrawCompleted','New',_binary '\0','2023-01-21 07:57:29',NULL);
/*!40000 ALTER TABLE `emails` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `flutterwave_account_subscriptions`
--

LOCK TABLES `flutterwave_account_subscriptions` WRITE;
/*!40000 ALTER TABLE `flutterwave_account_subscriptions` DISABLE KEYS */;
/*!40000 ALTER TABLE `flutterwave_account_subscriptions` ENABLE KEYS */;
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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `harvester_service_command`
--

LOCK TABLES `harvester_service_command` WRITE;
/*!40000 ALTER TABLE `harvester_service_command` DISABLE KEYS */;
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
  CONSTRAINT `match_game_board` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
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
-- Dumping data for table `match_games`
--

LOCK TABLES `match_games` WRITE;
/*!40000 ALTER TABLE `match_games` DISABLE KEYS */;
INSERT INTO `match_games` VALUES (73,83,'12ea834d-7b37-4a4d-ae07-3eac0902829b','Match Game 1','Match Game 1 description',100,9,100,6,0,0,'2023-02-01 23:19:44','2023-02-02 04:19:44','2023-03-02 23:19:44','2023-03-03 04:19:44','2023-01-31 23:22:09',NULL);
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
  CONSTRAINT `match_game_image` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `match_images`
--

LOCK TABLES `match_images` WRITE;
/*!40000 ALTER TABLE `match_images` DISABLE KEYS */;
INSERT INTO `match_images` VALUES (1,'fc5fecf3-ee09-4d87-ae3c-ed65fc97ea3f',73,'https://firebasestorage.googleapis.com/v0/b/matchgameuploads/o/12ea834d-7b37-4a4d-ae07-3eac0902829b%2F329133682_3334630180086412_6414166761162211263_n.jpg?alt=media&token=6a9199db-fc0f-49d0-be98-aea43d6f8baa','2023-02-09 17:47:22',NULL),(2,'87068423-1f89-4b83-ad1d-aed60f715bb7',73,'https://firebasestorage.googleapis.com/v0/b/matchgameuploads/o/12ea834d-7b37-4a4d-ae07-3eac0902829b%2F329133682_3334630180086412_6414166761162211263_n.jpg%2FIMG_20190912_104310.jpg?alt=media&token=df3ab924-cd1d-49b5-b66c-3a7015d1fc62','2023-02-09 17:47:22',NULL),(3,'bc1aa2fe-f02f-4db2-8ec3-0a222f156f8f',73,'https://firebasestorage.googleapis.com/v0/b/matchgameuploads/o/12ea834d-7b37-4a4d-ae07-3eac0902829b%2F329133682_3334630180086412_6414166761162211263_n.jpg%2FIMG_20190912_104310.jpg%2Funnamed%20%281%29.jpg?alt=media&token=23f04b84-bdf9-44d0-9529-39ec7283fe46','2023-02-09 17:47:22',NULL),(4,'65f770b3-8275-48ba-b27b-21c9a9328128',73,'https://firebasestorage.googleapis.com/v0/b/matchgameuploads/o/12ea834d-7b37-4a4d-ae07-3eac0902829b%2F329133682_3334630180086412_6414166761162211263_n.jpg%2FIMG_20190912_104310.jpg%2Funnamed%20%281%29.jpg%2Funnamed.jpg?alt=media&token=7516e130-6a35-4dfa-903b-83689b2b7fbe','2023-02-09 17:47:22',NULL);
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
  CONSTRAINT `match_game_prize` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
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
  CONSTRAINT `match_tile_board` FOREIGN KEY (`board_id`) REFERENCES `match_boards` (`board_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `match_tile_image` FOREIGN KEY (`image_id`) REFERENCES `match_images` (`image_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `match_tile_prize` FOREIGN KEY (`prize_id`) REFERENCES `match_prizes` (`prize_id`) ON DELETE CASCADE ON UPDATE CASCADE
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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payment_services`
--

LOCK TABLES `payment_services` WRITE;
/*!40000 ALTER TABLE `payment_services` DISABLE KEYS */;
INSERT INTO `payment_services` VALUES (1,'85cb591f-18e6-4cf3-b2e0-f60a840e0e51','Flutterwave','FlutterwavePaymentService',1,'2022-12-29 00:00:00','2022-12-29 08:45:55'),(2,'85cb591f-18e6-4cf3-b2e0-f60a840e0e52','Paystack','PaystackPaymentService',0,'2022-12-29 00:00:00','2022-12-29 08:48:01');
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
  UNIQUE KEY `plan_uid_UNIQUE` (`plan_uid`),
  UNIQUE KEY `plan_id_UNIQUE` (`plan_id`),
  UNIQUE KEY `plan_name_UNIQUE` (`plan_name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plans`
--

LOCK TABLES `plans` WRITE;
/*!40000 ALTER TABLE `plans` DISABLE KEYS */;
INSERT INTO `plans` VALUES (1,'e39503df-db14-4988-a9ad-adc6e938988e','Basic','A basic promotion plan',1,0,2,5000,50000,600000,'2022-12-11 00:00:00',NULL),(2,'f88e1f57-c2b9-47d0-95c4-3f8e93e619ed','Starter','For more seasoned members',0,0,3,12000,100000,1200000,'2022-12-11 00:00:00',NULL),(3,'8597b37d-99e4-403a-9b1c-7b75a5e78c50','Advanced','For more flexibility',0,0,5,30000,250000,3000000,'2022-12-11 00:00:00',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
INSERT INTO `subscribers` VALUES (1,'gamerwalt@gmail.com','2022-01-17 11:32:08','2022-01-17 11:32:08'),(2,'abayomiayomide12@gmail.com','2022-01-24 22:56:34','2022-01-24 22:56:34'),(3,'mariah_tinuola@yahoo.com','2022-01-25 04:05:33','2022-01-25 04:05:33'),(4,'maryawak187@gmail.com','2022-01-25 05:32:13','2022-01-25 05:32:13'),(5,'04opslag@gmail.com','2022-01-25 05:33:15','2022-01-25 05:33:15'),(6,'abdulakeemolatubosun@gmail.com','2022-01-26 11:42:44','2022-01-26 11:42:44'),(7,'carlaajanzen@gmail.com','2022-02-09 22:10:55','2022-02-09 22:10:55'),(8,'noel_despa@yahoo.com','2022-03-04 20:21:38','2022-03-04 20:21:38');
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
) ENGINE=InnoDB AUTO_INCREMENT=60 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep`
--

LOCK TABLES `sweep` WRITE;
/*!40000 ALTER TABLE `sweep` DISABLE KEYS */;
INSERT INTO `sweep` VALUES (57,'7a92ab24-e482-4fab-bbb5-35d79a276f33',2,83,'Promo 1-Sweepstakes',NULL,'2022-12-24 12:00:00','2022-12-24 11:00:00','2023-01-20 23:59:00','2023-01-20 22:59:00','2023-01-21 14:10:00','2023-01-21 13:10:00',0,0,1,0,NULL,'2022-12-20 10:47:42','2023-01-21 07:27:01'),(58,'e8549356-6181-4010-ac4e-f99bad80c2de',2,83,'Promo 2-Sweepstakes',NULL,'2023-01-04 00:00:00','2023-01-03 23:00:00','2023-01-20 23:59:00','2023-01-20 22:59:00','2023-01-21 14:00:00','2023-01-21 13:00:00',0,0,0,0,NULL,'2022-12-20 10:52:15','2023-01-21 07:28:10'),(59,'d489cbd2-cd3e-4fae-b750-5d5b8a9555bd',2,84,'Promo 2 - Sweepstakes',NULL,'2022-12-23 03:00:44','2022-12-23 09:00:44','2022-12-26 23:59:44','2022-12-27 05:59:44','2022-12-27 14:00:00','2022-12-27 20:00:00',0,0,0,0,NULL,'2022-12-21 17:35:38',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_draw_rules`
--

LOCK TABLES `sweep_draw_rules` WRITE;
/*!40000 ALTER TABLE `sweep_draw_rules` DISABLE KEYS */;
INSERT INTO `sweep_draw_rules` VALUES (7,'f00dcae8-c2a2-4553-bb65-eafebc0fb220',26,4,2,'2022-05-14 22:36:26','2022-05-18 06:01:23'),(8,'dd8d087c-f5b1-44a4-9da3-d20429a6065a',29,5,1,'2022-05-15 13:16:46',NULL),(9,'a2248f02-585c-4615-8850-105ceb76ddcb',27,2,1,'2022-06-26 09:40:36',NULL),(14,'cc1b9071-5dda-46a1-89eb-a86a8a391d3c',30,5,2,'2022-09-04 12:33:06',NULL),(22,'38fbf423-67b0-4e4b-b60c-4b56caa6587e',32,6,1,'2022-09-04 12:46:16','2022-09-04 12:55:57'),(24,'00041c89-ece5-4179-8cf8-603c69851a30',37,2,1,'2022-10-09 09:32:25',NULL),(25,'3f6bc6ec-6ca8-49e6-9ecf-5c7a318a8d9c',38,2,1,'2022-10-09 12:44:25',NULL),(28,'455c66c3-556f-482b-a552-45f6a0c0bba9',47,4,1,'2022-10-23 12:40:38','2022-11-05 09:27:51'),(29,'54992d79-6016-4eeb-95b5-2e196cfd6b91',48,5,1,'2022-11-06 12:08:37',NULL),(30,'38bde9cf-b559-455d-98b8-dd4852e09d9e',49,2,1,'2022-11-11 12:06:28','2022-11-11 12:31:36'),(31,'f6278af4-de51-424c-9c38-1178d2795ebe',53,2,1,'2022-11-11 15:21:06',NULL),(32,'81615231-a283-48e9-bc64-387b90f176f4',55,3,1,'2022-12-12 23:37:11',NULL),(33,'ef4245cc-ed99-4362-97b7-4fad049fe8e5',56,1,2,'2022-12-17 07:52:06',NULL),(34,'fe593dce-2481-4261-b024-30c331ef44ea',57,2,3,'2022-12-20 10:48:04','2023-01-21 06:38:03'),(35,'7b4a9f95-7f56-4f8c-9a4b-a7c5a60e96e5',58,2,1,'2022-12-20 10:52:21',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=567 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entry`
--

LOCK TABLES `sweep_entry` WRITE;
/*!40000 ALTER TABLE `sweep_entry` DISABLE KEYS */;
INSERT INTO `sweep_entry` VALUES (167,'0de84468-c726-4bfe-ae6a-177c19d57164',57,101,'Arno.Rau','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(168,'210068cc-7ecc-444b-9bf0-d3c211eb41cf',57,101,'Reyes.Feil','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(169,'68b2992b-aaef-47e9-9ece-799e8da91e08',57,101,'Edison42','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(170,'63ed72e0-d627-45df-834f-bbe876a4fa82',57,101,'Diamond41','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(171,'c8abf89e-d58a-45dd-a471-3d2c807a51ff',57,101,'Harrison48','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(172,'c17ccc82-fd9d-416f-9cbd-06c50c7fedc5',57,101,'Derick29','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(173,'494295d8-9413-40dc-b1d3-f6bca52349c9',57,101,'Roger_Sporer50','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(174,'17b7bd37-360a-4960-a4e1-8b25889035d7',57,101,'Carlo.Emmerich32','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(175,'eda0ced5-0040-4a83-ae43-4ebe4f1d658a',57,101,'Luciano.Bechtelar63','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(176,'b87e6053-a01a-4bc4-987e-42a2cec9b806',57,101,'Ada_Considine88','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(177,'25e2f162-088c-49b9-b660-f6375f95bdab',57,101,'Larue_Howell92','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(178,'32f45d2f-e636-4237-8ceb-aefbccf0f00a',57,101,'Franz.Schinner','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(179,'cdd3f65a-8792-4e0a-aecc-779412ca742e',57,101,'Alexis.Harber','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(180,'a605b665-e757-4f4d-83b7-4720202c0f09',57,101,'Darlene.Nikolaus66','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(181,'616b15e3-d59d-49b4-81f6-2ad030df8df4',57,101,'Madge.Christiansen28','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(182,'b0cb628a-ef8e-4707-95bd-3d710b7a793e',57,101,'Troy68','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(183,'ea8b2cb0-e877-401b-a258-aa45d5268a92',57,101,'Fidel34','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(184,'0644c262-48c2-46ce-b825-afd6242b907a',57,101,'Harmony_Olson','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(185,'a8dc2e0d-ddda-40f3-a153-5722f84284a5',57,101,'Ivah3','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(186,'eca1fac3-60ab-4479-b373-c410a36c8f3b',57,101,'Bryon_Hessel10','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(187,'91de5641-69a9-4d35-ae57-2699e69cc463',57,101,'Zachery.Dickens','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(188,'cbdeb3a4-34f8-4277-8daa-c78fd61b8954',57,101,'Heber.OKon','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(189,'8a841d9f-b5a6-410b-b1d9-a2ea307dc877',57,101,'Christy21','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(190,'d1be31db-2e12-48a8-8b6f-ac972194c311',57,101,'Bartholome_Rogahn67','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(191,'2a107d68-db25-4f56-958b-bf9797dacc61',57,101,'Imani_Schultz91','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(192,'07cbd819-89b0-49c5-b8ba-9d45afaf6d08',57,101,'Paula.Wisoky','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(193,'562d31e3-f6d7-4877-9b30-8f56a55cb56f',57,101,'Reyes_Flatley','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(194,'f0121aec-82b4-436e-8983-b30133dac21b',57,101,'Odie.Bednar94','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(195,'d8bc4f1f-2f3f-4f63-93e2-a4c2ed0e407e',57,101,'Leonard93','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(196,'f5fd5d5c-c3db-4d55-a1ac-56986bc219c9',57,101,'Trenton.Klocko81','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(197,'82a45527-cfab-4b44-9419-5d2e65c9acdf',57,101,'Gust.Kuhic','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(198,'a6dc5bc3-134d-4c0f-9b01-39c08f020239',57,101,'Bulah_Fahey','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(199,'bcbb3898-c168-475f-93f7-5543c44bba46',57,101,'Zackary.Mueller','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(200,'72bf5c09-d43b-4d3c-a272-eddfb91c50a7',57,101,'Gracie.Pfeffer82','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(201,'0d0043a6-0c1b-434d-a567-c61ecffab350',57,101,'Gwen.Steuber96','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(202,'ba142b3e-11d7-4578-82db-ea35846f8642',57,101,'Imogene.Waelchi34','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(203,'3defd343-b5ec-47f6-b8bc-4bc2f2ca30fa',57,101,'Jaycee_Lang83','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(204,'34550974-92df-4ee6-9e79-dd85960ae6f4',57,101,'Martin.Wiza','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(205,'df918cc1-eb0a-4641-b606-f93f26daf60c',57,101,'Roger40','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(206,'3a5d95a0-2395-44a0-a693-92789eda1c9c',57,101,'Alvis14','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(207,'60880b50-853b-41dc-818b-18e35651e117',57,101,'Olaf77','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(208,'9020b214-e0ac-4ee3-b9cc-f6ca05104750',57,101,'David4','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(209,'2d76dc32-6eec-4583-839b-a590c4e0c9af',57,101,'Abel.Lueilwitz4','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(210,'5e2b3ffc-9cbe-4dfc-a50a-1d274b5e2865',57,101,'Hilda_Monahan98','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(211,'df351362-74f2-4d44-ae6d-c5bdc86bf2e7',57,101,'Hailie57','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(212,'0582b18f-1296-4473-8b57-5b6797503b54',57,101,'Gilberto91','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(213,'516ab92f-f7dc-4c86-89c3-da141dfa1465',57,101,'Giuseppe45','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(214,'b69b7532-4243-45e7-b935-83c901c3c02c',57,101,'Lizeth.Ernser14','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(215,'a55e6ad8-9f40-4521-838d-c240aa4c5820',57,101,'Timmothy.Corkery','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(216,'4ace4f84-4184-4b14-8b6a-3fdb11c9bc58',57,101,'Rachael_Russel','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(217,'0b8e11bb-201a-4e8e-b510-190d32afa7e6',57,101,'Brian0','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(218,'8556ec59-b379-42ad-8519-6f7e12f549f1',57,101,'Halie52','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(219,'789f3c22-e652-496b-9139-54ccbf7279f1',57,101,'Angel.Kihn19','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(220,'2ac6786f-5130-4640-a576-5f511d2e28b8',57,101,'Jaylon.Schamberger','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(221,'6a0653a6-e64a-4586-8556-ee29bef82b86',57,101,'Elton_Predovic92','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(222,'a71a731c-f49d-4907-ba3f-00bbdc8f9b18',57,101,'Julia.Lesch68','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(223,'d1b5b266-d0ec-40da-9b9d-d89e9875a3d0',57,101,'Lucas_Haag34','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(224,'2a0b764e-c27b-444e-851c-4f7c4128019b',57,101,'Leopold19','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(225,'12a5926a-7371-450f-95ae-3ff8ff615a08',57,101,'Abelardo_Ullrich99','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(226,'21690ef7-d2dc-4804-b54c-09af7c279aa4',57,101,'Toney_Mante24','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(227,'16f02ba3-0e4c-40fd-b182-fd17105eab30',57,101,'Verna_Kilback','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(228,'c018c4c0-8e13-4e23-bb9f-5a92461f0690',57,101,'Terence74','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(229,'a507f3eb-b6d6-43bb-b392-d643c642ca3e',57,101,'Earl_Durgan26','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(230,'b61322ca-0045-4bab-9826-0b7df216afb5',57,101,'Brendan.Cassin31','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(231,'7e4db38f-a535-4763-b937-a4543641372b',57,101,'Roma.Moen','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(232,'1516b31a-4b60-4568-a1db-5b710c10c277',57,101,'Daphne.McLaughlin','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(233,'bbcfaae3-50c2-424b-8b1e-bb2f17a376a8',57,101,'Erling_Botsford35','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(234,'487e3382-9f12-4269-83e1-8ee8ce83ab30',57,101,'Nelda.Mayer','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(235,'a6f02c12-047b-42b7-a15c-2671cd5d8cf8',57,101,'Davin_Grady','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(236,'0695115f-b7b4-4ab1-bee9-e27ef4753a76',57,101,'Benny.Glover4','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(237,'f3e39060-ad15-4495-b914-2b0e95183f9c',57,101,'Jeromy_Johns','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(238,'5cfef39f-3733-4a2b-92b8-8f896b8fa9bb',57,101,'Jarret.Hand93','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(239,'e8c46dd5-fb28-4e60-b466-bc206aa56480',57,101,'Felipe.Schmeler','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(240,'4c03acd0-e88f-4ea3-9d47-bfba92fc8472',57,101,'Novella.Gottlieb','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(241,'78ccacc4-0e09-43ff-a4ea-eb9715ec5e1a',57,101,'Will_Ziemann','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(242,'2fe30948-1d84-4ae0-9b7e-673afbaa1334',57,101,'Vaughn_Anderson88','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(243,'c0d2bb56-fa47-441f-ae00-e729a38e3bfb',57,101,'Giuseppe_Smitham','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(244,'4f5e348f-b9ba-4a3b-90f8-b0c7420de320',57,101,'Ethelyn.Marquardt9','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(245,'628222b0-be92-43a3-b908-e2a6d25ef3ce',57,101,'Daphney_Sanford','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(246,'9dff4199-8304-40b3-a716-4940db765a49',57,101,'Henderson81','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(247,'f402728c-f650-4d0d-a7d0-b76fac9b4bc6',57,101,'Dangelo.Purdy51','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(248,'cbb5083a-ef89-4caa-9e3c-a8f8003c9d91',57,101,'Beau.Harris87','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(249,'062e4ff5-17a4-4b02-b23f-661c2c60d261',57,101,'Queen.Dietrich','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(250,'e384ed8a-5c7a-4f6f-bdb3-797436460913',57,101,'Sibyl_Bauch','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(251,'f6e26376-9802-4fe9-8632-f167b478417c',57,101,'Sedrick1','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(252,'d79b82b2-8a7c-4359-a09b-e93673a6ce2e',57,101,'Daniella66','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(253,'c6fdfed2-b4f9-451d-830e-e0339d7b886f',57,101,'Amalia.Reichel57','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(254,'967f3f31-610c-4e9e-a052-4c73f2ddd204',57,101,'Noe78','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(255,'8fe13ee8-2ff6-4490-a2e3-7d7819f8e4e9',57,101,'Amber_Zulauf73','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(256,'f6bc0fce-cb31-4b39-90dc-7acc640d53ff',57,101,'Carolanne69','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(257,'3110a6c4-0559-48c1-b1bd-e4593a02e871',57,101,'Chadrick_Yost','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(258,'f062c251-4d2b-491b-a453-836f5e9e1a43',57,101,'Shirley49','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(259,'8ea7f0d0-0f3e-491e-a725-54b8ec2b631a',57,101,'Alessandra.Heller','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(260,'51185c11-dbc1-4ca0-bd26-a0f770591739',57,101,'Javon_Goodwin5','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(261,'039dced8-4f23-4d6a-9bfb-6e59ce81c9db',57,101,'Leopoldo.Friesen4','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(262,'3224426c-a1ab-45de-859a-fd59e741a61c',57,101,'Jackie_Jacobson','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(263,'b19b37b3-f594-4d8f-af29-3c68ebfc45a6',57,101,'Allison14','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(264,'0f57e093-3d3b-4c3a-bb5d-6fc7c8b6f57d',57,101,'Lavinia_Jast36','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(265,'f8b92e16-5b7f-4fb7-9cc7-005f3d444943',57,101,'Alvah83','TwitterHandle',_binary '','2023-01-21 06:42:18',NULL),(266,'8abad924-ba93-4386-9673-031c0396b15d',57,101,'Derek73','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(267,'4af7aab7-1ae5-4d1e-93c3-52042c3cf576',57,101,'Giovanna_Lesch','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(268,'08656f5d-aad3-4e1d-95f0-19292f82ad4a',57,101,'Mohamed67','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(269,'697547d0-6b41-4ee8-bbab-ed5bd1d91e6f',57,101,'Katarina39','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(270,'43e5f6f5-f6c6-463a-8ee6-ccfdde82766c',57,101,'Breana20','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(271,'570b5cc6-358e-45c4-96c1-1090e06fb27c',57,101,'Amara43','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(272,'93a48808-e38d-4c9b-a9b8-5a4f595bd32e',57,101,'Sheridan24','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(273,'63d7d022-8219-4b56-9829-d8c58d50e337',57,101,'Unique_Veum83','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(274,'386691f3-6295-4869-b9c7-117454469920',57,101,'Oscar.Ziemann','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(275,'dbe787bf-a4d3-48b0-89a4-325ce984c0b5',57,101,'Elyse.Bergnaum26','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(276,'df91f0d4-488b-44e0-a2b1-1717d9ac89a2',57,101,'Emilia46','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(277,'6a62f0e6-1322-4f51-8a50-33f9320c5278',57,101,'Amelia.Stracke23','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(278,'9e8d245b-8b6e-4a50-b356-992ee21db47c',57,101,'Jon78','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(279,'986952ca-d677-482e-9395-0f70fc23c2c5',57,101,'Sadye_Bahringer','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(280,'877b2595-6f20-4d29-8cf0-c13bb1aa65f0',57,101,'Lucie_Harvey','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(281,'a1f14fff-91c0-4392-a8fa-f632a7ffa2a9',57,101,'Randi84','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(282,'10d0641d-a9fa-42b6-97ee-414853c8858a',57,101,'Nyasia.Lubowitz','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(283,'a9223e05-ea43-4ab1-b200-3c5ab1ee2bfa',57,101,'Reagan16','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(284,'63250099-e06b-46b8-bfce-14f889902299',57,101,'Oswald.Blanda','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(285,'e4b6e59d-9c94-46d6-afc7-c289701891fe',57,101,'Henri.Prohaska','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(286,'c2fde995-4551-4ffe-8270-1693e2490ce4',57,101,'Rafaela_Parker','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(287,'e2ae8689-de46-46f2-a9a2-b6fe5c5d2915',57,101,'Rebecca57','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(288,'e21035ad-458a-4994-aace-4e6e665dd3d9',57,101,'Kyler_Koss','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(289,'7094faaf-35ab-4cc5-92a8-45eab42cbe47',57,101,'Megane_Nolan','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(290,'0d8114c4-37b1-4b8c-b322-2b46cb9081c1',57,101,'Minnie31','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(291,'9bf917d7-510a-4de7-85d7-ba33e3152b01',57,101,'Emmanuelle.Wiza','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(292,'ab9b7f4a-db18-4739-a321-19e5d2f93c30',57,101,'Marquis78','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(293,'62938ef8-c3e3-4780-8cd2-b70120244523',57,101,'Keely_Ernser','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(294,'52aa1aff-c016-4896-929a-dbbb4eb9d9fc',57,101,'Zoila.Wehner49','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(295,'745b4089-f2ae-4b79-9042-30ca13cd99cc',57,101,'Kara.Barrows95','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(296,'fdd4fee1-46dd-4ddb-87a9-452b8600f99b',57,101,'Kole74','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(297,'a9422cda-eee8-4dc8-a696-2476eb9e1f2e',57,101,'Jakob_Conn','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(298,'baa326c5-2a50-48f6-a76b-ff9abb95ba23',57,101,'Maureen.Schoen','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(299,'a7262e60-20dd-4e6a-9d04-b719c93569be',57,101,'Ardith38','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(300,'79206030-e84d-47a4-b0ee-b120e6eaab59',57,101,'Antwon.Wiza1','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(301,'c63f641e-a217-4196-a5c4-436ea97dea68',57,101,'Freda_Beahan','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(302,'2a9bea1c-cb01-49ba-a884-3e11bba30cb1',57,101,'Octavia.Simonis','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(303,'d2907bb4-63a2-459e-aefc-870c90ea9ff4',57,101,'Chelsea_McKenzie','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(304,'b3120d83-1a8d-4f56-a1d0-18167138cf47',57,101,'Fredrick0','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(305,'1caf94f4-934d-4708-94f1-031128d32c15',57,101,'Triston.Hoppe1','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(306,'b996d3be-f664-4414-894c-3903d036bd35',57,101,'Julian_Morar','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(307,'34ffe46c-d949-40a7-b7b6-adfacac0a96d',57,101,'Sadie.Wunsch46','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(308,'a4c04f14-6898-4033-bf26-541e8bad4cd4',57,101,'Amya93','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(309,'23c341cc-96c8-4936-a9ac-3f4920f90c0b',57,101,'Cristina.Gutkowski','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(310,'8a892660-76cf-4b34-b946-52b99c739a84',57,101,'Maud11','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(311,'e6ebec2a-6c5e-42d6-9e2d-0765a05d73c4',57,101,'Jorge.Erdman70','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(312,'b076185d-ae05-4b91-a522-87489dc49276',57,101,'Icie.Douglas','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(313,'db88dec1-7ca6-4860-9880-b16aacb44db8',57,101,'Lambert99','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(314,'7dc64002-6067-45a6-8152-6359f71f9b86',57,101,'Rogelio_Kerluke90','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(315,'53f7fa20-c184-4083-88ac-934e34dc9839',57,101,'Rosemarie71','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(316,'24239c26-0c29-4e3b-a627-eb464ebad9e7',57,101,'Kacey.Rogahn18','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(317,'505a9e88-3286-43d0-ab5c-082ad757ee05',57,101,'Norris_Kilback55','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(318,'fefa2657-104b-4ebc-9bbd-dfbd44788b1b',57,101,'Aiyana39','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(319,'7e39f459-cb61-4e97-b6a4-9fa82a237f51',57,101,'Lindsay54','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(320,'fd7748f1-2980-470b-ad72-aefd232adf4e',57,101,'Maia.Raynor','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(321,'d5df594d-b390-406f-be4c-d6f248b8f0ea',57,101,'Lonny.Welch86','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(322,'ef99ef8f-18f0-49c0-b1f6-b265a9d5c0e3',57,101,'Lavern.OConnell68','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(323,'bdf6d7c5-bbb9-498c-8a03-092924bb4fa3',57,101,'Pat.Schneider31','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(324,'74e8a40e-cb38-4196-8da2-e51fa7be09f1',57,101,'Dylan_Gerlach','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(325,'109a6317-9e40-4095-8539-bee478c1a945',57,101,'Gregorio.Turcotte','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(326,'50c0c68c-8b5e-41be-83d3-efd9cfda5cb3',57,101,'Athena33','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(327,'531840e0-7842-49e2-a366-b08eecebec15',57,101,'Milan85','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(328,'85910a96-ecaf-4c92-ac54-918fa811af65',57,101,'Lennie11','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(329,'4044e96b-6db3-4aa8-8155-f8d6bb82b24d',57,101,'Scot_Kuhic84','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(330,'227305fb-5c5a-4640-8c03-47184862c978',57,101,'Arnoldo33','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(331,'e9f33b65-59f3-48be-8458-a9861446fcd3',57,101,'Bettye.Doyle52','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(332,'2bd57142-b08b-466b-be41-f1d12d6ab69f',57,101,'Savanah77','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(333,'ac7c2aae-6a21-4c0f-80c2-5c4443739aa1',57,101,'Travis83','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(334,'cf8dd0cb-384b-4378-a9ce-5f54da7829e8',57,101,'Royal.Lockman93','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(335,'cbb2b15d-e972-4097-958f-aa859b183c08',57,101,'Alexys.Crist43','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(336,'f29203bc-ea3e-4e14-9eef-fa19a6bcce78',57,101,'Libbie_Bashirian','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(337,'fccf8575-ae2d-475e-927c-4e2d4e7716cd',57,101,'Jabari31','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(338,'fd2d2232-f6d3-431c-81ec-708ecb83b94a',57,101,'Abbie_Kulas62','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(339,'9049771c-5a12-4425-8226-a388a90bd8a0',57,101,'Esmeralda88','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(340,'2c498528-ddf2-41d7-ae4f-14b2203345e0',57,101,'Alaina.Murazik50','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(341,'45a2665f-08e3-4e54-a280-d656070aaf0f',57,101,'Annabelle.Batz','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(342,'77d1e029-e7b1-4398-94e8-96980f9b240b',57,101,'Annette86','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(343,'1dd164de-4b68-456a-8406-5b0f36236edd',57,101,'Tremaine_Labadie8','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(344,'5cc638a3-3a83-42c2-8c12-5794ebf3c115',57,101,'Howell83','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(345,'984dd107-8705-4b73-8a9d-f50faac85dca',57,101,'Marjory_Parisian','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(346,'7ff02ff2-4feb-491b-b073-9d6941638eeb',57,101,'Keanu_Beier49','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(347,'f446feeb-455c-4b0f-866f-66d6c62abbae',57,101,'Gonzalo13','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(348,'42f7605e-2499-453b-b436-190c2318131f',57,101,'Conrad.Heller96','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(349,'69951a48-d5b0-4797-8140-6034107a997d',57,101,'Gregoria47','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(350,'28004528-483d-4664-91e3-5f303f6e5642',57,101,'Johanna55','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(351,'77f3f5a1-0048-4156-9876-190e4ee3954e',57,101,'Domenic.Bauch42','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(352,'0d31787b-3a0c-4914-85a6-f76bcac97675',57,101,'Barrett94','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(353,'24f94815-7709-4e13-9fd3-db882326a65f',57,101,'Candace_Senger97','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(354,'26e99b4c-2dd1-495c-8cc5-1826fc45fd12',57,101,'Nicolas.Hauck','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(355,'1b472f5d-f96c-4954-b23a-1ada8d27753b',57,101,'Hanna.Ratke58','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(356,'d3e2d525-9d5b-4adb-ab9d-9009f4850e61',57,101,'Otto_Wolf','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(357,'e1ce6b89-b79a-4b74-b2df-d64cec8a7e32',57,101,'Telly17','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(358,'7675fe68-7a54-4464-b3b2-98083808ee9b',57,101,'Ivah79','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(359,'9c35a0ac-f917-4ac8-8ac0-11c541d67183',57,101,'Rodrick96','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(360,'166217d1-4865-494f-be82-1c70d1217c37',57,101,'Lucie.Heidenreich19','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(361,'52b7d188-dad3-4ecd-b6db-06c4185eeaf3',57,101,'Caterina.DAmore','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(362,'11c563c3-b20b-4fd0-b76c-d5e1de4d3c67',57,101,'Gussie.Stiedemann75','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(363,'b44d6fd3-71f7-4eb7-a6e1-b826a0456872',57,101,'Helene.Romaguera6','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(364,'ba36196e-9702-4ead-8d46-750e2cdf60d3',57,101,'Dolly_Windler','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(365,'d29d197c-45e4-486e-93c0-70563e32a116',57,101,'Kay.Renner51','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(366,'3e7793a1-205f-4b5b-8d17-8e48d1b373fc',57,101,'Jocelyn.Denesik31','TwitterHandle',_binary '','2023-01-21 06:42:19',NULL),(367,'bbc1bf80-dc07-4f63-ab8f-341499968f84',58,100,'Arno.Rau','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(368,'1457289c-0d5d-46c3-95fb-a45d6a263fa7',58,100,'Reyes.Feil','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(369,'d6de994b-8564-4466-bb4e-8d1cbecbefa5',58,100,'Edison42','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(370,'538f61f1-3f55-444d-ac87-80ab1477c46b',58,100,'Diamond41','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(371,'37882282-f747-47af-80bf-0f8e8143e3f7',58,100,'Harrison48','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(372,'8538adc7-0204-4a95-ac0d-8fa20d1ecca5',58,100,'Derick29','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(373,'bf4bc340-addb-43d5-8f20-4aff4b15c01e',58,100,'Roger_Sporer50','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(374,'69d8f43f-72e3-456a-9e4c-122520fae67c',58,100,'Carlo.Emmerich32','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(375,'819357ae-5b88-4b30-a4e0-5d3880c8532c',58,100,'Luciano.Bechtelar63','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(376,'c7f03bad-4af9-4d86-a675-0020217e0548',58,100,'Ada_Considine88','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(377,'c65d3149-1d97-417d-8583-8005cc3f003e',58,100,'Larue_Howell92','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(378,'721036a0-9d82-46bf-b7b2-af8ba08a13c1',58,100,'Franz.Schinner','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(379,'6c2bc6f0-12fe-42bf-9235-843be75b235e',58,100,'Alexis.Harber','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(380,'35802e42-f00b-4a96-ab58-f99650c47a6c',58,100,'Darlene.Nikolaus66','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(381,'cf99e740-1951-40c3-b06d-1901f3425c6b',58,100,'Madge.Christiansen28','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(382,'99ae0d0f-2ecf-4a87-a7bb-fe772015b45e',58,100,'Troy68','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(383,'8ff1b471-0a0d-4345-a188-2a22fd473ed8',58,100,'Fidel34','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(384,'519a20c4-05f1-47f7-930c-92af45d91215',58,100,'Harmony_Olson','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(385,'6038db0f-1e38-4331-b495-dc79d2fec9b6',58,100,'Ivah3','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(386,'23f50cce-f1b6-47b3-99c0-58a71ad37e98',58,100,'Bryon_Hessel10','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(387,'0cfd4b46-6b27-457d-9d81-80beaee7e3aa',58,100,'Zachery.Dickens','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(388,'05dee668-65c2-422a-9448-d9fe0f3ec827',58,100,'Heber.OKon','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(389,'e3dd522d-9347-4e33-9136-583fd6288016',58,100,'Christy21','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(390,'117728d5-03e4-41c0-b2ef-6066631c1639',58,100,'Bartholome_Rogahn67','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(391,'cb67ba1c-b7a9-4789-9c6f-c4f0c0f155aa',58,100,'Imani_Schultz91','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(392,'e234790d-564b-4ba5-86b7-023f70083469',58,100,'Paula.Wisoky','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(393,'d7865aa9-d016-4700-a2a7-08f90d81f0a9',58,100,'Reyes_Flatley','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(394,'84089b37-3bb6-445f-a2b4-519941a5cf92',58,100,'Odie.Bednar94','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(395,'105dfa93-67ad-464e-bbc9-5aca50a82efa',58,100,'Leonard93','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(396,'8dccb3b8-17c0-4e4e-a1e5-6decfdae7abf',58,100,'Trenton.Klocko81','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(397,'da80ee89-27b9-450a-8a55-be33a9e9ac7f',58,100,'Gust.Kuhic','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(398,'9677f690-b83b-4ba4-a846-961f8bb42e98',58,100,'Bulah_Fahey','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(399,'5468387e-5fde-43ae-af74-f09af4ba80e5',58,100,'Zackary.Mueller','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(400,'45b9d657-4164-4eeb-9613-7447c56b030c',58,100,'Gracie.Pfeffer82','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(401,'1ddcc2a1-7329-42c0-b873-d218cf44b7fa',58,100,'Gwen.Steuber96','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(402,'bf1d2c66-94b4-467e-a82f-a7031cdd340f',58,100,'Imogene.Waelchi34','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(403,'f76012f4-4bce-4d72-96ca-873985364ea3',58,100,'Jaycee_Lang83','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(404,'3a7ad7c3-b2fd-45b3-a263-14c75ce300d5',58,100,'Martin.Wiza','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(405,'0c7e09aa-807d-41a9-97ba-5e098ce5cd27',58,100,'Roger40','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(406,'b98fa2b6-15f3-44af-ae86-c1acc41e4dd3',58,100,'Alvis14','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(407,'a3192e47-9ad2-4fed-9185-a6aa45b6d129',58,100,'Olaf77','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(408,'1e7370ae-9113-4bef-bcb6-f287a4bc37b6',58,100,'David4','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(409,'294eadc7-426c-4a87-83dc-4b61065f0774',58,100,'Abel.Lueilwitz4','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(410,'4a3c88ab-1173-4388-98b1-5bdb1e38ff45',58,100,'Hilda_Monahan98','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(411,'df0808fe-5103-4dbd-b3f5-fcacd0948108',58,100,'Hailie57','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(412,'5ae21f3a-9eb6-4da4-a65a-f22f41d30d12',58,100,'Gilberto91','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(413,'b0481ed4-d1c6-42e9-bd0c-13b1affb8426',58,100,'Giuseppe45','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(414,'a5a40ff3-f32f-4780-ad43-ca039d72ca7f',58,100,'Lizeth.Ernser14','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(415,'d6f07b44-a0d7-4268-87b4-5ccbd1a5c2e4',58,100,'Timmothy.Corkery','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(416,'a76f588e-9489-481c-b4e1-22ac531b9fc5',58,100,'Rachael_Russel','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(417,'9f9932da-193e-4ac2-accc-de5c87ef01bc',58,100,'Brian0','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(418,'7441754b-56d7-407a-8d3c-bf48474a4d99',58,100,'Halie52','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(419,'fa405aba-4e25-46ff-ada7-14e92dbfeaf6',58,100,'Angel.Kihn19','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(420,'7d56e2ff-5221-46d9-8364-71bb99037d58',58,100,'Jaylon.Schamberger','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(421,'90113f6c-b0f4-4873-a9ba-50606f0212b1',58,100,'Elton_Predovic92','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(422,'d05cfe48-20df-4db9-b1bb-afa08a3a67f7',58,100,'Julia.Lesch68','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(423,'aba547a0-9abc-4245-b3aa-550d6a1cbbc5',58,100,'Lucas_Haag34','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(424,'10e1ab36-6db8-4a24-9937-5ed7ada37eb3',58,100,'Leopold19','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(425,'b611efa0-56fd-4c38-a762-0c439d9ed7e9',58,100,'Abelardo_Ullrich99','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(426,'61569579-561e-45bf-aa5f-7f59be0b8bb2',58,100,'Toney_Mante24','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(427,'1f8ab2f4-d570-4bdb-a058-5a846cc16f6b',58,100,'Verna_Kilback','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(428,'963a9c96-2c5e-4459-b3d3-8115d2a70805',58,100,'Terence74','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(429,'7de300df-f9a3-48c7-abaa-6c130528cdf5',58,100,'Earl_Durgan26','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(430,'27cce9a0-4078-4dab-a6e3-499e70220885',58,100,'Brendan.Cassin31','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(431,'3b31ad09-7b57-4e4c-8a34-86a9c64bdd46',58,100,'Roma.Moen','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(432,'536cdc5e-7e70-49c0-9f28-0f000da53799',58,100,'Daphne.McLaughlin','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(433,'8961149d-24e8-4770-8821-719d5a4fcffb',58,100,'Erling_Botsford35','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(434,'66ca67d9-ae3d-46ee-b79d-c2de97ba200c',58,100,'Nelda.Mayer','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(435,'c148ebed-ccf9-4b31-ad00-dd9700e88682',58,100,'Davin_Grady','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(436,'13d94855-1d3b-4340-99ca-6641aa8f6fba',58,100,'Benny.Glover4','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(437,'3fd936ff-d1dc-4c37-a2f1-84d808ec370d',58,100,'Jeromy_Johns','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(438,'1038182a-065c-4ae3-8a48-b71ec8e634f9',58,100,'Jarret.Hand93','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(439,'199c516f-502c-4cbd-8e71-17a359d9f8b0',58,100,'Felipe.Schmeler','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(440,'1cb6dad6-8513-45f4-960d-1e4d83186f09',58,100,'Novella.Gottlieb','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(441,'1867cba8-88d6-46e7-8bea-88c279b1ff9c',58,100,'Will_Ziemann','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(442,'c259a88c-4b90-483d-8b01-0b5a357dcb06',58,100,'Vaughn_Anderson88','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(443,'8ea24db3-b9ce-43cb-a35a-c64f90c04855',58,100,'Giuseppe_Smitham','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(444,'affadd61-0011-447c-9e96-76907481b77f',58,100,'Ethelyn.Marquardt9','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(445,'6f94ebb3-88d9-40b9-b6f1-2fb4030648e6',58,100,'Daphney_Sanford','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(446,'2e428f5c-3028-42e4-b10c-dc829083e973',58,100,'Henderson81','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(447,'b84a33a0-f4bb-466b-a457-5d39c6526448',58,100,'Dangelo.Purdy51','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(448,'bbbd9982-e184-4157-ac27-fc92b31132db',58,100,'Beau.Harris87','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(449,'d50facd8-9d09-46cc-8f39-3d93db3f1a29',58,100,'Queen.Dietrich','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(450,'b5dd4412-bcca-4456-9bba-1e6f7dab56d2',58,100,'Sibyl_Bauch','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(451,'792b7495-c3d1-431c-a786-ed89a1f264b9',58,100,'Sedrick1','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(452,'c43e1cd7-e99f-4958-a231-17b2ffbb7476',58,100,'Daniella66','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(453,'2ddba869-e6db-46ff-8504-9d03314ef373',58,100,'Amalia.Reichel57','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(454,'9d72e78e-f87f-4e9f-a99a-33a613654211',58,100,'Noe78','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(455,'31f95521-8814-4099-828b-03f293e38fb2',58,100,'Amber_Zulauf73','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(456,'a753738b-267e-4c42-80e3-263f367e1929',58,100,'Carolanne69','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(457,'b9775030-7b44-4134-bd7e-7e9e132845af',58,100,'Chadrick_Yost','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(458,'2660e6e6-d01c-4733-9ca2-317fdd49a170',58,100,'Shirley49','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(459,'e3671be5-8513-4452-9e2b-474ef187807d',58,100,'Alessandra.Heller','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(460,'33a5bf51-c97e-4d19-9f3d-d5d595042d0a',58,100,'Javon_Goodwin5','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(461,'8a3dabdb-ab88-4bd5-97a2-2da06e98edb3',58,100,'Leopoldo.Friesen4','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(462,'8ebbf3ec-58cb-4027-93e2-79b1dec86fe9',58,100,'Jackie_Jacobson','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(463,'d7289f0e-2771-4ead-bc49-1ea9c9a635c3',58,100,'Allison14','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(464,'f9952a42-a7ac-4fe2-8fd3-ef884c0b8fd5',58,100,'Lavinia_Jast36','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(465,'489c98ad-f8bd-4fed-b573-cd9e9ffaf8db',58,100,'Alvah83','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(466,'49d05459-abdc-4ba3-ad76-b37c327f8fef',58,100,'Derek73','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(467,'700baf07-c037-40eb-9b07-b3c72aa97072',58,100,'Giovanna_Lesch','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(468,'81a4a0e3-036d-4443-9011-952da4c7aac2',58,100,'Mohamed67','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(469,'25b045ee-299a-4dfe-819f-73f7063419f0',58,100,'Katarina39','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(470,'2f8f5996-5d2e-4140-91b4-c54666a3f8d6',58,100,'Breana20','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(471,'bd98618b-cc29-4dc4-a0ba-c001af3ddd11',58,100,'Amara43','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(472,'4abb0b52-8d5e-4a9e-a411-e3e070f65fde',58,100,'Sheridan24','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(473,'72eee3cf-4c7f-47eb-b922-b9de58c1bbf5',58,100,'Unique_Veum83','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(474,'de754af5-bdbb-4511-b3ec-690f5f1e206b',58,100,'Oscar.Ziemann','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(475,'636d70ed-1949-4eb4-b25b-716f1586ab7f',58,100,'Elyse.Bergnaum26','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(476,'8c75b919-59ee-4764-8fce-b2dafd0d3391',58,100,'Emilia46','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(477,'1bfa95c2-957c-4b42-9422-4c14fedab2d1',58,100,'Amelia.Stracke23','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(478,'939e43c6-304e-42a9-85be-f05c8d4ed5e5',58,100,'Jon78','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(479,'78280cf1-68a4-4aec-a4ca-2d6b01e16556',58,100,'Sadye_Bahringer','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(480,'8a957218-733f-4378-8515-03ca3ebd349c',58,100,'Lucie_Harvey','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(481,'9bf7e832-a8d7-4435-bed2-66cf22db4ca9',58,100,'Randi84','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(482,'70be79f9-aed4-4c62-bd75-2530c636019e',58,100,'Nyasia.Lubowitz','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(483,'2b84a11a-b082-4e51-934a-915b6f58ffcf',58,100,'Reagan16','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(484,'f9996b3d-807a-4864-8059-f5158f2aa32a',58,100,'Oswald.Blanda','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(485,'9be11ed1-a062-4f6b-9810-94e1fef18903',58,100,'Henri.Prohaska','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(486,'2b8f1995-0667-4ce0-8e46-4bb588367970',58,100,'Rafaela_Parker','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(487,'9daf91e2-3638-42ac-b173-1a980618c136',58,100,'Rebecca57','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(488,'4d980d04-8ef0-477a-903d-6a8994cab125',58,100,'Kyler_Koss','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(489,'e9d6e3da-e866-4aba-9acf-177599c9902c',58,100,'Megane_Nolan','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(490,'da800a32-f9b1-48b7-abff-86ad4b177c78',58,100,'Minnie31','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(491,'3ee195d4-0ca5-4769-bee7-646d65f51bba',58,100,'Emmanuelle.Wiza','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(492,'f461af88-fb8e-4d04-b225-9e5ba32f1e7a',58,100,'Marquis78','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(493,'7f2e4837-7f45-47b8-b19d-da5aed5da0d0',58,100,'Keely_Ernser','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(494,'bc3d0c46-64fb-4a72-845f-8a24e233eb99',58,100,'Zoila.Wehner49','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(495,'633f048c-dd95-4481-b869-e56deb817308',58,100,'Kara.Barrows95','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(496,'ce235f00-0033-409c-bfda-aa7d5e30833c',58,100,'Kole74','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(497,'7237bb62-d314-40cd-9bf0-ba448c94f8e3',58,100,'Jakob_Conn','TwitterHandle',_binary '','2023-01-21 06:42:26',NULL),(498,'fa5ef133-2e69-4d2e-a251-0feb974998d5',58,100,'Maureen.Schoen','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(499,'336d58b8-13b3-4fd0-9146-363f84ebe53e',58,100,'Ardith38','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(500,'4be2bcd8-6259-4d72-81b3-39598efd04a3',58,100,'Antwon.Wiza1','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(501,'5e666ea8-3936-4b09-943e-645a9a294f00',58,100,'Freda_Beahan','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(502,'a8266556-db3d-49e0-8fa8-00b892e09001',58,100,'Octavia.Simonis','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(503,'c539da84-b508-4bc9-aaa7-41043a689d41',58,100,'Chelsea_McKenzie','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(504,'eb46c524-1bd6-4fe4-9e86-9c19d5e76447',58,100,'Fredrick0','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(505,'5da5f105-9c06-4750-82db-8f77559e8891',58,100,'Triston.Hoppe1','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(506,'97dc34e4-11f9-4e55-97d4-f708a1bb412c',58,100,'Julian_Morar','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(507,'7fe7cdcb-22f7-40a7-b0ed-687a27376e9c',58,100,'Sadie.Wunsch46','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(508,'26b875c1-0e1d-4190-989b-8a96f9667bed',58,100,'Amya93','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(509,'5ee5a566-badb-4bf1-873b-4f84127a1c3d',58,100,'Cristina.Gutkowski','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(510,'223c2147-9a33-4cbd-9fb7-05a89bd65696',58,100,'Maud11','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(511,'8ba7926e-9c60-4c1a-934f-ef06d0f460b3',58,100,'Jorge.Erdman70','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(512,'8747c854-f0f6-40d3-8d6c-ec3203dd8066',58,100,'Icie.Douglas','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(513,'69b3f202-33ee-468c-830e-569eb2436eb0',58,100,'Lambert99','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(514,'43a0ef27-5f30-4f79-900a-f9e43304604c',58,100,'Rogelio_Kerluke90','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(515,'c26b3479-b139-429c-ac06-69e453942efa',58,100,'Rosemarie71','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(516,'7f625816-cf6c-4935-ad93-46fcb1cf5618',58,100,'Kacey.Rogahn18','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(517,'9aae82f1-dbdf-48c2-b388-107f587a2427',58,100,'Norris_Kilback55','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(518,'3ca5f296-48e5-4fad-aee1-c4460305de54',58,100,'Aiyana39','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(519,'f2795beb-0683-438a-9f44-58648d5a4e82',58,100,'Lindsay54','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(520,'b5e11e34-cf4a-4ff6-9c93-d599604e10e4',58,100,'Maia.Raynor','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(521,'677f03ac-dbb7-4e5d-bf74-d594b0e71019',58,100,'Lonny.Welch86','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(522,'3465f112-05c8-4d22-84ba-7b14db98d99e',58,100,'Lavern.OConnell68','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(523,'b248301b-e95a-4ff2-8cd2-84d0473a2e75',58,100,'Pat.Schneider31','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(524,'372f3fee-a448-42f0-a2b7-6af98bbdbb08',58,100,'Dylan_Gerlach','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(525,'2b3f8955-ad0a-4db4-986a-6d7b7225264e',58,100,'Gregorio.Turcotte','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(526,'27211d6a-0aea-4fbb-9b36-0cb6ab50d204',58,100,'Athena33','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(527,'51215688-edd1-44d3-938f-e1c7713f225f',58,100,'Milan85','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(528,'c1a40db1-8d9b-4fdb-976a-506f475a7dec',58,100,'Lennie11','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(529,'af26c918-a1b0-4b5f-b50b-4a3279b4667a',58,100,'Scot_Kuhic84','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(530,'5cda16b2-4cf1-42bd-a504-28a7d32288c5',58,100,'Arnoldo33','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(531,'2b81d5bb-810f-4b26-8eb1-1dbd989895f6',58,100,'Bettye.Doyle52','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(532,'d2c2601a-a880-4e23-8784-20a3e8dfd75f',58,100,'Savanah77','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(533,'d0079f7f-8281-46a0-b642-542f5e3594e2',58,100,'Travis83','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(534,'5c4b59bc-819f-47b9-abdc-24bd40c4a677',58,100,'Royal.Lockman93','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(535,'3b998147-cc1b-4107-a42c-fc96f401b5b8',58,100,'Alexys.Crist43','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(536,'f8aa344a-eb9c-4cfa-a7c3-626d9f8eaa90',58,100,'Libbie_Bashirian','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(537,'c9777e63-cd16-4971-a393-88fbf051a112',58,100,'Jabari31','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(538,'79455b94-b530-43ed-9a03-3952c30fec43',58,100,'Abbie_Kulas62','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(539,'d401a749-1eac-46f3-8323-bd9731ab623c',58,100,'Esmeralda88','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(540,'edf27bbe-a13e-47ba-9db4-eff25eecc0e0',58,100,'Alaina.Murazik50','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(541,'0b784901-3bc6-4a0a-a686-22e1ca8e45a1',58,100,'Annabelle.Batz','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(542,'1de69aec-bf18-445f-9c91-0cc01d96e9ab',58,100,'Annette86','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(543,'6d3f74f9-3fc0-4171-ad0c-75635ee04fba',58,100,'Tremaine_Labadie8','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(544,'f37bf56c-fefc-436e-a995-8e3e6242cbf8',58,100,'Howell83','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(545,'55a10913-49f3-4242-b998-1fc5873764dd',58,100,'Marjory_Parisian','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(546,'ef103984-0886-48da-a324-102fed9c339d',58,100,'Keanu_Beier49','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(547,'4153a5b2-8254-4735-8fa1-f4bac0198d4f',58,100,'Gonzalo13','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(548,'6cbc1034-e239-4278-bb58-fd93a7ae1b9b',58,100,'Conrad.Heller96','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(549,'e2099f44-b314-420d-aaa4-f99a8d06576e',58,100,'Gregoria47','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(550,'9541f604-d954-4de2-aab1-10cbeda450b8',58,100,'Johanna55','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(551,'e860b72e-d33d-448e-93b6-a6225341d68b',58,100,'Domenic.Bauch42','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(552,'d78ffef1-57f1-4f46-a27b-929c5b2ab8ec',58,100,'Barrett94','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(553,'2ec35f75-85a1-4c23-b5b6-b3dde1768d30',58,100,'Candace_Senger97','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(554,'d72ee0e4-1981-424b-b10d-7ee4658aeb97',58,100,'Nicolas.Hauck','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(555,'09c2b55a-08b3-46c7-aeb5-e8fcfc1ef71a',58,100,'Hanna.Ratke58','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(556,'210bd92a-2b47-4882-acfc-49a8661ca307',58,100,'Otto_Wolf','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(557,'6c041657-c654-4ff8-bd08-c4840d9584c0',58,100,'Telly17','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(558,'127a3bae-e278-48d7-93fc-e61ad48af81e',58,100,'Ivah79','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(559,'e2d94731-06b2-4e17-b115-b5c49526921d',58,100,'Rodrick96','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(560,'5ea0e6db-fd06-4124-ac53-8a7c5e0021d3',58,100,'Lucie.Heidenreich19','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(561,'a4ea4348-7693-4009-9f0f-0daeff7edc69',58,100,'Caterina.DAmore','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(562,'3397688c-6399-4764-9fa3-46f3c19ac4ab',58,100,'Gussie.Stiedemann75','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(563,'aeaf7053-a0fa-435f-be7c-4cf9cc54f027',58,100,'Helene.Romaguera6','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(564,'5a161759-0696-427c-b18b-f5488777cacd',58,100,'Dolly_Windler','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(565,'0d1363cf-b115-426d-b197-b5bebe2bc47d',58,100,'Kay.Renner51','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL),(566,'116bdc27-73fa-4cd2-a3f2-e5be04640802',58,100,'Jocelyn.Denesik31','TwitterHandle',_binary '','2023-01-21 06:42:27',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_entry_rules`
--

LOCK TABLES `sweep_entry_rules` WRITE;
/*!40000 ALTER TABLE `sweep_entry_rules` DISABLE KEYS */;
INSERT INTO `sweep_entry_rules` VALUES (9,'612e0fb3-cd69-4aca-b598-6f2cae2a0b38',29,15,'2022-05-15 13:16:29',NULL),(11,'180311c8-aa3b-425c-baa9-5f1d35d5149c',26,15,'2022-05-28 09:14:00','2022-07-10 13:38:11'),(12,'0ec93d71-67b6-4ec8-ad66-a3bd595dc508',27,15,'2022-07-09 17:02:18',NULL),(14,'133bbf97-70eb-4d49-b896-d00a4e3014fb',32,15,'2022-09-04 12:47:18','2022-09-04 12:55:49'),(15,'e95ac1f3-750e-4fcb-bbcd-6ffbec3a0c03',37,5,'2022-10-09 09:32:21',NULL),(16,'6dd8a95f-bb64-44ee-b424-db20b48ea19d',38,5,'2022-10-09 12:44:21',NULL),(18,'e183548b-95c5-40eb-b921-679200050b4a',46,5,'2022-10-23 10:12:14','2022-10-23 11:40:20'),(20,'84469a1b-2993-4373-8818-58a848db6aef',47,5,'2022-10-23 11:49:00',NULL),(21,'3347de2b-abba-4491-8006-0a8ecc99fa97',48,15,'2022-11-06 12:08:12','2022-11-06 12:08:32'),(22,'50b010c9-f554-4ed8-894b-fbfd1c89c713',49,15,'2022-11-11 12:06:25',NULL),(23,'86cee7dc-ea87-48b1-b3be-2f4a78ac1c87',53,5,'2022-11-11 15:27:04',NULL),(24,'317b8d65-f195-493f-8b13-cd2f782decf1',55,15,'2022-12-12 23:37:06',NULL),(25,'87c3d2d5-a21c-4818-a402-66e1413aefe2',56,4,'2022-12-17 07:52:02',NULL),(26,'7df0ef0b-cc1c-441f-80b0-73140940a31f',58,5,'2022-12-20 10:52:27',NULL),(27,'fd3466b8-c0fb-4259-9071-e36c4e613a1e',57,5,'2023-01-21 06:37:50',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_notifications`
--

LOCK TABLES `sweep_notifications` WRITE;
/*!40000 ALTER TABLE `sweep_notifications` DISABLE KEYS */;
INSERT INTO `sweep_notifications` VALUES (1,57,0,0,0,1,1,'2023-01-21 07:28:55','2023-01-21 07:57:29'),(3,58,0,0,0,1,1,'2023-01-21 07:28:55','2023-01-21 07:57:29');
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
) ENGINE=InnoDB AUTO_INCREMENT=66 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_prizes`
--

LOCK TABLES `sweep_prizes` WRITE;
/*!40000 ALTER TABLE `sweep_prizes` DISABLE KEYS */;
INSERT INTO `sweep_prizes` VALUES (13,'1e23f8ea-afc9-42bf-ba1d-2c58421629d8',1,26,_binary '','Prize 1 updated','Prize1 Description updated',599999.99,'English (Canada)','2022-06-11 18:36:31','2022-09-04 23:12:23'),(18,'400eaa04-97ae-401e-a099-bb2878913dfd',1,27,_binary '','Prize 1','Prize 1',20000.00,'English (Canada)','2022-06-26 09:58:54',NULL),(23,'8b870154-3cff-41b5-a8e4-68cb8936f86f',2,27,_binary '','Prize 2','Prize 2',5000.00,'English (Canada)','2022-07-03 23:22:12',NULL),(25,'88a6e1bb-9887-437d-834c-6f9738679220',2,26,_binary '','adfsad','sadf sadfasdf s',50000.00,'English (Canada)','2022-09-04 11:00:33',NULL),(26,'5cea6498-5d42-4ab0-a7ca-3c259953dc52',3,26,_binary '','sadf adsf','sdafdasf dasf ads fdasf',500000.00,'English (Canada)','2022-09-04 11:01:36',NULL),(36,'0b55a1cc-af0f-4eb2-bf74-9f6bcddeb248',1,47,_binary '','Prize 1','Prize 1 description',15000.00,'English (Canada)','2022-11-05 09:32:06',NULL),(37,'2de1fb0a-587a-4dcc-99b9-7a3b7dfa9430',4,47,_binary '','Prize 4','Prize 4 description',1000.00,'English (Canada)','2022-11-05 09:32:17',NULL),(38,'7edd6abb-aad8-481d-80da-0f8581145e56',2,47,_binary '','Prize 2','Prize 2 description',6000.00,'English (Canada)','2022-11-05 09:32:40',NULL),(39,'5455f8e9-2e19-4d10-93aa-91d3913d81dc',3,47,_binary '','Prize 3','Prize 3 description',3000.00,'English (Canada)','2022-11-05 09:32:51',NULL),(40,'971382eb-b9f7-4923-9af3-4c689f1a4133',1,48,_binary '','Prize 1','Prize 1',100000.00,'English (Canada)','2022-11-06 12:08:51',NULL),(41,'f70ff44e-4249-487e-aab7-1d53a8d78c19',2,48,_binary '','Prize 2','Prize 2',60000.00,'English (Canada)','2022-11-06 12:09:01',NULL),(50,'1c19f427-a092-41bd-b109-8fe041ad91f5',4,48,_binary '','Prize 4','Prize 4',20000.00,'English (Canada)','2022-11-06 12:21:09',NULL),(51,'23a9aa1e-5d8d-400a-9155-2b0a6389b99d',5,48,_binary '','Prize 5','Prize 5',7000.00,'English (Canada)','2022-11-06 12:21:25',NULL),(52,'c1c9c2dc-462a-4e4a-81dc-49b4c924d1d9',3,48,_binary '','Prize 3','Prize 3',30000.00,'English (Canada)','2022-11-06 12:21:53',NULL),(53,'207ddfe4-50cd-4255-b83b-e02e65ae71ed',1,49,_binary '','Prize 1','Prize 1',15000.00,'English (Canada)','2022-11-11 12:06:39',NULL),(54,'3b1b7ae8-3319-4ba8-a9f7-8dd6f7fe686a',2,49,_binary '','Prize 2','Prize 2',5000.00,'English (Canada)','2022-11-11 12:06:46',NULL),(56,'bc1b1c88-9148-44fc-9386-530d8c9ba008',2,53,_binary '','Prize 2','Prize 2',50000.00,'English (Canada)','2022-11-11 15:22:40',NULL),(57,'6a6174e7-1347-4d2b-a7f8-f8d33fd51542',1,53,_binary '','Prize 1','Prize 1',150000.00,'English (Canada)','2022-11-11 15:26:47',NULL),(58,'f9f25db1-5a35-49e8-9163-56faa040a145',1,55,_binary '','Prize 1','Prize 1',200000.00,'English (Canada)','2022-12-12 23:37:23',NULL),(59,'aa5e73b8-841a-4e92-876a-1e8a783b0640',2,55,_binary '','Prize 2','Prize 2',100000.00,'English (Canada)','2022-12-12 23:37:31',NULL),(60,'f9c6b9ee-6628-4ea3-b891-d79794cab5f4',3,55,_binary '','Prize 3','Prize 3',50000.00,'English (Canada)','2022-12-12 23:37:44',NULL),(61,'912002c1-af97-452b-bb5a-69a46aee3585',1,56,_binary '','Prize 1','Prize 1',15000.00,'English (Canada)','2022-12-17 07:52:16',NULL),(62,'35500543-9e4b-457b-99ef-f5f95561cc45',1,57,_binary '','Prize 1','Prize 1',20.00,'English (Canada)','2023-01-21 06:38:13',NULL),(63,'1133c73b-feea-4678-abaa-c2ce123a8f98',2,57,_binary '','Prize 2','Prize 2',10.00,'English (Canada)','2023-01-21 06:38:23',NULL),(64,'e0d0ef43-0f33-4275-be60-0c1617744f75',1,58,_binary '','Prize 1','Prize 1',30.00,'English (Canada)','2023-01-21 06:39:21',NULL),(65,'06582054-f593-44bc-a806-692e4812734c',2,58,_binary '','Prize 2','Prize 2',10.00,'English (Canada)','2023-01-21 06:39:31',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=102 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_rules`
--

LOCK TABLES `sweep_rules` WRITE;
/*!40000 ALTER TABLE `sweep_rules` DISABLE KEYS */;
INSERT INTO `sweep_rules` VALUES (100,'82688349-fab0-4e13-abb3-4a84a545f792',58,2,'#thisisatest',1,1,_binary '\0',_binary '',1616777644472770561,_binary '\0',NULL,'2023-01-21 06:40:02',NULL),(101,'e22793e1-75b3-439e-8515-c3f304b5c90e',57,2,'#Test1',1,1,_binary '\0',_binary '',1616777751238680576,_binary '\0',NULL,'2023-01-21 06:40:27',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners`
--

LOCK TABLES `sweep_winners` WRITE;
/*!40000 ALTER TABLE `sweep_winners` DISABLE KEYS */;
INSERT INTO `sweep_winners` VALUES (59,'e6ea93df-fbce-4c28-8ef1-5a3eb3529302',57,'bcbb3898-c168-475f-93f7-5543c44bba46',1,'Zackary.Mueller','TwitterHandle',1,0,NULL,'2023-01-21 07:57:29','2023-01-21 07:59:04'),(60,'1cc869c2-8621-4aab-931f-f233087f5fa5',57,'63d7d022-8219-4b56-9829-d8c58d50e337',2,'Unique_Veum83','TwitterHandle',1,0,NULL,'2023-01-21 07:57:29','2023-01-21 07:59:24'),(61,'653fed69-da9b-4609-8cda-287a2743f9b4',58,'d0079f7f-8281-46a0-b642-542f5e3594e2',1,'Travis83','TwitterHandle',0,1,64,'2023-01-21 07:57:29','2023-02-01 21:08:52'),(62,'30978917-c35f-41ac-b83a-2d2b7e21ebb7',58,'70be79f9-aed4-4c62-bd75-2530c636019e',2,'Nyasia.Lubowitz','TwitterHandle',1,1,65,'2023-01-21 07:57:29','2023-02-01 21:10:26');
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
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners_backup`
--

LOCK TABLES `sweep_winners_backup` WRITE;
/*!40000 ALTER TABLE `sweep_winners_backup` DISABLE KEYS */;
INSERT INTO `sweep_winners_backup` VALUES (62,59,57,'34ffe46c-d949-40a7-b7b6-adfacac0a96d',1,'Sadie.Wunsch46','TwitterHandle',1,'2023-01-21 07:57:29','2023-01-21 07:58:37'),(63,59,57,'697547d0-6b41-4ee8-bbab-ed5bd1d91e6f',2,'Katarina39','TwitterHandle',1,'2023-01-21 07:57:29','2023-01-21 07:58:52'),(64,59,57,'bcbb3898-c168-475f-93f7-5543c44bba46',3,'Zackary.Mueller','TwitterHandle',1,'2023-01-21 07:57:29','2023-01-21 07:59:04'),(65,60,57,'63d7d022-8219-4b56-9829-d8c58d50e337',1,'Unique_Veum83','TwitterHandle',1,'2023-01-21 07:57:29','2023-01-21 07:59:24'),(66,60,57,'78ccacc4-0e09-43ff-a4ea-eb9715ec5e1a',2,'Will_Ziemann','TwitterHandle',0,'2023-01-21 07:57:29',NULL),(67,60,57,'dbe787bf-a4d3-48b0-89a4-325ce984c0b5',3,'Elyse.Bergnaum26','TwitterHandle',0,'2023-01-21 07:57:29',NULL),(68,61,58,'d401a749-1eac-46f3-8323-bd9731ab623c',1,'Esmeralda88','TwitterHandle',0,'2023-01-21 07:57:29',NULL),(69,62,58,'70be79f9-aed4-4c62-bd75-2530c636019e',1,'Nyasia.Lubowitz','TwitterHandle',1,'2023-01-21 07:57:29','2023-02-01 21:09:04');
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
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sweep_winners_unfulfilled`
--

LOCK TABLES `sweep_winners_unfulfilled` WRITE;
/*!40000 ALTER TABLE `sweep_winners_unfulfilled` DISABLE KEYS */;
INSERT INTO `sweep_winners_unfulfilled` VALUES (57,'e6ea93df-fbce-4c28-8ef1-5a3eb3529302',57,'52aa1aff-c016-4896-929a-dbbb4eb9d9fc',1,'Zoila.Wehner49','TwitterHandle','This is a test promotion','2023-01-21 07:58:37',NULL),(58,'e6ea93df-fbce-4c28-8ef1-5a3eb3529302',57,'34ffe46c-d949-40a7-b7b6-adfacac0a96d',1,'Sadie.Wunsch46','TwitterHandle','Next winner','2023-01-21 07:58:52',NULL),(59,'e6ea93df-fbce-4c28-8ef1-5a3eb3529302',57,'697547d0-6b41-4ee8-bbab-ed5bd1d91e6f',1,'Katarina39','TwitterHandle','Zackary next','2023-01-21 07:59:04',NULL),(60,'1cc869c2-8621-4aab-931f-f233087f5fa5',57,'79206030-e84d-47a4-b0ee-b120e6eaab59',2,'Antwon.Wiza1','TwitterHandle','Unique should be winner','2023-01-21 07:59:24',NULL),(61,'30978917-c35f-41ac-b83a-2d2b7e21ebb7',58,'2660e6e6-d01c-4733-9ca2-317fdd49a170',2,'Shirley49','TwitterHandle','fdgdfg','2023-02-01 21:09:04',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=164 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_activities`
--

LOCK TABLES `user_activities` WRITE;
/*!40000 ALTER TABLE `user_activities` DISABLE KEYS */;
INSERT INTO `user_activities` VALUES (1,'74e6286f-2e2c-4cda-9a83-196c2838a57a',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-19 13:56:07','2022-12-19 19:56:07'),(2,'94719fc4-69d9-4599-98b1-1bb32233062f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-19 14:01:33','2022-12-19 20:01:33'),(3,'4a581e1b-67cb-4c6a-95d8-a7d938a0252c',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedOut','accounts','2022-12-19 14:01:36','2022-12-19 20:01:36'),(4,'303821de-54f6-414a-b9df-01ec0ebf44cd',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-20 08:09:52','2022-12-20 14:09:52'),(5,'f0d50c3c-2525-405b-b644-f5f3fbd85f83',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedOut','accounts','2022-12-20 08:09:56','2022-12-20 14:09:56'),(6,'72700c3b-daa5-46cf-88b9-a6c646ca7631',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-20 10:46:30','2022-12-20 16:46:30'),(7,'1a7267ed-2ddd-41c6-9242-166a749830fe',2,'d346f403-5f7e-4571-8ab4-25fc66e01b9b','CreatedPromotionByWizard','campaigns','2022-12-20 10:47:39','2022-12-20 16:47:39'),(8,'158fa515-d8b0-4a7c-8472-45303b54bd73',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','CreatedSweepstakes','sweep','2022-12-20 10:47:42','2022-12-20 16:47:42'),(9,'5d89214d-17d8-48cc-b474-402d9c1f0856',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-20 10:51:59','2022-12-20 16:51:59'),(10,'d7dae966-256f-427c-802e-8d32b3ea0741',2,'e8549356-6181-4010-ac4e-f99bad80c2de','CreatedSweepstakes','sweep','2022-12-20 10:52:16','2022-12-20 16:52:16'),(11,'909f20cb-413b-4f2c-a097-04b724b6d80b',2,'7b4a9f95-7f56-4f8c-9a4b-a7c5a60e96e5','AddedSweepstakesDrawRule','sweep','2022-12-20 10:52:22','2022-12-20 16:52:22'),(12,'6fcf2e90-d975-4f8d-ae1c-5c8daa080782',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-21 16:23:04','2022-12-21 22:23:04'),(13,'e70e2c12-1464-419c-9087-79b656eeef0a',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2022-12-21 16:25:44','2022-12-21 22:25:44'),(14,'961d710d-0af3-49c7-8530-2b5411f994f9',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2022-12-21 16:26:55','2022-12-21 22:26:55'),(15,'59fd4ba7-4b98-4007-b0c2-108bb129ec97',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-21 17:32:13','2022-12-21 23:32:13'),(16,'550f531a-1641-404f-bec0-abd7434e6021',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2022-12-21 17:32:58','2022-12-21 23:32:58'),(17,'5003c866-ac8e-4440-bd4c-6d0218353eb0',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2022-12-21 17:33:31','2022-12-21 23:33:31'),(18,'bd6d3fe5-caba-40e7-9c1a-cb68ed6bf0ce',2,'54e6c83a-ec04-4699-b2b9-5a5d8dfad8d5','CreatedPromotion','campaigns','2022-12-21 17:34:48','2022-12-21 23:34:48'),(19,'6e380abe-a23d-4516-be17-bf5c0f0922c2',2,'d489cbd2-cd3e-4fae-b750-5d5b8a9555bd','CreatedSweepstakes','sweep','2022-12-21 17:35:39','2022-12-21 23:35:39'),(20,'8556a9fc-7371-4137-b7f8-56752cfdeb2f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-29 08:53:24','2022-12-29 14:53:24'),(21,'ef1d0041-6af9-4ec5-8712-b136fe7009f1',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-29 09:04:02','2022-12-29 15:04:02'),(22,'4704da08-effd-4a7f-a39b-a1c12dd791fb',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2022-12-29 09:05:49','2022-12-29 15:05:49'),(23,'93282db4-e302-41dd-8b82-54b2aae0923e',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-29 09:05:59','2022-12-29 15:05:59'),(24,'d1249b10-b43b-4917-9191-da6e76ca5d4d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-29 09:07:09','2022-12-29 15:07:09'),(25,'55b4e9da-ce6e-49c4-8589-12c621b3d84f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-29 09:08:28','2022-12-29 15:08:28'),(26,'f99f6310-d29d-47c6-a41c-d5f15350bb63',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-29 23:00:16','2022-12-30 05:00:16'),(27,'cddde3a2-1c4b-4a98-ab5c-a9d4a031e742',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-30 08:17:12','2022-12-30 14:17:12'),(28,'a9eb1d14-862f-4e3c-9350-9fd07ee53d76',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-30 08:25:28','2022-12-30 14:25:28'),(29,'0e50feed-d462-42da-8ac8-e1f9e1f0647d',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-30 08:59:35','2022-12-30 14:59:35'),(30,'44740749-3347-488f-b578-d11ff54416c1',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 10:45:52','2022-12-30 16:45:52'),(31,'5cea4ca1-ae72-4bdf-8677-63f724073ec3',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-30 10:46:07','2022-12-30 16:46:07'),(32,'2e099f3d-78d0-4dfd-9809-81fdc257c260',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:03:04','2022-12-30 17:03:04'),(33,'f2be5bdc-e2f6-4430-8aa4-01fd4807c45f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:04:00','2022-12-30 17:04:00'),(34,'096d48a0-a19f-4042-8f39-f9bb1a2e3475',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:05:13','2022-12-30 17:05:13'),(35,'9178ff84-73c7-406c-80d2-bdc76968816c',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:13:07','2022-12-30 17:13:07'),(36,'29ee89bf-7ebe-4b50-ae8c-a131eea38aab',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:15:49','2022-12-30 17:15:49'),(37,'b8fbc7ac-58be-4121-8241-d9f800805210',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:33:21','2022-12-30 17:33:21'),(38,'1fc1ae18-a2d0-48d3-9a51-a26601d8cad4',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2022-12-30 11:36:45','2022-12-30 17:36:45'),(39,'9da3b44e-1993-4009-ba3d-1e1c08b9d92a',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-30 11:36:57','2022-12-30 17:36:57'),(40,'a9aff3ac-ea4d-4489-85fa-0f458e673fe7',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2022-12-30 11:40:29','2022-12-30 17:40:29'),(41,'2efbeab4-6444-4216-8b34-412b0391c1aa',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-30 11:40:35','2022-12-30 17:40:35'),(42,'7972fbeb-9435-4609-b870-0420efdd3b0e',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedOut','accounts','2022-12-30 12:03:09','2022-12-30 18:03:09'),(43,'edf82f3e-284d-44f7-a535-77bf0431a9fa',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2022-12-30 12:03:12','2022-12-30 18:03:12'),(44,'01879708-3b63-45e0-a59d-a91497bdd23d',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-31 23:40:40','2023-01-01 05:40:40'),(45,'86f3eb12-a626-46f0-b638-09db79555b9b',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2022-12-31 23:43:55','2023-01-01 05:43:55'),(46,'6a1cf7df-4102-4e58-aaf8-73b9d23240a9',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2023-01-02 11:52:57','2023-01-02 17:52:57'),(47,'60493cdc-01b5-4f74-b529-b2232615289e',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2023-01-02 11:53:03','2023-01-02 17:53:03'),(48,'12cef85d-adba-41e7-a40b-e35de6e14bb2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-02 11:53:14','2023-01-02 17:53:14'),(49,'94b5210a-09b5-4b14-bdd7-0d00c5e5d4da',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-02 11:58:58','2023-01-02 17:58:58'),(50,'d7d9ef72-4292-438d-af0c-a6aea552db6f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-02 12:10:41','2023-01-02 18:10:41'),(51,'88981179-2a01-4fb1-9d03-cdd7cebe645d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-02 12:14:37','2023-01-02 18:14:37'),(52,'b43e0fa7-f6be-4074-96aa-c016a47f7c31',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-02 12:14:57','2023-01-02 18:14:57'),(53,'88559e74-e570-40c9-b454-b5a81efc9087',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2023-01-03 23:09:26','2023-01-04 05:09:26'),(54,'96acebb7-ac04-44c6-b04e-2767f1dd482e',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2023-01-03 23:09:35','2023-01-04 05:09:35'),(55,'1cc468e4-6df0-4f51-ae9e-d32dde5a01b0',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-03 23:09:45','2023-01-04 05:09:45'),(56,'1bfb4a77-d98c-49b9-9e18-87d080a0fc20',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-03 23:11:32','2023-01-04 05:11:32'),(57,'e73fcd4f-c172-44c0-b015-5cfc50028973',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-03 23:52:24','2023-01-04 05:52:24'),(58,'98fdba08-d0d9-440a-9a3a-f3196666eafc',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-03 23:52:55','2023-01-04 05:52:55'),(59,'a9105109-bc79-4343-bc5f-ba44b060ca79',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-03 23:53:10','2023-01-04 05:53:10'),(60,'c3d2f9c7-6e02-47c6-8a0c-8248bc98f669',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-03 23:55:18','2023-01-04 05:55:18'),(61,'95722bc7-aaca-45ae-95cb-13e0e60e3c3d',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-03 23:55:30','2023-01-04 05:55:30'),(62,'45a14cfd-e2a9-4a73-b5b5-e1ededa4b193',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-03 23:55:40','2023-01-04 05:55:40'),(63,'589d12d6-03e4-49a4-b9ce-bf6021d955aa',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-03 23:56:15','2023-01-04 05:56:15'),(64,'c505d423-b668-425a-9705-0cd5ca9ee033',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 06:36:32','2023-01-21 12:36:32'),(65,'ff0450e2-42e5-471a-a3d1-beff26fe2f60',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2023-01-21 06:37:18','2023-01-21 12:37:18'),(66,'f77cc3d9-81c5-4172-be90-38583d313f66',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2023-01-21 06:37:46','2023-01-21 12:37:46'),(67,'16826ef7-928a-4e08-9e00-17a78bca8903',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-21 06:38:59','2023-01-21 12:38:59'),(68,'e8d64348-fb12-447e-9446-20246dd1f99b',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 06:43:10','2023-01-21 12:43:10'),(69,'cbb77d5b-a114-4fc6-9160-bb35f28d99ae',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 06:43:45','2023-01-21 12:43:45'),(70,'3883bece-7cd2-43be-9285-c1044090c3d7',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 06:57:09','2023-01-21 12:57:09'),(71,'2c0e0071-24b6-4d50-b326-e53b51dfca58',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 07:05:59','2023-01-21 13:05:59'),(72,'538bacc8-c04c-4887-a3e2-d429711737d1',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2023-01-21 07:10:39','2023-01-21 13:10:39'),(73,'92797d13-3249-4ea7-ae44-2f3a03ad5e31',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2023-01-21 07:12:17','2023-01-21 13:12:17'),(74,'f47dd73c-097c-4afa-bc67-9c514bf9f456',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 14:17:44','2023-01-21 13:17:44'),(75,'6277b334-ede8-4aa9-9405-07956be3cffd',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:19:48','2023-01-21 13:19:48'),(76,'5a32391e-a642-4ea4-af75-98ef5f44dd32',2,'7a92ab24-e482-4fab-bbb5-35d79a276f33','UpdatedSweepstakes','sweep','2023-01-21 13:27:02','2023-01-21 13:27:02'),(77,'6cb0a057-6938-4a56-82dd-1ebc5053b012',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:27:49','2023-01-21 13:27:49'),(78,'728d916c-1a3f-4c8a-a7e8-2effe36cafd7',2,'e8549356-6181-4010-ac4e-f99bad80c2de','UpdatedSweepstakes','sweep','2023-01-21 13:28:11','2023-01-21 13:28:11'),(79,'3e36a17d-d3cc-4922-b5a6-fe295c55279d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:36:16','2023-01-21 13:36:16'),(80,'86bab8ca-b902-4559-af25-049f0cbe6b32',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:38:27','2023-01-21 13:38:27'),(81,'f54c37f3-1099-487a-95e8-2675ec2a007a',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:42:04','2023-01-21 13:42:04'),(82,'4e6c85cb-c743-4148-aeda-34a960e989e8',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:46:32','2023-01-21 13:46:32'),(83,'be76806f-dee6-4528-9b65-8f8deee07c3c',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:49:50','2023-01-21 13:49:50'),(84,'481cbfc7-ac63-4079-953b-c4c9dec4e988',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-21 13:56:57','2023-01-21 13:56:57'),(85,'42c54785-df60-401d-9a2c-95ddab463225',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-22 17:40:10','2023-01-22 17:40:10'),(86,'55678bd4-a626-41e2-81ac-92e1dbd1394b',2,'842d3152-7815-443c-8331-4c04e041880f','CreatedPromotion','campaigns','2023-01-22 17:41:34','2023-01-22 17:41:34'),(87,'43c18a8d-f008-48ba-bd6e-72573f09f101',2,'c0eca944-6d6e-4cc6-b99c-476a9706f8cd','CreatedPromotion','campaigns','2023-01-22 17:42:39','2023-01-22 17:42:39'),(88,'6a95d3d3-656a-4ab0-a146-5956bba5e862',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-22 17:43:35','2023-01-22 17:43:35'),(89,'bcbc76fa-1bf9-4829-9418-ba4462845956',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-22 17:45:13','2023-01-22 17:45:13'),(90,'13aebc25-d4de-4ca2-836d-37e3bda4c914',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-22 17:51:38','2023-01-22 17:51:38'),(91,'1d205153-91fb-46af-9a2d-d1a0ff12c759',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-22 18:54:51','2023-01-22 18:54:51'),(92,'322408b1-7d9f-450a-8f26-921c9c8191d5',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-30 16:55:56','2023-01-30 22:55:56'),(93,'9fdde4e6-a235-4e47-b753-08b3cc489bd3',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-30 21:02:57','2023-01-31 03:02:57'),(94,'9b44799a-90b5-4a0b-a66e-d9f9912e06ac',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-30 21:05:07','2023-01-31 03:05:07'),(95,'739ec3b6-fa6e-45c7-8d9b-05a6b4301371',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-31 22:33:17','2023-02-01 04:33:17'),(96,'bf1cbceb-397a-4e8e-95c0-689a6ef8378e',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-31 22:41:32','2023-02-01 04:41:32'),(97,'d0b6bd19-8a74-41dc-810d-06d0390ee529',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-31 23:25:19','2023-02-01 05:25:19'),(98,'a3efc7a8-9f4e-4529-a69f-d81ed21daedb',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-01-31 23:27:51','2023-02-01 05:27:51'),(99,'8ae81ad8-bc84-4883-85cd-c8b38ddc73ae',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 05:25:00','2023-02-01 11:25:00'),(100,'6a8f43c2-31e2-4988-a64f-bab5c044e10b',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 05:37:20','2023-02-01 11:37:20'),(101,'74801ebc-757e-460c-b48a-108956061138',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 05:44:37','2023-02-01 11:44:37'),(102,'244358f2-5bdf-4700-8a96-9cb1b92a190a',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 15:21:49','2023-02-01 21:21:49'),(103,'dfe932e6-f45f-469b-b15e-256eb459e0d5',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 20:47:22','2023-02-02 02:47:22'),(104,'4925904e-a051-4f07-b2b8-00f4aa50c257',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 20:47:34','2023-02-02 02:47:34'),(105,'b3bb7fa2-5eb1-41d9-95f5-d399035f0bb5',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-01 21:08:01','2023-02-02 03:08:01'),(106,'9e127415-122f-424e-a2ad-ceedf02958ab',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-02 17:18:50','2023-02-02 23:18:50'),(107,'5d475d34-fb8b-44e7-b7c3-1605dc12d7aa',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-02 21:10:25','2023-02-03 03:10:25'),(108,'4398fcd4-3049-445e-92bf-21d6c1f1df34',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 12:28:08','2023-02-05 18:28:08'),(109,'2e212f60-711f-4b3f-a7b9-a00172da183f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 12:29:56','2023-02-05 18:29:56'),(110,'0d47ebee-5298-4eef-b289-103f0968bbf2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 12:30:54','2023-02-05 18:30:54'),(111,'e6b174b8-e254-4e6e-acee-cf2c283c4de0',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 12:35:10','2023-02-05 18:35:10'),(112,'ac37682f-713f-4287-8222-3a4027211051',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 12:39:25','2023-02-05 18:39:25'),(113,'2e86f02f-b84c-4258-82e2-4d21a86e0901',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 12:43:05','2023-02-05 18:43:05'),(114,'f14f591c-96b1-4099-ad99-3ac044cd2b65',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 14:39:47','2023-02-05 20:39:47'),(115,'0d5e26b8-3fe0-4ad9-9033-848d3f41a8dc',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-05 20:50:19','2023-02-06 02:50:19'),(116,'ddfa8283-ada1-46b9-a2b8-5f96126da9c0',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-06 08:13:41','2023-02-06 14:13:41'),(117,'632a7011-6a97-4b29-a997-bdf07caaef1d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-06 20:38:30','2023-02-07 02:38:30'),(118,'d32d92d2-fdb2-4179-9293-9f1deaf777d2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-06 21:10:36','2023-02-07 03:10:36'),(119,'db857d76-c964-46b8-a0db-21a9a5d815fa',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-06 21:23:15','2023-02-07 03:23:15'),(120,'c86e8261-96f3-40da-9220-5adf6ccc3e80',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-06 21:35:23','2023-02-07 03:35:23'),(121,'e518506b-6d91-4eb0-96f4-6b531033f6b6',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 06:07:50','2023-02-07 12:07:50'),(122,'3132cd74-0125-406b-a2ce-7de152b6982c',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:11:19','2023-02-08 03:11:19'),(123,'1eabadf7-1cf3-4bff-a786-e3e3de682307',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:12:02','2023-02-08 03:12:02'),(124,'303cee7a-1206-4658-beee-d4b03b74259d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:12:45','2023-02-08 03:12:45'),(125,'c3d7cae4-ea34-4254-886b-6fe52237c8f2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:13:57','2023-02-08 03:13:57'),(126,'b1aae0d1-b9aa-446f-a274-a80139a39fe4',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:17:40','2023-02-08 03:17:40'),(127,'0f59b061-a360-4d12-920d-ee2b4c3feb36',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:44:24','2023-02-08 03:44:24'),(128,'6fa5fbfd-13f8-4357-a332-13cae2211c20',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-07 21:45:28','2023-02-08 03:45:28'),(129,'0dfab8b4-56dd-4f7b-88dd-53f4ba20009d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 08:44:11','2023-02-08 14:44:11'),(130,'1357ed83-9d22-4384-a6b6-ab191571920e',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 08:49:45','2023-02-08 14:49:45'),(131,'cfeb11ce-b094-4626-8e9e-5e796a10053a',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 09:44:49','2023-02-08 15:44:49'),(132,'7b743606-5541-4bbb-be99-64f45cb34931',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 09:45:57','2023-02-08 15:45:57'),(133,'da5921c4-97ba-4d02-ae3e-e24bd895bc72',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 09:56:20','2023-02-08 15:56:20'),(134,'9c0948ee-c310-4e88-81ed-3f7c4687f533',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 10:02:53','2023-02-08 16:02:53'),(135,'9f3b2c6b-3cc9-4b40-9af1-26124f421a7d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 10:06:51','2023-02-08 16:06:51'),(136,'7ad49402-7b2a-4519-82e5-f5abe467eb10',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:11:13','2023-02-09 02:11:13'),(137,'efc2239b-8677-436d-bf54-84659164c33b',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:13:21','2023-02-09 02:13:21'),(138,'b01f48ce-f236-4c59-af45-72de8ddfc896',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:16:44','2023-02-09 02:16:44'),(139,'019d3161-651d-4a85-ba29-01932fbe2817',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:22:41','2023-02-09 02:22:41'),(140,'09efdc4a-7efc-45db-8930-84f5c38c655f',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:24:14','2023-02-09 02:24:14'),(141,'fb749642-9ca3-463f-beea-16d23a70c05f',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2023-02-08 20:41:24','2023-02-09 02:41:24'),(142,'40ba60ee-9c15-481b-a745-5d77981b9276',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2023-02-08 20:41:27','2023-02-09 02:41:27'),(143,'c4740e33-1e67-40cb-b2be-b6b210aa5cf2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:41:45','2023-02-09 02:41:45'),(144,'9a1b5126-a376-417b-a5cd-f3509c4ab572',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 20:45:24','2023-02-09 02:45:24'),(145,'9ddda056-f3d3-4974-a756-74b0794a00d2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-08 21:12:27','2023-02-09 03:12:27'),(146,'aa02a192-aa3d-4a13-a191-e0d533978fa5',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-09 05:48:27','2023-02-09 11:48:27'),(147,'213c29de-a996-4bb6-8a35-b1eddb2ce0df',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-09 05:53:46','2023-02-09 11:53:46'),(148,'d5900a70-c47d-4da8-b4cf-6f0675b7f021',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-09 16:30:52','2023-02-09 22:30:52'),(149,'a2e5c633-3400-4a04-8628-3d3e7fb4e580',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-09 17:42:52','2023-02-09 23:42:52'),(150,'ce48c0b0-c0a1-44ab-a851-cb80db1c00e9',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-09 17:50:44','2023-02-09 23:50:44'),(151,'76efbef1-7645-4971-8f5f-a3d4c6201ee2',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-10 05:55:52','2023-02-10 11:55:52'),(152,'f43e130f-6f94-459b-964d-7f39d62b5c1d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-10 07:31:16','2023-02-10 13:31:16'),(153,'dc483ca3-456d-493e-8217-dac8fc67be44',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-11 21:25:04','2023-02-12 03:25:04'),(154,'81b5beb1-456b-443e-9dc5-730b05a1e05d',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 06:49:54','2023-02-12 12:49:54'),(155,'94e3ad5f-3e14-4383-ac9e-c2e1eff136df',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedIn','accounts','2023-02-12 06:55:00','2023-02-12 12:55:00'),(156,'f19ab592-2b3d-42c9-b81d-d4279b7a26b6',1,'8ccb50dd-79cb-4fc9-a913-28043126d6d2','LoggedOut','accounts','2023-02-12 06:55:17','2023-02-12 12:55:17'),(157,'a8d67b74-cb78-465a-b8f2-9310eb50de02',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 06:55:30','2023-02-12 12:55:30'),(158,'e0e73a37-0d60-4e51-9b33-80fcd9c369d3',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 06:58:30','2023-02-12 12:58:30'),(159,'2c7a33bb-c43e-49af-bf5e-fc30b3793d64',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 06:59:27','2023-02-12 12:59:27'),(160,'8a056e59-013e-4ff4-a142-b89233926120',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 07:10:36','2023-02-12 13:10:36'),(161,'a496491b-15b2-48d1-95ff-b8df93e2f8f3',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 07:12:57','2023-02-12 13:12:57'),(162,'2d07aceb-b8af-417f-b5ed-6e0210f91f1e',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 08:09:10','2023-02-12 14:09:10'),(163,'5bffb0f9-f3da-4beb-bed5-fc87ffbe4109',2,'c96686d0-7942-4b89-8fa5-dc0b0755ad6c','LoggedIn','accounts','2023-02-12 13:37:47','2023-02-12 19:37:47');
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
-- Dumping routines for database 'hrvster'
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

-- Dump completed on 2023-02-13 20:26:16
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
) ENGINE=InnoDB AUTO_INCREMENT=410 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=2340 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account_subscriptions_history`
--

LOCK TABLES `account_subscriptions_history` WRITE;
/*!40000 ALTER TABLE `account_subscriptions_history` DISABLE KEYS */;
INSERT INTO `account_subscriptions_history` VALUES (1,'00000000-0000-0000-0000-000000000000',6336,'Subscribed',1,'2023-02-11 07:35:42','2023-02-11 13:35:42','2023-03-13 07:35:42','2023-03-13 13:35:42',0,0,'2023-02-11 07:35:41',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=6579 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=5694 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=1823 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `prize_id` int unsigned DEFAULT NULL,
  `is_winnable` tinyint NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`board_id`,`board_uid`),
  UNIQUE KEY `board_id_UNIQUE` (`board_id`),
  UNIQUE KEY `board_uid_UNIQUE` (`board_uid`),
  KEY `match_game_board_idx` (`match_game_id`),
  KEY `match_game_prize_idx` (`prize_id`),
  CONSTRAINT `match_game_board` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `match_game_prize_fk` FOREIGN KEY (`prize_id`) REFERENCES `match_prizes` (`prize_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=1232 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  CONSTRAINT `match_game_image` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=409 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `match_image_id` int unsigned NOT NULL,
  `prize_name` varchar(50) NOT NULL,
  `prize_description` varchar(100) DEFAULT NULL,
  `prize_value` decimal(18,2) NOT NULL,
  `created_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`prize_id`,`prize_uid`),
  UNIQUE KEY `prize_id_UNIQUE` (`prize_id`),
  UNIQUE KEY `prize_uid_UNIQUE` (`prize_uid`),
  KEY `match_game_prize_idx` (`match_game_id`),
  KEY `match_game_image_idx` (`match_image_id`),
  CONSTRAINT `match_game_image_prize` FOREIGN KEY (`match_image_id`) REFERENCES `match_images` (`image_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `match_game_prize` FOREIGN KEY (`match_game_id`) REFERENCES `match_games` (`match_game_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=360 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
  `prize_id` int unsigned DEFAULT NULL,
  `win_indicator` tinyint NOT NULL DEFAULT '0',
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
) ENGINE=InnoDB AUTO_INCREMENT=251 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
INSERT INTO `plans` VALUES (1,'e39503df-db14-4988-a9ad-adc6e938988e','Basic','A basic promotion plan',1,1,2,5000,50000,600000,'2022-12-11 00:00:00','2023-01-24 22:55:21'),(2,'f88e1f57-c2b9-47d0-95c4-3f8e93e619ed','Starter','For more seasoned members',0,1,3,12000,100000,1200000,'2022-12-11 00:00:00','2023-02-13 20:21:37'),(3,'8597b37d-99e4-403a-9b1c-7b75a5e78c50','Advanced','For more flexibility',0,0,5,30000,250000,3000000,'2022-12-11 00:00:00',NULL);
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
) ENGINE=InnoDB AUTO_INCREMENT=186 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=3817 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=326 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=7827 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=319 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=2134 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=475 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=893 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=901 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=1930 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=584 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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
) ENGINE=InnoDB AUTO_INCREMENT=5343 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
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

-- Dump completed on 2023-02-13 20:26:17
