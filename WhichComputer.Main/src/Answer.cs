namespace WhichComputer
{
    public class Answer
    {
        public string Choice = string.Empty;
        public Dictionary<string, double> Tags = new();
        public string Explanation = string.Empty;
        public List<int> FollowUp = new();
    }
}