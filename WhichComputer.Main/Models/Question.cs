using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhichComputer.Main
{
    public class Question
    {
        public string? Prompt = string.Empty;
        public int Id;
        public string Explanation = string.Empty;
        public List<int> FollowUp = new();
        public List<Answer> Answers = new();

        public Answer? GetAnswer(string choice)
        {
            return Answers.Find(a => a.Choice.Equals(choice));
        }
    }
}