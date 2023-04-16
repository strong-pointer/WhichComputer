using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace WhichComputer.Main;

public class NeweggResultHandler : IComputerResultHandler
{
    private const string BaseUrl = "https://www.newegg.com";
    private static ScrapingBrowser _browser = new();
    private string _file = string.Empty;
    private ComputerLoader _computerLoader;

    public NeweggResultHandler(ComputerLoader loader) => _computerLoader = loader;

    public NeweggResultHandler(ComputerLoader loader, string file)
    {
        _file = file;
        _computerLoader = loader;
        _browser.UserAgent = FakeUserAgents.Chrome;
    }

    public SupportedServices Service { get; } = SupportedServices.NEWEGG;

    public async Task<IEnumerable<ComputerResult>> Fetch(Computer computer, bool used, int amount = 5)
    {
        HtmlNode doc;
        Regex regex = new Regex(@"-?\d+(?:\.\d+)?");
        Match match = null;
        if (!_file.Equals(string.Empty))
        {
            var tmp = new HtmlDocument();
            tmp.Load(_file);
            doc = tmp.DocumentNode;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(computer.Name))
            {
                throw new InvalidOperationException("A computer cannot have an empty name.");
            }

            string condition = used ? "4016%204823" : "4814";
            string searchifiedName = computer.Name.Replace(" ", "+");

            var url = $"{BaseUrl}/p/pl?N={condition}&d={searchifiedName}";
            HtmlWeb web = new HtmlWeb();
            web.UserAgent = FakeUserAgents.Chrome.ToString();
            doc = web.Load(url).DocumentNode;
        }

        // The first item is simply the results header.
        var test = doc.CssSelect(".item-cell");
        List<ComputerResult> results = new();
        int total = 0;
        bool isFirstToSkip = true;
        foreach (var node in test)
        {
            if (total == amount)
                break;
            if (isFirstToSkip)
            {
                isFirstToSkip = false;
                continue;
            }

            try
            {
                // Ensures that the item is NOT a sponsored item. Messes w numbers and typically not the same product
                var isSponsored = node.CssSelect(".item-sponsored-box").FirstOrDefault();
                if (isSponsored != null)
                    continue;

                ComputerResult result = new ComputerResult();
                result.Computer = computer;
                result.Used = used;
                result.ListingName = node.CssSelect(".item-title").First().InnerText.Trim();
                match = regex.Match(node.CssSelect(".price-current").First().InnerText.Trim());
                result.Price = double.Parse(match.ToString());
                result.Url = node.CssSelect(".item-img").First().GetAttributeValue("href");
                result.Source = Service;
                results.Add(result);
                total++;
            }
            catch (InvalidOperationException ex)
            {
                // There is no price for this result - keep moving.
                continue;
            }
        }

        return results;
    }

    public Task<IEnumerable<ComputerResult>> Fetch(string computerName, bool used, int amount)
    {
        Computer? computer = _computerLoader.Computers?.GetComputer(computerName);
        if (string.IsNullOrWhiteSpace(computerName))
        {
            {
                throw new InvalidOperationException("A computer cannot have an empty name.");
            }
        }

        if (computer.HasValue)
        {
            return Fetch(computer.Value, used, amount);
        }

        throw new InvalidOperationException("No such computer exists in our database.");
    }
}