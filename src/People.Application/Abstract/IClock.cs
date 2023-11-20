namespace People.Application.Abstract;
public interface IClock
{
    DateTime UtcNow { get; }
}
