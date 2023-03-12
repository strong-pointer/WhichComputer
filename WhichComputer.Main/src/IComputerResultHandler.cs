namespace WhichComputer.Main;

public interface IComputerResultHandler
{
    public SupportedServices Service { get; }

    public IEnumerable<ComputerResult> Fetch(Computer computer, bool used, int amount);

    public IEnumerable<ComputerResult> Fetch(string computerName, bool used, int amount);
}