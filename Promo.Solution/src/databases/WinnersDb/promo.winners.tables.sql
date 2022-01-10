use `promo.winners`

CREATE TABLE `promo.winners`.`sweep_winners` (
  `sweep_winner_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `sweep_winner_uid` VARCHAR(50) NOT NULL,
  `sweep_entry_uid` VARCHAR(50) NOT NULL,
  `user_identifier` VARCHAR(100) NOT NULL,
  `user_identifier_type` VARCHAR(100) NOT NULL,
  `fulfilled_ind` BIT NOT NULL DEFAULT 0,
  `created_date` DATETIME NOT NULL,
  `updated_date` DATETIME NULL,
  PRIMARY KEY (`sweep_winner_id`),
  UNIQUE INDEX `sweep_winner_id_UNIQUE` (`sweep_winner_id` ASC) VISIBLE,
  UNIQUE INDEX `sweep_winner_uid_UNIQUE` (`sweep_winner_uid` ASC) VISIBLE);

CREATE TABLE `promo.winners`.`sweep_winners_backup` (
  `sweep_winner_backup_id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `sweep_winner_id` INT UNSIGNED NOT NULL,
  `sweep_entry_uid` VARCHAR(50) NOT NULL,
  `user_identifier` VARCHAR(100) NOT NULL,
  `user_identifier_type` VARCHAR(100) NOT NULL,
  `selected_ind` BIT NOT NULL DEFAULT 0,
  `created_date` DATETIME NOT NULL,
  `updated_date` DATETIME NULL,
  PRIMARY KEY (`sweep_winner_backup_id`),
  UNIQUE INDEX `sweep_winner_backup_id_UNIQUE` (`sweep_winner_backup_id` ASC) VISIBLE,
  INDEX `sweep_winners_backup_fk_idx` (`sweep_winner_id` ASC) VISIBLE,
  CONSTRAINT `sweep_winners_backup_fk`
    FOREIGN KEY (`sweep_winner_id`)
    REFERENCES `promo.winners`.`sweep_winners` (`sweep_winner_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);

ALTER TABLE `promo.winners`.`sweep_winners` 
ADD COLUMN `assigned_id` INT NOT NULL AFTER `sweep_entry_uid`;

ALTER TABLE `promo.winners`.`sweep_winners` 
ADD COLUMN `sweep_uid` VARCHAR(50) NOT NULL AFTER `sweep_winner_uid`;

ALTER TABLE `promo.winners`.`sweep_winners_backup` 
ADD COLUMN `sweep_uid` VARCHAR(50) NOT NULL AFTER `sweep_winner_id`;

ALTER TABLE `promo.winners`.`sweep_winners_backup` 
ADD COLUMN `assigned_id` INT NOT NULL AFTER `sweep_entry_uid`;
