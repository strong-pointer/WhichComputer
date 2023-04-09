using MySql.Data.MySqlClient;

public class DatabaseRepository
{
    private readonly string connectionString;

    public DatabaseRepository()
    {
        this.connectionString = "Data Source=whichcomputer4720.cnhnorewhlzk.us-east-1.rds.amazonaws.com";
    }

    public void AddResponse(Responses response)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = "INSERT INTO Responses (tag, total_score, total_count) VALUES (@Tag, @TotalScore, @TotalCount)";
            connection.Execute(query, new { Tag = response.Tag, TotalScore = response.TotalScore, TotalCount = response.TotalCount });
        }
    }

    public void AddRating(Ratings rating)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = "INSERT INTO Ratings (computer_id, rating) VALUES (@ComputerId, @Rating)";
            connection.Execute(query, new { ComputerId = rating.ComputerId, Rating = rating.Rating });
        }
    }

    public void AddRecommendation(Recommendations recommendation)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = "INSERT INTO Recommendations (computer_id, rating) VALUES (@ComputerId, @Rating)";
            connection.Execute(query, new { ComputerId = recommendation.ComputerId, Rating = recommendation.Rating });
        }
    }

    public List<Responses> GetResponses()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = "SELECT * FROM Responses";
            return connection.Query<Responses>(query).ToList();
        }
    }

    public List<Ratings> GetRatings()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = "SELECT * FROM Ratings";
            return connection.Query<Ratings>(query).ToList();
        }
    }

    public List<Recommendations> GetRecommendations()
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var query = "SELECT * FROM Recommendations";
            return connection.Query<Recommendations>(query).ToList();
        }
    }
}
