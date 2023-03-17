using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace WhichComputer.Main;

public class BestBuyResultHandler : IComputerResultHandler
{
    private const string BaseUrl = "https://www.bestbuy.com";
    private static ScrapingBrowser _browser = new();
    private string _file = string.Empty;
    private ComputerLoader _computerLoader;

    public BestBuyResultHandler(ComputerLoader loader) => _computerLoader = loader;

    public BestBuyResultHandler(ComputerLoader loader, string file)
    {
        _file = file;
        _computerLoader = loader;
        _browser.UserAgent = FakeUserAgents.Chrome;
    }

    public SupportedServices Service { get; } = SupportedServices.BEST_BUY;

    public IEnumerable<ComputerResult> Fetch(Computer computer, bool used, int amount)
    {
        HtmlNode doc;
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

            string condition = used ? "Refurbished" : "New";

            var url = $"{BaseUrl}/site/searchpage.jsp?qp=condition_facet%3DCondition~{condition}&st={computer.Name}";
            doc = new HtmlWeb().Load(url).DocumentNode;
        }

        // The first item is simply the results header.
        var test = doc.CssSelect(".sku-item");
        List<ComputerResult> results = new();
        int total = 0;
        foreach (var node in test)
        {
            if (total == amount)
                break;
            try
            {
                ComputerResult result = new ComputerResult();
                result.Computer = computer;
                result.Used = used;
                result.ListingName = node.CssSelect(".sku-title").First().InnerText.Trim();
                result.Price = double.Parse(Regex.Replace(node.CssSelect(".priceView-hero-price span").First().InnerText.Trim(), @"([a-zA-Z\$,])", string.Empty));
                result.Url = node.CssSelect(".sku-title a").First().GetAttributeValue("href");
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

    public IEnumerable<ComputerResult> Fetch(string computerName, bool used, int amount)
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