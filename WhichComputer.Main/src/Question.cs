using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhichComputer.App_Start
{
    public class Question
    {
        public string? Prompt;
        public int Id;
        public string Explanation = string.Empty;
        public List<int> FollowUp = new();
        public List<Answer> Answers = new();
    }
}