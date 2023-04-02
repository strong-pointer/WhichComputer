using System.Security.Cryptography.X509Certificates;

namespace WhichComputer.Main.Models
{
    public class FinalScore
    {
        public int PCTier { get; set; } // Performance Rating 1 - 10

        public string? FormFactor { get; set; } // Laptop, Mini-PC, Desktop

        public bool ChromebookOK { get; set; } // False = No Chromebook, True = Chromebook Allowed
    }
}
