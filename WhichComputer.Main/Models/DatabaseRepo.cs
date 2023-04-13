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
            
            connection.Close();
            
            return command.LastInsertedId;
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
            List<QuestionnaireResponse> allResponses = new List<QuestionnaireResponse>();
            var query = "SELECT * FROM Responses";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                QuestionnaireResponse curr = QuestionnaireResponse.FromEncryptedHash(reader.GetString("response_hash"));
                curr.id = reader.GetInt32("response_id");
                curr.time = reader.GetDateTime("created_at");

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
            var query = "SELECT * FROM Ratings";
            throw new NotImplementedException();
        }
    }
}
