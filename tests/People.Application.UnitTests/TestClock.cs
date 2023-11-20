using People.Application.Abstract;

namespace People.Application.UnitTests;

public class TestClock : IClock
{
    private readonly DateTime _utcNow;

    public TestClock(DateTime utcNow)
    {
        _utcNow = utcNow;
    }

    public TestClock() : this(new(2023, 1, 1)) { }

    public DateTime UtcNow => _utcNow;
}
