using System.Xml.XPath;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;

namespace WhichComputer.Main;

public class AmazonComputerResultHandler : IComputerResultHandler
{
    private const string BaseUrl = "https://www.amazon.com";
    private static ScrapingBrowser _browser = new();
    private string _file = string.Empty;
    private ComputerLoader _computerLoader;

    public AmazonComputerResultHandler(ComputerLoader loader)
    {
        _computerLoader = loader;
    }

    public AmazonComputerResultHandler(ComputerLoader loader, string file)
    {
        _file = file;
        _computerLoader = loader;
    }

    public SupportedServices Service { get; } = SupportedServices.AMAZON;

    public IEnumerable<ComputerResult> Fetch(Computer computer, bool used, int amount = 5)
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

            var url = BaseUrl + "/s?k=" + computer.Name;
            if (used)
            {
                // This is the query parameter for the "Renewed" status on Amazon's website.
                url += "&rh=n:172282,p_n_condition-type:16907720011&ref=sr_nr_p_n_condition-type_2";
            }

            doc = _browser.NavigateToPage(new Uri(url)).Html;
        }

        // The first item is simply the results header.
        var test = doc.CssSelect(".s-result-item.sg-col-16-of-20").Where(n =>
            !n.InnerText.Contains("RESULTS") && !n.InnerText.Trim().Equals(string.Empty));
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
                result.ListingName = node.CssSelect(".a-text-normal.a-size-medium.a-color-base").First().InnerText.ReplaceLineEndings(string.Empty);
                result.Price = double.Parse(node.CssSelect(".a-offscreen").First().InnerText.Substring(1));
                result.Url = node.CssSelect("a").First().GetAttributeValue("href");
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
        Computer? computer = _computerLoader.Computers.GetComputer(computerName);
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
        else
        {
            {
                throw new InvalidOperationException("No such computer exists in our database.");
            }
        }
    }
}