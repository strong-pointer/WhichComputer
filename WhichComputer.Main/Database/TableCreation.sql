CREATE TABLE Responses (
    response_id INT NOT NULL AUTO_INCREMENT,
    tag VARCHAR(255) NOT NULL,
    total_score DOUBLE NOT NULL,
    total_count INT NOT NULL,
    PRIMARY KEY (response_id)
);

CREATE TABLE Computers (
    computer_id INTEGER PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT NOT NULL,
    brand TEXT NOT NULL,
    available_from TEXT NOT NULL,
    caveats TEXT NOT NULL,
    weight FLOAT NOT NULL,
    resolution TEXT NOT NULL,
    screen FLOAT NOT NULL,
    ports TEXT NOT NULL,
    os TEXT NOT NULL,
    storage INTEGER NOT NULL,
    ram INTEGER NOT NULL,
    processor TEXT NOT NULL,
    cores INTEGER NOT NULL,
    threads INTEGER NOT NULL,
    graphics TEXT NOT NULL,
    tags TEXT NOT NULL
);

CREATE TABLE Ratings (
    rating_id INT NOT NULL AUTO_INCREMENT,
    computer_id INT NOT NULL,
    rating INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (rating_id),
    FOREIGN KEY (computer_id) REFERENCES Computers(computer_id)
);

CREATE TABLE Recommendations (
    reccomendations_id INT PRIMARY KEY,
    computer_id INT,
    rating INT,
    FOREIGN KEY (computer_id) REFERENCES Computers(computer_id)
);

