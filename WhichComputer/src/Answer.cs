using System.Collections.Generic;

namespace WhichComputer.App_Start
{
    public class Answer
    {
        public string choice;
        public Dictionary<String, double> tags = new();
        public string explanation = String.Empty;
        public List<int> follow_up = new();
    }
}