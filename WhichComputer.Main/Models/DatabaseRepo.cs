using MySql.Data.MySqlClient;
using WhichComputer.Main;

public class DatabaseRepository
{
    private readonly string _connectionString;

    public DatabaseRepository()
    {
        _connectionString = Program.Config.GetConnectionString("AWS");
    }

    public void AddResponse(Responses response)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var query = "INSERT INTO Responses (tag, total_score, total_count) VALUES (@Tag, @TotalScore, @TotalCount)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("Tag", response.Tag);
            command.Parameters.AddWithValue("TotalScore", response.TotalScore);
            command.Parameters.AddWithValue("TotalCount", response.TotalCount);
            command.ExecuteNonQuery();
        }
    }

    public void AddRating(Ratings rating)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var query = "INSERT INTO Ratings (computer_id, rating) VALUES (@ComputerId, @Rating)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("ComputerId", rating.ComputerId);
            command.Parameters.AddWithValue("Rating", rating.Rating);
            command.ExecuteNonQuery();
        }
    }

    public void AddRecommendation(Recommendations recommendation)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var query = "INSERT INTO Recommendations (computer_id, rating) VALUES (@ComputerId, @Rating)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("ComputerId", recommendation.ComputerId);
            command.Parameters.AddWithValue("Rating", recommendation.Rating);
            command.ExecuteNonQuery();
        }
    }

    public List<Responses> GetResponses()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Responses";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            throw new NotImplementedException();
        }
    }

    public List<Ratings> GetRatings()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Ratings";
            throw new NotImplementedException();
        }
    }

    public List<Recommendations> GetRecommendations()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            var query = "SELECT * FROM Recommendations";
            throw new NotImplementedException();
        }
    }
}
