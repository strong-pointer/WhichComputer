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

