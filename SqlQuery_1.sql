﻿CREATE TABLE player (
player_id int PRIMARY KEY,
player_name varchar(20),
player_password varchar(20),
player_position varchar(20));

INSERT INTO player (player_id, player_name, player_password) VALUES (10, '123f', '123a', '1235');

CREATE TABLE active_players (
player_id int PRIMARY KEY,
player_name varchar(20),
player_password varchar(20),
player_position varchar(20));

SELECT COUNT(*) FROM player;
SELECT COUNT(*) FROM player WHERE player_name='asdf' AND player_password='fdsa';