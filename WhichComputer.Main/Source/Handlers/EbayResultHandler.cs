using Newtonsoft.Json;

namespace WhichComputer.Main
{
    public class EbayResultHandler : IComputerResultHandler
    {
        private string AppId = Program.Config.GetValue<string>("eBayKey");
        private ComputerLoader _computerLoader;

        public EbayResultHandler(ComputerLoader loader)
        {
            _computerLoader = loader;
        }

        public SupportedServices Service => SupportedServices.EBAY;

        public async Task<IEnumerable<ComputerResult>> Fetch(string computerName, bool used, int amount)
        {
            var url = "https://svcs.ebay.com/services/search/FindingService/v1"
                  + "?OPERATION-NAME=findItemsAdvanced"
                  + "&SERVICE-VERSION=1.0.0"
                  + "&SECURITY-APPNAME=" + AppId
                  + "&RESPONSE-DATA-FORMAT=JSON"
                  + "&REST-PAYLOAD"
                  + "&categoryId=175672"
                  + "&paginationInput.entriesPerPage=" + amount
                  + "&sortOrder=PricePlusShippingLowest"
                  + "&itemFilter(0).name=Condition"
                  + "&itemFilter(0).value=" + (used ? "3000" : "1000")
                  + "&keywords=" + Uri.EscapeDataString(computerName);

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to fetch results from eBay API: " + response.ReasonPhrase);
            }

            var content = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<dynamic>(content);

            var results = new List<ComputerResult>();

            foreach (var item in json["findItemsAdvancedResponse"][0]["searchResult"][0]["item"])
            {
                var result = new ComputerResult();
                result.ListingName = item["title"][0];
                result.Price = double.Parse((string)item["sellingStatus"][0]["currentPrice"][0]["__value__"]);
                result.Used = !item["condition"][0]["conditionDisplayName"][0].Equals("New");
                result.Url = item["viewItemURL"][0];
                result.Source = Service;
                results.Add(result);
            }

            return results;
        }

        public async Task<IEnumerable<ComputerResult>> Fetch(Computer computer, bool used, int amount)
        {
            return await Fetch(computer.Name, used, amount);
        }
    }
}
