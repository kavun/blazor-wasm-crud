namespace People.BlazorWasmClient;

public interface IClientClock
{
    DateTime UtcNow { get; }
}
public class ClientSystemClock : IClientClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
