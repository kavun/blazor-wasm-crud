using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using People.Application.Abstract;

namespace People.Infrastructure;

public static class DependencyInjection
{
    public static void AddPeopleInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("People");
        ArgumentNullException.ThrowIfNull(connectionString);
        services.AddDbContext<PeopleDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IPeopleRepository, PeopleEfRepository>();
        services.AddSingleton<IClock, SystemClock>();
    }
}
