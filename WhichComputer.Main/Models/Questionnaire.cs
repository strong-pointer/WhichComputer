namespace WhichComputer.Main
{
    public class Questionnaire
    {
        public List<Question> Questions = new();
        public List<string> Tags = new();

        public Question? GetQuestionWithID(int id)
        {
            return Questions.Find(q => q.Id == id);
        }

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