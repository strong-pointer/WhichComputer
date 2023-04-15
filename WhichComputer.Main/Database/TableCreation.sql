CREATE SCHEMA IF NOT EXISTS whichschema;
USE whichschema;

CREATE TABLE IF NOT EXISTS Responses (
	response_id INT AUTO_INCREMENT PRIMARY KEY,
    response_hash TEXT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS Ratings (
	rating_id INT AUTO_INCREMENT PRIMARY KEY,
    response_id INT NOT NULL,
    computer_name TEXT NOT NULL,
    rating DECIMAL(5, 2) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (response_id) REFERENCES Responses(response_id)
);

CREATE TABLE IF NOT EXISTS QuestionnaireMetrics (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tag VARCHAR(255) UNIQUE NOT NULL,
    times_selected INT NOT NULL
);

