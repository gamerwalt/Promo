USE `promo.harvester`;

CREATE TABLE `users` (
  `user_id` int unsigned NOT NULL AUTO_INCREMENT,
  `user_uid` varchar(50) NOT NULL,
  `external_id` varchar(100) NOT NULL,
  `profile_image_url` varchar(5000) NOT NULL,
  `verified_ind` bit(1) NOT NULL DEFAULT b'0',
  `location` varchar(150) NOT NULL,
  `description` varchar(500) NOT NULL,
  `screen_name` varchar(100) NOT NULL,
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `user_id_UNIQUE` (`user_id`),
  UNIQUE KEY `user_uid_UNIQUE` (`user_uid`),
  UNIQUE KEY `external_id_UNIQUE` (`external_id`)
) ENGINE=InnoDB AUTO_INCREMENT=69 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

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
) ENGINE=InnoDB AUTO_INCREMENT=78 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

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
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
