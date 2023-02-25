using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WhichComputer.App_Start
{
    public class Computers
    {
        public List<Computer> ComputersList = new();

        // Returns a single computer based on its name
        public Computer? GetComputer(string name)
        {
            return ComputersList.Find(computer => computer.Name == name);
        }

        // Returns a list of computers with matching tags
        public List<Computer> GetComputersWithTags(List<string> tags)
        {
            return ComputersList.Where(computer => computer.Tags.Intersect(tags).Any()).ToList();
        }

        // Returns a list of all unique tags from all computers
        public List<string> GetAllTags()
        {
            return ComputersList.SelectMany(computer => computer.Tags).Distinct().ToList();
        }
    }
}
