--liquibase formatted sql

--changeset seed_data:1
--preconditions onFail:MARK_RAN
--precondition-sql-check expectedResult:0 SELECT COUNT(*) FROM clubs WHERE name = 'Real Madrid'
INSERT INTO clubs (name, type, city, country, founded, created_at) VALUES
('Real Madrid', 1, 'Madrid', 'Spain', '1902-03-06', CURRENT_TIMESTAMP),
('FC Barcelona', 1, 'Barcelona', 'Spain', '1899-11-29', CURRENT_TIMESTAMP),
('Manchester United', 1, 'Manchester', 'England', '1878-01-01', CURRENT_TIMESTAMP),
('Bayern Munich', 1, 'Munich', 'Germany', '1900-02-27', CURRENT_TIMESTAMP),
('Paris Saint-Germain', 1, 'Paris', 'France', '1970-08-12', CURRENT_TIMESTAMP),
('Los Angeles Lakers', 2, 'Los Angeles', 'USA', '1947-01-01', CURRENT_TIMESTAMP),
('Chicago Bulls', 2, 'Chicago', 'USA', '1966-01-16', CURRENT_TIMESTAMP),
('Boston Celtics', 2, 'Boston', 'USA', '1946-06-06', CURRENT_TIMESTAMP);

