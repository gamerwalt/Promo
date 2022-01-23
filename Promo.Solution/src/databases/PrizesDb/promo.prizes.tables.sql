CREATE TABLE `sweep_prizes` (
  `prize_id` int unsigned NOT NULL AUTO_INCREMENT,
  `prize_uid` varchar(50) NOT NULL,
  `prize_position` int NOT NULL,
  `sweep_uid` varchar(50) NOT NULL,
  `enabled_ind` bit(1) NOT NULL DEFAULT b'0',
  `prize_name` varchar(100) NOT NULL,
  `prize_description` varchar(5000) DEFAULT NULL,
  `prize_value` decimal(18,2) NOT NULL,
  `locale` varchar(50) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`prize_id`),
  UNIQUE KEY `prize_id_UNIQUE` (`prize_id`),
  UNIQUE KEY `prize_uid_UNIQUE` (`prize_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
