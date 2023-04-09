using MySql.Data.MySqlClient;

public class Responses
{
    public int ResponseId { get; set; }
    public string Tag { get; set; }
    public double TotalScore { get; set; }
    public int TotalCount { get; set; }
}
