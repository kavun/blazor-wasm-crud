namespace People.Application.UnitTests;

public class TestPeopleRepository : IPeopleRepository
{
    private Person[] _people = Array.Empty<Person>();

    public TestPeopleRepository() { }

    public TestPeopleRepository(Person[] people)
    {
        WithPeople(people);
    }

    public TestPeopleRepository WithPeople(Person[] people)
    {
        _people = people;
        return this;
    }

    public Task AddPersonAsync(Person person, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeletePersonAsync(Person person, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Person?> FindPersonAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Person>> GetAllPeopleAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<Person>>(_people.ToList());
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
