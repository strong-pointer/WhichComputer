using MySql.Data.MySqlClient;

public class Ratings
{
    public int RatingId { get; set; }
    public int ComputerId { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
}
