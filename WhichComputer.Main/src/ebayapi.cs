using System;
using eBay.Service.Core.Soap;
using eBay.Service.Call;

namespace WhichComputer
{
    class eBayAPI
    {
        static void Main(string[] args)
        {
            ApiContext apiContext = new ApiContext();
            apiContext.ApiCredential = new ApiCredential();
            apiContext.ApiCredential.eBayToken = "v^1.1#i^1#I^3#f^0#p^3#r^1#t^Ul4xMF8xMDo5MzlGODZCODgzRTU5REExNEY4QjBBNzY3RjlCOUNCM18zXzEjRV4yNjA=";
            apiContext.ApiCredential.AppId = "AieshaPa-whichcom-PRD-d756826fc-fafa7d06";
            apiContext.ApiCredential.DevId = "c40ac8de-96cf-4601-bd0b-21418807e2c6";
            apiContext.ApiCredential.CertId = "PRD-756826fccb48-e32f-4f1c-a73c-a0c4";
            apiContext.SoapApiServerUrl = "https://api.ebay.com/wsapi";

            // Create the GeteBayOfficialTime API call
            GeteBayOfficialTimeCall apiCall = new GeteBayOfficialTimeCall(apiContext);

            // Call the API and print the result
            Console.WriteLine(apiCall.GeteBayOfficialTime());
            Console.ReadLine();
        }
    }
}
