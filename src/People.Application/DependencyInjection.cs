using Microsoft.Extensions.DependencyInjection;

namespace People.Application;
public static class DependencyInjection
{
    public static void AddPeopleApplication(this IServiceCollection services)
    {
        services.AddScoped<IPeopleService, PeopleService>();
    }
}
