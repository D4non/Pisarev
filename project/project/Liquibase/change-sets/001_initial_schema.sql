--liquibase formatted sql

--changeset initial_schema:1
CREATE TABLE IF NOT EXISTS users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username VARCHAR(100) NOT NULL UNIQUE,
    email VARCHAR(200) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    role VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

--changeset initial_schema:2
CREATE TABLE IF NOT EXISTS api_keys (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    key VARCHAR(200) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    expires_at TIMESTAMP NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

--changeset initial_schema:3
CREATE TABLE IF NOT EXISTS clubs (
    id SERIAL PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    type INTEGER NOT NULL,
    city VARCHAR(100) NOT NULL,
    country VARCHAR(100) NOT NULL,
    founded DATE NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

CREATE INDEX idx_clubs_name ON clubs(name);

--changeset initial_schema:4
CREATE TABLE IF NOT EXISTS stadiums (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(200) NOT NULL,
    city VARCHAR(100) NOT NULL,
    capacity INTEGER NOT NULL,
    club_id INTEGER,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    CONSTRAINT fk_stadiums_clubs FOREIGN KEY (club_id) REFERENCES clubs(id) ON DELETE SET NULL
);

--changeset initial_schema:5
CREATE TABLE IF NOT EXISTS players (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    date_of_birth DATE NOT NULL,
    position VARCHAR(50) NOT NULL,
    jersey_number INTEGER NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

--changeset initial_schema:6
CREATE TABLE IF NOT EXISTS coaches (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    date_of_birth DATE NOT NULL,
    license_type VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP
);

--changeset initial_schema:7
CREATE TABLE IF NOT EXISTS matches (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    home_club_id INTEGER NOT NULL,
    away_club_id INTEGER NOT NULL,
    stadium_id UUID,
    match_date TIMESTAMP NOT NULL,
    home_score INTEGER,
    away_score INTEGER,
    status INTEGER NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP,
    CONSTRAINT fk_matches_home_club FOREIGN KEY (home_club_id) REFERENCES clubs(id) ON DELETE RESTRICT,
    CONSTRAINT fk_matches_away_club FOREIGN KEY (away_club_id) REFERENCES clubs(id) ON DELETE RESTRICT,
    CONSTRAINT fk_matches_stadium FOREIGN KEY (stadium_id) REFERENCES stadiums(id) ON DELETE SET NULL
);

--changeset initial_schema:8
CREATE TABLE IF NOT EXISTS club_players (
    club_id INTEGER NOT NULL,
    player_id UUID NOT NULL,
    joined_date DATE NOT NULL,
    left_date DATE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (club_id, player_id),
    CONSTRAINT fk_club_players_club FOREIGN KEY (club_id) REFERENCES clubs(id) ON DELETE CASCADE,
    CONSTRAINT fk_club_players_player FOREIGN KEY (player_id) REFERENCES players(id) ON DELETE CASCADE
);

--changeset initial_schema:9
CREATE TABLE IF NOT EXISTS club_coaches (
    club_id INTEGER NOT NULL,
    coach_id UUID NOT NULL,
    started_date DATE NOT NULL,
    ended_date DATE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    PRIMARY KEY (club_id, coach_id),
    CONSTRAINT fk_club_coaches_club FOREIGN KEY (club_id) REFERENCES clubs(id) ON DELETE CASCADE,
    CONSTRAINT fk_club_coaches_coach FOREIGN KEY (coach_id) REFERENCES coaches(id) ON DELETE CASCADE
);

--changeset initial_schema:10
CREATE TABLE IF NOT EXISTS player_matches (
    player_id UUID NOT NULL,
    match_id UUID NOT NULL,
    points INTEGER,
    assists INTEGER,
    goals INTEGER,
    rebounds INTEGER,
    minutes_played INTEGER NOT NULL DEFAULT 0,
    PRIMARY KEY (player_id, match_id),
    CONSTRAINT fk_player_matches_player FOREIGN KEY (player_id) REFERENCES players(id) ON DELETE CASCADE,
    CONSTRAINT fk_player_matches_match FOREIGN KEY (match_id) REFERENCES matches(id) ON DELETE CASCADE
);

