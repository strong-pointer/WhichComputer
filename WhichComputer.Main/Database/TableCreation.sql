CREATE TABLE Responses (
    response_id INT NOT NULL AUTO_INCREMENT,
    tag TEXT NOT NULL,
    total_score DOUBLE NOT NULL,
    total_count INT NOT NULL,
    PRIMARY KEY (response_id)
);

CREATE TABLE Ratings (
    rating_id INT NOT NULL AUTO_INCREMENT,
    computer_id INT NOT NULL,
    rating INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (rating_id)
);

CREATE TABLE Recommendations (
    reccomendations_id INT PRIMARY KEY,
    computer_id INT,
    rating INT
);

/* Hayden's Tweaks for Rating System */
CREATE SCHEMA Whichschema;
USE Whichschema;

CREATE TABLE responses (
	response_id INT AUTO_INCREMENT PRIMARY KEY,
    response_hash TEXT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ratings (
	rating_id INT AUTO_INCREMENT PRIMARY KEY,
    response_id INT NOT NULL,
    computer_name TEXT NOT NULL,
    rating DECIMAL(5, 2) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (response_id) REFERENCES responses(response_id)
);

