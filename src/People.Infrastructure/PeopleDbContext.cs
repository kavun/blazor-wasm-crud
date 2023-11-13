using Microsoft.EntityFrameworkCore;
using People.Application;

namespace People.Infrastructure;

public class PeopleDbContext : DbContext
{
    public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options) { }
    public DbSet<Person> People { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PeopleDbContext).Assembly);
    }
}
