use `promo.email`;

CREATE TABLE `emails` (
  `email_id` int unsigned NOT NULL AUTO_INCREMENT,
  `email_uid` varchar(50) NOT NULL,
  `email_address_to` varchar(400) NOT NULL,
  `parameters` varchar(8000) NOT NULL,
  `template_name` varchar(45) NOT NULL,
  `status` varchar(45) NOT NULL,
  `sent` bit(1) NOT NULL DEFAULT b'0',
  `created_date` datetime NOT NULL,
  `updated_date` datetime DEFAULT NULL,
  PRIMARY KEY (`email_id`),
  UNIQUE KEY `email_id_UNIQUE` (`email_id`),
  UNIQUE KEY `email_uid_UNIQUE` (`email_uid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
