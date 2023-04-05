using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WhichComputer.Main
{
    public class QuestionnaireLoader
    {
        public QuestionnaireLoader(string filePath)
        {
            Questions = LoadQuestionnaire(File.ReadAllText(filePath));
        }

        public static string LocalPath => "../data/questionnaire.yaml";

        public Questionnaire Questions { get; private set; }

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
