using People.Application.Abstract;

namespace People.Infrastructure;
public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
