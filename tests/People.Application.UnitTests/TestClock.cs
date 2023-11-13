namespace People.Application.UnitTests;

public class TestClock : IClock
{
    public DateTime UtcNow => new(2023, 1, 1);
}
