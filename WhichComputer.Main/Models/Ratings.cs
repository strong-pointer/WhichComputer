namespace WhichComputer.Main
{
    public class Ratings
    {
        public int RatingId { get; set; }

        public string ComputerName { get; set; }
        
        public int ResponseId { get; set; }

        public double Rating { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}