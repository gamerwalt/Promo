UPDATE `hrvster`.`match_games` SET `ready_to_import_boards` = '0', `is_generated` = '0', `is_ready_for_tile_generation` = '0' 
WHERE (`match_game_id` = '14') and (`match_game_uid` = '38bdf394-1a63-46ae-8ca7-13d89150c887');

UPDATE `hrvster`.`match_games` SET `ready_to_import_boards` = '0', `is_generated` = '0', `tiles_generated` = '0' 
WHERE (`match_game_id` = '12') and (`match_game_uid` = 'e05a750f-ef9b-4410-8d69-f888b1e2a7c1');


UPDATE `hrvster`.`match_games` SET `tiles_generated` = '0', `is_ready_for_tile_generation` = '1' WHERE (`match_game_id` = '12') and (`match_game_uid` = 'e05a750f-ef9b-4410-8d69-f888b1e2a7c1');
