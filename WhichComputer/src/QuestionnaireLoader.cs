using WhichComputer.App_Start;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WhichComputer
{
    public class QuestionnaireLoader
    {
        public static Questionnaire Questions { get; private set; }
        private QuestionnaireLoader()
        {
            Questions = LoadQuestionnaire(File.ReadAllText("data/questionnaire.yaml"));
        }
        private Questionnaire LoadQuestionnaire(string yaml)
        {
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

            Questionnaire data = deserializer.Deserialize<Questionnaire>(yaml);
            foreach (Question question in data.questions)
            {
                bool sameFollowUp = question.follow_up.Count != 0;
                question.answers.ForEach(answer =>
                {
                    if (sameFollowUp) answer.follow_up = question.follow_up;
                });
            }
            return data;
        }
        
        public static QuestionnaireLoader Instance { get; } = new QuestionnaireLoader();

    }
}
