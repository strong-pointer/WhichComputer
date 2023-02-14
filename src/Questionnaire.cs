using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhichComputer.App_Start
{
    public class Questionnaire
    {
        public List<Question> questions = new();
        public List<string> tags = new();

        public Question? GetFirstQuestion()
        {
            return questions.Find(question => question.id == 1);
        }

        public List<Question> GetFollowUpQuestions(Answer answer)
        {
            return questions.Where(q => answer.follow_up.Contains(q.id)).ToList();
        }
    }
}