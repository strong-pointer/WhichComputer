using System;
using System.Collections.Generic;
using eBay.Service.Core.Soap;
using eBay.Service.Call;

namespace WhichComputer.Main
{
    class eBayAPI : IComputerResultHandler
    {
        public SupportedServices Service => SupportedServices.eBay;

        public IEnumerable<ComputerResult> Fetch(Computer computer, bool used, int amount)
        {
            ApiContext apiContext = new ApiContext();
            apiContext.ApiCredential = new ApiCredential();
            apiContext.ApiCredential.eBayToken = "v^1.1#i^1#I^3#f^0#p^3#r^1#t^Ul4xMF8xMDo5MzlGODZCODgzRTU5REExNEY4QjBBNzY3RjlCOUNCM18zXzEjRV4yNjA=";
            apiContext.ApiCredential.AppId = "AieshaPa-whichcom-PRD-d756826fc-fafa7d06";
            apiContext.ApiCredential.DevId = "c40ac8de-96cf-4601-bd0b-21418807e2c6";
            apiContext.ApiCredential.CertId = "PRD-756826fccb48-e32f-4f1c-a73c-a0c4";
            apiContext.SoapApiServerUrl = "https://api.ebay.com/wsapi";

            FindItemsAdvancedCall apiCall = new FindItemsAdvancedCall(apiContext);
            apiCall.Keywords = computer.Brand + " " + computer.Model;
            if (used)
            {
                apiCall.ConditionIds = new int[] { 3000 };
            }
            apiCall.Sort = 2; // sort by lowest price

            // pages
            apiCall.Pagination = new PaginationType();
            apiCall.Pagination.EntriesPerPage = amount;
            apiCall.Pagination.PageNumber = 1;


            var results = new List<ComputerResult>();
            var response = apiCall.Execute();
            foreach (var item in response.SearchResult.Item)
            {
                var result = new ComputerResult();
                result.Title = item.Title;
                result.Price = (decimal)item.SellingStatus.CurrentPrice.Value;
                result.Currency = item.SellingStatus.CurrentPrice.CurrencyID.ToString();
                result.Condition = item.ConditionDisplayName;
                result.Url = item.ViewItemURLForNaturalSearch;
                results.Add(result);
            }
            return results;
        }

        public IEnumerable<ComputerResult> Fetch(string computerName, bool used, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
