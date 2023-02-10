using WhichComputer.App_Start;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WhichComputer
{
    public class QuestionnaireLoader
    {

        public static String LOCAL_PATH = "data/questionnaire.yaml";
        public static Questionnaire LoadQuestionnaire(string yaml)
        {
            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

            Questionnaire data = deserializer.Deserialize<Questionnaire>(yaml);
            return data;
        }
    }
}
