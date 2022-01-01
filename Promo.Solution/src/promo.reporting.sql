CREATE DATABASE `promo.reporting` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

CREATE TABLE `sweep_entries` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
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
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
