-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: localhost    Database: promo.harvester
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `account_create`(IN accountUid VARCHAR(50), IN emailAddress VARCHAR(500), IN phoneNumber VARCHAR(50), IN displayName VARCHAR(100))
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `blacklisted_sweeps_users_get`(IN sweepUid VARCHAR(50))
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `campaign_create`(IN accountUid VARCHAR(50), IN campaignName VARCHAR(100), IN campaignUid VARCHAR(50), IN campaignTypeCode VARCHAR(100), IN activeInd BIT, IN startDate DATETIME, IN endDate DATETIME)
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `campaign_update`(IN campaignUid VARCHAR(50), IN campaignName VARCHAR(100), IN campaignTypeCode VARCHAR(100), IN activeInd BIT, IN startDate DateTime, IN endDate DateTime)
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `sweep_prizes_create`(IN sweepUid VARCHAR(50), IN prizeUid VARCHAR(50), IN prizePosition INT, IN enabledInd BIT, IN prizeName VARCHAR(100), IN prizeValue DECIMAL(18, 2))
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `sweep_prizes_get_by_prize_uid`(IN prizeUid VARCHAR(50))
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `sweep_prizes_get_by_sweep_uid`(IN sweepUid VARCHAR(50))
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
CREATE DEFINER=`doadmin`@`%` PROCEDURE `tweets_get_harvested`(IN ruleUid VARCHAR(50))
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
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-01-29  9:33:34
