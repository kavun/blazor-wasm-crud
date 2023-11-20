namespace People.Application.Abstract;

public interface IPeopleRepository
{
    public Task<IEnumerable<Person>> GetAllPeopleAsync(CancellationToken cancellationToken);
    public Task AddPersonAsync(Person person, CancellationToken cancellationToken);
    public Task<Person?> FindPersonAsync(Guid id, CancellationToken cancellationToken);
    public Task SaveChangesAsync(CancellationToken cancellationToken);
    public Task DeletePersonAsync(Person person, CancellationToken cancellationToken);
}
