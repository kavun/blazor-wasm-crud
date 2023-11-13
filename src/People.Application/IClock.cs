namespace People.Application;
public interface IClock
{
    DateTime UtcNow { get; }
}
