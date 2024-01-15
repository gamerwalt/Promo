
LOCK TABLES `campaign_types` WRITE;
/*!40000 ALTER TABLE `campaign_types` DISABLE KEYS */;
INSERT INTO `campaign_types` VALUES (1,'ad21aa80-6832-4713-b668-784d76fbdf11','AutoReplyTweet','Auto Reply Tweet','Campaign that only supports automatically replying to a specific Tweet id.',_binary '\0','2021-12-12 00:00:00',NULL),(2,'27e64712-3663-45c6-a32f-3e7a598c3b09','AutoReplyDM','Auto Reply DM','Campaign that only supports automatically replying to a specific DM Id.',_binary '\0','2021-12-12 00:00:00',NULL),(3,'4d5b1fdc-763a-48dd-89c6-09fe075bf1ee','AutoReplyHashtag','Auto Reply Hashtag','Campaign that only supports automatically replying to a specific Hashtag.',_binary '\0','2021-12-12 00:00:00',NULL),(4,'a1fcf285-96c9-4810-a9c5-eafed80f9d70','AutoReplyLikedTweet','Auto Reply Liked Tweet','Campaign that only supports automatically tweeting someone once they like a tweet.',_binary '\0','2021-12-12 00:00:00',NULL),(5,'5465b768-33a5-498a-8c8f-f8ff424e7098','AutoPlayFromTweet','Auto Play From Tweet','Campaign that supports automatically playing a game when someone replies, likes or interacts with a tweet.',_binary '\0','2021-12-12 00:00:00',NULL),(6,'a445f2c8-dc06-4367-8597-c550fc02d7e4','AutoPlayFromHashtag','Auto Play From Hashtag','Campaign that supports automatically playing a game when tweets a hashtag.',_binary '\0','2021-12-12 00:00:00',NULL),(7,'3b73d6c4-ebe3-4f68-9c73-845589d13d9f','AutoAll','Auto All','A la carte',_binary '\0','2021-12-12 00:00:00',NULL),(15,'3e5431d6-ba17-41f4-8f21-fd2d70245845','AutoPlayFromTweetReplyHashtag','Auto Play From Tweet Reply Hashtag','Campaign that supports automatically playing a game when a user replies a tweet with a specific hashtag.',_binary '','2021-12-12 00:00:00',NULL);
/*!40000 ALTER TABLE `campaign_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `flutterwave_plans`
--

LOCK TABLES `flutterwave_plans` WRITE;
/*!40000 ALTER TABLE `flutterwave_plans` DISABLE KEYS */;
INSERT INTO `flutterwave_plans` VALUES (1,'23a97b0e-8861-4e93-a729-90a89d479801',1,'https://sandbox-flw-web-v3.herokuapp.com/pay/basicdailycharge',NULL),(2,'23a97b0e-8861-4e93-a729-90a89d479802',2,'https://sandbox-flw-web-v3.herokuapp.com/pay/startermonthlypayment',NULL),(3,'23a97b0e-8861-4e93-a729-90a89d479803',3,'https://sandbox-flw-web-v3.herokuapp.com/pay/advancedmonthlypayment',NULL);
/*!40000 ALTER TABLE `flutterwave_plans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `payment_services`
--

LOCK TABLES `payment_services` WRITE;
/*!40000 ALTER TABLE `payment_services` DISABLE KEYS */;
INSERT INTO `payment_services` VALUES (1,'85cb591f-18e6-4cf3-b2e0-f60a840e0e51','Flutterwave','FlutterwavePaymentService',1,'2022-12-29 00:00:00','2022-12-29 08:45:55'),(2,'85cb591f-18e6-4cf3-b2e0-f60a840e0e52','Paystack','PaystackPaymentService',0,'2022-12-29 00:00:00','2022-12-29 08:48:01');
/*!40000 ALTER TABLE `payment_services` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `plans`
--

LOCK TABLES `plans` WRITE;
/*!40000 ALTER TABLE `plans` DISABLE KEYS */;
INSERT INTO `plans` VALUES (1,'e39503df-db14-4988-a9ad-adc6e938988e','Basic','A basic promotion plan',1,0,2,5000,50000,600000,'2022-12-11 00:00:00',NULL),(2,'f88e1f57-c2b9-47d0-95c4-3f8e93e619ed','Starter','For more seasoned members',0,0,5,12000,100000,1200000,'2022-12-11 00:00:00',NULL),(3,'8597b37d-99e4-403a-9b1c-7b75a5e78c50','Advanced','For more flexibility',0,0,0,30000,250000,3000000,'2022-12-11 00:00:00',NULL);
/*!40000 ALTER TABLE `plans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `profile_types`
--

LOCK TABLES `profile_types` WRITE;
/*!40000 ALTER TABLE `profile_types` DISABLE KEYS */;
INSERT INTO `profile_types` VALUES (1,'1b3b9e77-6a8c-42be-8266-e0d116ac203e','Twitter'),(2,'2b3b9e77-6a8c-42be-8266-e0d116ac203f','Facebook');
/*!40000 ALTER TABLE `profile_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `watch_type_campaign_types`
--

LOCK TABLES `watch_type_campaign_types` WRITE;
/*!40000 ALTER TABLE `watch_type_campaign_types` DISABLE KEYS */;
INSERT INTO `watch_type_campaign_types` VALUES (7,1,1),(8,1,4),(9,1,5),(10,2,3),(11,2,6),(12,3,2);
/*!40000 ALTER TABLE `watch_type_campaign_types` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `watch_type_providers`
--

LOCK TABLES `watch_type_providers` WRITE;
/*!40000 ALTER TABLE `watch_type_providers` DISABLE KEYS */;
INSERT INTO `watch_type_providers` VALUES (1,'0718b2e4-6a86-485e-a9b8-525097eabdc3','TWITTER','Twitter',1,NULL),(2,'eb9c0884-9ebc-4849-ada5-bbd2a0ca9593','FB','Facebook',0,NULL),(3,'1d45cc9d-f714-4a8e-8a2d-03e7204461c7','INSTAGRAM','Instagram',0,NULL);
/*!40000 ALTER TABLE `watch_type_providers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `watch_types`
--

LOCK TABLES `watch_types` WRITE;
/*!40000 ALTER TABLE `watch_types` DISABLE KEYS */;
INSERT INTO `watch_types` VALUES (1,'Tweet','A simple tweet',0),(2,'Hashtag','A hashtag',1),(3,'DM','A direct message',0);
/*!40000 ALTER TABLE `watch_types` ENABLE KEYS */;
UNLOCK TABLES;