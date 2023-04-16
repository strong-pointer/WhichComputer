using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WhichComputer.Main;
using YamlDotNet.Serialization;

namespace WhichComputer.Main
{
    public record struct Computer(string Name, List<string> ExcludeTags, List<Dictionary<string, object>> Tags, string Brand, List<string> AvailableFrom, List<string> Caveats, string Description, ComputerSpecs Specs)
    {
        internal string Model;

        public Dictionary<string, object>? GetTag(string identifier)
        {
            foreach (var tag in Tags)
            {
                if (tag.ContainsKey(identifier))
                {
                    return tag;
                }
            }

            return null;
        }

        public double GetTagValue(string identifier)
        {
            var tag = GetTag(identifier);
            double value;
            double.TryParse((string)tag[identifier], out value);
            return value;
        }
    }

    public record struct ComputerSpecs(List<ComputerOption> Options, [property: YamlMember(Alias = "OS", ApplyNamingConventions = false)] string OS, float Weight, string Resolution, float Screen, string Ports);

    public record struct ComputerOption(int Storage, [property: YamlMember(Alias = "RAM", ApplyNamingConventions = false)] int RAM, string Processor, int Cores, int Threads, string Graphics);

    public class ComputerList
    {
        public List<Computer> Computers = new();

        public ComputerList()
        {
        }

        public ComputerList(List<Computer> computers)
        {
            Computers = computers;
        }

        // Returns a single computer based on its name
        public Computer? GetComputer(string name)
        {
            return Computers.Find(computer => computer.Name == name);
        }

        // // Returns a list of computers with matching tags
        // public List<Computer> GetComputersWithTags(List<string> tags)
        // {
        //     return ComputersList.Where(computer => computer.Tags.Intersect(tags).Any()).ToList();
        // }
        //
        // // Returns a list of all unique tags from all computers
        // public List<string> GetAllTags()
        // {
        //     return ComputersList.SelectMany(computer => computer.Tags).Distinct().ToList();
        // }
    }
}
