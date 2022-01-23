use `promo.rules`;

CREATE TABLE `watch_types` (
  `watch_type_id` int unsigned NOT NULL AUTO_INCREMENT,
  `watch_type_code` varchar(50) NOT NULL,
  `watch_type_name` varchar(100) NOT NULL,
  PRIMARY KEY (`watch_type_id`),
  UNIQUE KEY `watch_id_UNIQUE` (`watch_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `watch_type_campaign_types` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `watch_type_id` int unsigned NOT NULL,
  `campaign_type_uid` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `rules` (
  `rule_id` int unsigned NOT NULL AUTO_INCREMENT,
  `rule_uid` varchar(50) NOT NULL,
  `campaign_uid` varchar(50) NOT NULL,
  `sweep_uid` varchar(50) DEFAULT NULL,
  `watch_type_id` int unsigned NOT NULL,
  `watch_value` varchar(500) NOT NULL,
  `active_ind` bit(1) NOT NULL DEFAULT b'0',
  `tweet_id` bigint DEFAULT NULL,
  `added_for_tracking` bit(1) DEFAULT b'0',
  `date_added_for_tracking` datetime DEFAULT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`rule_id`),
  UNIQUE KEY `rule_id_UNIQUE` (`rule_id`),
  UNIQUE KEY `rule_uid_UNIQUE` (`rule_uid`),
  KEY `watch_types_rules_idx` (`watch_type_id`),
  CONSTRAINT `watch_types_rules` FOREIGN KEY (`watch_type_id`) REFERENCES `watch_types` (`watch_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


CREATE TABLE `sweep_draw_rules` (
  `sweep_draw_rule_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_draw_rule_uid` varchar(50) NOT NULL,
  `sweep_uid` varchar(50) NOT NULL,
  `winner_count` int NOT NULL,
  `winner_backup_count` int NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`sweep_draw_rule_id`),
  UNIQUE KEY `sweep_draw_rule_id_UNIQUE` (`sweep_draw_rule_id`),
  UNIQUE KEY `sweep_draw_rule_uid_UNIQUE` (`sweep_draw_rule_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `sweep_entry_rules` (
  `entry_rule_id` int unsigned NOT NULL AUTO_INCREMENT,
  `entry_rule_uid` varchar(50) NOT NULL,
  `campaign_uid` varchar(50) NOT NULL,
  `sweep_uid` varchar(50) NOT NULL,
  `entry_per_user` int NOT NULL DEFAULT '0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`entry_rule_id`),
  UNIQUE KEY `entry_rule_id_UNIQUE` (`entry_rule_id`),
  UNIQUE KEY `entry_rule_uid_UNIQUE` (`entry_rule_uid`) /*!80000 INVISIBLE */,
  UNIQUE KEY `entry_rule_unique_accross_campaigns` (`campaign_uid`,`sweep_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


LOCK TABLES `watch_types` WRITE;
/*!40000 ALTER TABLE `watch_types` DISABLE KEYS */;
INSERT INTO `watch_types` VALUES (1,'Tweet','A simple tweet'),(2,'Hashtag','A hashtag'),(3,'DM','A direct message');
/*!40000 ALTER TABLE `watch_types` ENABLE KEYS */;
UNLOCK TABLES;

LOCK TABLES `watch_type_campaign_types` WRITE;
/*!40000 ALTER TABLE `watch_type_campaign_types` DISABLE KEYS */;
INSERT INTO `watch_type_campaign_types` VALUES (1,1,'ad21aa80-6832-4713-b668-784d76fbdf11'),(2,1,'a1fcf285-96c9-4810-a9c5-eafed80f9d70'),(3,1,'5465b768-33a5-498a-8c8f-f8ff424e7098'),(4,2,'4d5b1fdc-763a-48dd-89c6-09fe075bf1ee'),(5,2,'a445f2c8-dc06-4367-8597-c550fc02d7e4'),(6,3,'27e64712-3663-45c6-a32f-3e7a598c3b09');
/*!40000 ALTER TABLE `watch_type_campaign_types` ENABLE KEYS */;
UNLOCK TABLES;