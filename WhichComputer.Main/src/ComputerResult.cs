namespace WhichComputer.Main;
public record class ComputerResult
{
    public ComputerResult()
    {
    }

    public ComputerResult(Computer computer, double price, bool used, string url, SupportedServices source)
    {
        Computer = computer;
        Price = price;
        Used = used;
        Url = url;
        Source = source;
    }

    public Computer Computer { get; set; }

    public string ListingName { get; set; }

    public double Price { get; set; }

    public bool Used { get; set; }

    public string? Url { get; set; }

    public SupportedServices Source { get; set; }

    public virtual bool Equals(ComputerResult? other)
    {
        return Computer.Name.Equals(other.Computer.Name) && ListingName.Equals(other.ListingName) &&
               Math.Abs(Price - other.Price) < 0.001 && Used == other.Used && Url == other.Url;
    }
}