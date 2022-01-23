use `promo.winners`

CREATE TABLE `sweep_winner_prizes` (
  `sweep_winner_prize_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_prize_uid` varchar(50) NOT NULL,
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

CREATE TABLE `sweep_winners` (
  `sweep_winner_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_uid` varchar(50) NOT NULL,
  `sweep_uid` varchar(50) NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `sweep_winners_backup` (
  `sweep_winner_backup_id` int unsigned NOT NULL AUTO_INCREMENT,
  `sweep_winner_id` int unsigned NOT NULL,
  `sweep_uid` varchar(50) NOT NULL,
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
) ENGINE=InnoDB AUTO_INCREMENT=47 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
