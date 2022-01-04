use `promo.sweepstakes`

CREATE TABLE `sweep` (
  `sweep_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_uid` varchar(50) NOT NULL,
  `campaign_uid` varchar(50) NOT NULL,
  `sweep_name` varchar(100) NOT NULL,
  `start_date` datetime NOT NULL,
  `end_date` datetime NOT NULL,
  `draw_date` datetime DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_id`),
  UNIQUE KEY `sweep_id_UNIQUE` (`sweep_id`),
  UNIQUE KEY `sweep_uid_UNIQUE` (`sweep_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `sweep_entry` (
  `sweep_entry_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_entry_uid` varchar(50) NOT NULL,
  `sweep_id` int unsigned NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
