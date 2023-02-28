using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace WhichComputer.Main;

public class ComputerLoader
{
    public ComputerLoader(string filePath)
    {
        Computers = LoadComputers(File.ReadAllText(filePath));
    }

    public static string LocalPath => "../data/computers.yaml";

    public ComputerList? Computers { get; private set; }

    private ComputerList LoadComputers(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

        var data = deserializer.Deserialize<ComputerList>(yaml);

        return data;
    }
}