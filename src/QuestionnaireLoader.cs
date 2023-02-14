using System.Linq;
using WhichComputer.App_Start;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WhichComputer
{
    public class QuestionnaireLoader
    {
        private QuestionnaireLoader()
        {
            Questions = LoadQuestionnaire(File.ReadAllText("data/questionnaire.yaml"));
        }

        public static Questionnaire? Questions { get; private set; }

        public static QuestionnaireLoader Instance { get; } = new QuestionnaireLoader();

        private Questionnaire LoadQuestionnaire(string yaml)
        {
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

            Questionnaire data = deserializer.Deserialize<Questionnaire>(yaml);
            foreach (Question question in data.Questions)
            {
                bool sameFollowUp = question.FollowUp.Count != 0;
                question.Answers.ForEach(answer =>
                {
                    if (sameFollowUp)
                        answer.FollowUp = question.FollowUp;
                });
            }

            return data;
        }
    }
}
