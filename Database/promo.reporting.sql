-- MySQL dump 10.13  Distrib 8.0.30, for Win64 (x86_64)
--
-- Host: localhost    Database: promo.reporting
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

-- Dump completed on 2023-01-29  9:41:04
