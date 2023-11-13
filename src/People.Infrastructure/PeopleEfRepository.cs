using Microsoft.EntityFrameworkCore;
using People.Application;

namespace People.Infrastructure;

public class PeopleEfRepository : IPeopleRepository
{
    private readonly PeopleDbContext _peopleDbContext;

    public PeopleEfRepository(PeopleDbContext peopleDbContext)
    {
        _peopleDbContext = peopleDbContext;
    }

    public async Task AddPersonAsync(Person person, CancellationToken cancellationToken)
    {
        _peopleDbContext.People.Add(person);
        await _peopleDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _peopleDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Person?> FindPersonAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _peopleDbContext.People.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Person>> GetAllPeopleAsync(CancellationToken cancellationToken)
    {
        return await _peopleDbContext.People.ToListAsync(cancellationToken);
    }

    public async Task DeletePersonAsync(Person person, CancellationToken cancellationToken)
    {
        _peopleDbContext.People.Remove(person);
        await _peopleDbContext.SaveChangesAsync(cancellationToken);
    }
}
