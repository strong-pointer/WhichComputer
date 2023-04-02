namespace WhichComputer.Main.Models
{
    public class InitialScoreTags
    {

        public bool Novice { get; set; }

        public bool Intermediate { get; set; }

        public bool Expert { get; set; }

        public bool Laptop { get; set; }

        public bool Desktop { get; set; }

        public bool LowUse { get; set; }

        public bool Office { get; set; }

        public bool BrowserFocused { get; set; }

        public bool Minimal { get; set; }

        public bool Lightweight { get; set; }

        public bool HighUse { get; set; }

        public bool Professional { get; set; }

        public int Graphics { get; set; } // 1 - 10

        public bool Gaming { get; set; }

        public bool SmallStorage { get; set; }

        public bool MediumStorage { get; set; }

        public bool HighStorage { get; set; }

        public bool NoChromebook { get; set; }

        public int PriceRange { get; set; } // 1 - 7
    }
}
