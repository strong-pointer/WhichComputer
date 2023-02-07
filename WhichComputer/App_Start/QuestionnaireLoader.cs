using System;
using System.IO;
using System.Web;
using WhichComputer.App_Start;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WhichComputer
{
    public class QuestionnaireLoader
    {

        public static String LOCAL_PATH = "../App_Data/questionnaire.yaml";
        public static String ASP_PATH = File.ReadAllText(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data/questionnaire.yaml"));
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
