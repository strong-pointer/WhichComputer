using WhichComputer.Main.Models;

namespace WhichComputer.Main
{
    public class ScoringCalculation
    {
        public const double Threshold = 1.5;

        public static List<Computer> CalculateScore(QuestionnaireResponse responses, int resultsToReturn = 3)
        {
            int items = 0;
            List<Computer> valid = new();
            var loader = Program.GetComputerLoader().Computers;
            Dictionary<string, double> averages = responses.GetAllTagAverages();
            var nonExcluded = loader.Computers.Where(c => !averages.Keys.Any(k => c.ExcludeTags.Contains(k)));

            Dictionary<Computer, int> points = new();
            foreach (var computer in nonExcluded)
            {
                foreach (var entry in averages)
                {
                    if (computer.GetTag(entry.Key) != null && Math.Abs(entry.Value - computer.GetTagValue(entry.Key)) <= Threshold)
                    {
                        if (!points.ContainsKey(computer))
                        {
                            points.Add(computer, 1);
                        }
                        else
                        {
                            points[computer]++;
                        }
                    }
                }
            }

            valid = points.OrderByDescending(e => e.Value).Take(resultsToReturn).Select(p => p.Key).ToList();

            return valid;
        }

        public static FinalScore CalculateScore(InitialScoreTags initialScore)
        {
            FinalScore finalScore = new FinalScore();

            if (initialScore.Novice || initialScore.BrowserFocused || initialScore.SmallStorage || initialScore.LowUse)
            {
                if (!initialScore.NoChromebook || !initialScore.Gaming || !initialScore.Professional || !initialScore.Desktop)
                {
                    finalScore.ChromebookOK = true;
                }
                else
                {
                    finalScore.ChromebookOK = false;
                }
            }

            switch (initialScore.PriceRange)
            {
                case 1:
                    finalScore.PCTier = 1;
                    break;
                case 2:
                    finalScore.PCTier = 2;
                    break;
                case 3:
                    finalScore.PCTier = 2;
                    break;
                case 4:
                    finalScore.PCTier = 3;
                    break;
                case 5:
                    finalScore.PCTier = 3;
                    break;
                case 6:
                    finalScore.PCTier = 4;
                    break;
                case 7:
                    finalScore.PCTier = 4;
                    break;
                case 8:
                    finalScore.PCTier = 5;
                    break;
                case 9:
                    finalScore.PCTier = 6;
                    break;
                case 10:
                    finalScore.PCTier = 7;
                    break;
            }

            if (initialScore.Professional)
            {
                if (finalScore.PCTier <= 2)
                {
                    finalScore.PCTier = 3;
                }

                if (!initialScore.Office)
                {
                    if (finalScore.PCTier < 4)
                    {
                        finalScore.PCTier = 4;
                    }
                }
            }

            if (initialScore.Gaming)
            {
                switch (initialScore.Graphics)
                {
                    case 1:
                        if (finalScore.PCTier < 4)
                        {
                            finalScore.PCTier = 4;
                        }

                        break;
                    case 2:
                        if (finalScore.PCTier < 4)
                        {
                            finalScore.PCTier = 4;
                        }

                        break;
                    case 3:
                        if (finalScore.PCTier < 4)
                        {
                            finalScore.PCTier = 4;
                        }

                        break;
                    case 4:
                        if (finalScore.PCTier < 5)
                        {
                            finalScore.PCTier = 5;
                        }

                        break;
                    case 5:
                        if (finalScore.PCTier < 5)
                        {
                            finalScore.PCTier = 5;
                        }

                        break;
                    case 6:
                        if (finalScore.PCTier < 5)
                        {
                            finalScore.PCTier = 5;
                        }

                        break;
                    case 7:
                        if (finalScore.PCTier < 6)
                        {
                            finalScore.PCTier = 6;
                        }

                        break;
                    case 8:
                        if (finalScore.PCTier < 6)
                        {
                            finalScore.PCTier = 6;
                        }

                        break;
                    case 9:
                        if (finalScore.PCTier < 7)
                        {
                            finalScore.PCTier = 7;
                        }

                        break;
                    case 10:
                        if (finalScore.PCTier < 7)
                        {
                            finalScore.PCTier = 7;
                        }

                        break;
                }
            }

            if (initialScore.MediumStorage)
            {
                if (finalScore.PCTier < 2)
                {
                    finalScore.PCTier = 2;
                }
            }

            if (initialScore.HighStorage)
            {
                if (finalScore.PCTier < 3)
                {
                    finalScore.PCTier = 3;
                }
            }

            return finalScore;
        }
    }
}
