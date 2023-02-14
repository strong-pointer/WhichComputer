using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhichComputer.App_Start
{
    public class Questionnaire
    {
        public List<Question> Questions = new();
        public List<string> Tags = new();

        public Question? GetFirstQuestion()
        {
            return Questions.Find(question => question.Id == 1);
        }

        public List<Question> GetFollowUpQuestions(Answer answer)
        {
            return Questions.Where(q => answer.FollowUp.Contains(q.Id)).ToList();
        }
    }
}