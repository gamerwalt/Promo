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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-09-05 21:58:29
