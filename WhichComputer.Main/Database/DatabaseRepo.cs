using MySql.Data.MySqlClient;
using WhichComputer;
using WhichComputer.Main;

public class DatabaseRepository
{
    private readonly string _connectionString;

    public DatabaseRepository()
    {
        _connectionString = Program.Config.GetConnectionString("AWS");
    }

    public long AddResponse(QuestionnaireResponse response)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query = "INSERT INTO Responses (response_hash) VALUES (@response_hash)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("response_hash", response.GetHashedAndEncryptedResponse());
            command.ExecuteNonQuery();
            long responseID = command.LastInsertedId;
            MySqlCommand metricsUpload =
                new MySqlCommand(
                    "INSERT INTO QuestionnaireMetrics (tag, times_selected) VALUES (@tag, 1) ON DUPLICATE KEY UPDATE times_selected = times_selected + 1", connection);

            foreach (var tag in response.GetAllTags())
            {
                metricsUpload.Parameters.Clear();
                metricsUpload.Parameters.AddWithValue("tag", tag);
                metricsUpload.ExecuteNonQuery();
            }

            connection.Close();

            return responseID;
        }
    }

    public long AddRating(Ratings rating)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var query = "INSERT INTO Ratings (response_id, computer_name, rating) VALUES (@ResponseId, @ComputerName, @Rating)";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("ResponseId", rating.ResponseId);
            command.Parameters.AddWithValue("ComputerName", rating.ComputerName);
            command.Parameters.AddWithValue("Rating", rating.Rating);
            command.ExecuteNonQuery();

            connection.Close();

            return command.LastInsertedId;
        }
    }

    public List<QuestionnaireResponse> GetResponses()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            List<QuestionnaireResponse> allResponses = new();
            var query = "SELECT * FROM Responses";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                QuestionnaireResponse curr = QuestionnaireResponse.FromEncryptedHash(reader.GetString("response_hash"));
                curr.Id = reader.GetInt32("response_id");
                curr.Time = reader.GetDateTime("created_at");

                allResponses.Add(curr);
            }

            connection.Close();
            return allResponses;
        }
    }

    public List<Ratings> GetRatings()
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            List<Ratings> allRatings = new();
            var query = "SELECT * FROM Ratings";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Ratings curr = new Ratings();
                curr.RatingId = reader.GetInt32("rating_id");
                curr.ResponseId = reader.GetInt32("response_id");
                curr.ComputerName = reader.GetString("computer_name");
                curr.Rating = reader.GetDouble("rating");
                curr.CreatedAt = reader.GetDateTime("created_at");

                allRatings.Add(curr);
            }

            connection.Close();
            return allRatings;
        }
    }
}
