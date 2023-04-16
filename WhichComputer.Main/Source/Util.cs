using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WhichComputer.Main;

public static class Util
{
    public static Type GetHandlerForService(SupportedServices service)
    {
        switch (service)
        {
            case SupportedServices.EBAY:
                return typeof(EbayResultHandler);
            case SupportedServices.AMAZON:
                return typeof(AmazonComputerResultHandler);
            case SupportedServices.BEST_BUY:
                return typeof(BestBuyResultHandler);
            case SupportedServices.NEWEGG:
                return typeof(NeweggResultHandler);
        }

        throw new NotImplementedException();
    }
}