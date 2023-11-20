namespace People.Shared.Extensions;
public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime dateTime)
    {
        return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}
