using System;
using System.Net;
using System.Xml;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;

namespace WhichComputer
{
    // ComputerResult interface or abstract class to store result information about a computer (price, model).
    public interface IComputerResult
    {
        string Model { get; set; }
        decimal Price { get; set; }
    }

    // eBayComputerResult class that implements IComputerResult.
    public class EBayComputerResult : IComputerResult
    {
        public string Model { get; set; }
        public decimal Price { get; set; }
        
        // Fetch function that accepts an enum as parameter with what type of site to search.
        public static EBayComputerResult Fetch(SiteCodeType siteCode, string query)
        {
            // Create an instance of the eBay API client.
            ApiContext context = new ApiContext();
            context.ApiCredential.eBayToken = "AieshaPa-whichcom-PRD-d756826fc-fafa7d06c40ac8de-96cf-4601-bd0b-21418807e2c6PRD-756826fccb48-e32f-4f1c-a73c-a0c4"; 
            context.SoapApiServerUrl = "https://api.ebay.com/wsapi";
            context.Site = siteCode;

            // Create a request to search for items on eBay.
            ItemType[] items;
            FindItemsAdvancedCall call = new FindItemsAdvancedCall(context);
            call.keywords = query;
            call.Execute();
            items = call.ApiResponse.SearchResult.ItemArray;

            // Parse the search results and return the first item as a ComputerResult object.
            if (items.Length > 0)
            {
                ItemType item = items[0];
                return new EBayComputerResult
                {
                    Model = item.Title,
                    Price = item.CurrentPrice.Value
                };
            }
            else
            {
                return null;
            }
        }
    }
}
