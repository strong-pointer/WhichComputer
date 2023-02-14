using System.Collections.Generic;

namespace WhichComputer.App_Start
{
    public class Answer
    {
        public string? Choice;
        public Dictionary<string, double> Tags = new();
        public string Explanation = string.Empty;
        public List<int> FollowUp = new();
    }
}