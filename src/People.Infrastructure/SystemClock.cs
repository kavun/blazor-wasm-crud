using People.Application;

namespace People.Infrastructure;
public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
