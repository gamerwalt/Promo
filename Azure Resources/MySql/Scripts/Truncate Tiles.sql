START TRANSACTION;

SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE match_game_plays;  
TRUNCATE TABLE match_tiles;

UPDATE `hrvster`.`match_games` SET `tiles_generated` = '0', `is_ready_for_tile_generation` = '1' WHERE (`match_game_id` = 12) and (`match_game_uid` = 'e05a750f-ef9b-4410-8d69-f888b1e2a7c1');

SET FOREIGN_KEY_CHECKS = 1;

COMMIT;
