namespace WhichComputer.Main;

public static class Util
{
    public static Type GetHandlerForService(SupportedServices service)
    {
        switch (service)
        {
            case SupportedServices.EBAY:
                throw new NotImplementedException(); // TODO
            case SupportedServices.AMAZON:
                return typeof(AmazonComputerResultHandler);
        }

        throw new NotImplementedException();
    }
}