using MySql.Data.MySqlClient;

public class Recommendations
{
    public int RecommendationsId { get; set; }
    public int ComputerId { get; set; }
    public int Rating { get; set; }
}
