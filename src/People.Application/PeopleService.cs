using OneOf.Monads;
using People.Shared.People;

namespace People.Application;

public interface IPeopleService
{
    Task<Result<PersonAddError, PersonResponse>> AddPersonAsync(PersonAddEditRequest request, CancellationToken cancellationToken);
    Task<Result<PersonEditError, PersonResponse>> EditPersonAsync(Guid id, PersonAddEditRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<PersonResponse>> GetPeopleAsync(CancellationToken cancellationToken);
    Task<Result<PersonNotFound, PersonResponse>> GetPersonAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<PersonNotFound, PersonDeleted>> DeletePersonAsync(Guid id, CancellationToken cancellationToken);
}

public class PeopleService : IPeopleService
{
    private readonly IPeopleRepository _repository;
    private readonly IClock _clock;

    public PeopleService(IPeopleRepository repository, IClock clock)
    {
        _repository = repository;
        _clock = clock;
    }

    public async Task<IEnumerable<PersonResponse>> GetPeopleAsync(CancellationToken cancellationToken)
    {
        var people = await _repository.GetAllPeopleAsync(cancellationToken);
        return people.Select(p => p.ToPersonResponse());
    }

    public async Task<Result<PersonAddError, PersonResponse>> AddPersonAsync(PersonAddEditRequest request, CancellationToken cancellationToken)
    {
        var result = Person.Add(_clock, request);
        if (result.IsError())
        {
            return new PersonAddError(result.ErrorValue());
        }

        var person = result.SuccessValue();
        await _repository.AddPersonAsync(person, cancellationToken);
        return person.ToPersonResponse();
    }

    public async Task<Result<PersonEditError, PersonResponse>> EditPersonAsync(Guid id, PersonAddEditRequest request, CancellationToken cancellationToken)
    {
        var person = await _repository.FindPersonAsync(id, cancellationToken);
        if (person is null)
        {
            return new PersonEditError(new PersonNotFound(id));
        }

        var result = person.Edit(_clock, request);
        if (result.IsError())
        {
            return new PersonEditError(result.ErrorValue());
        }

        await _repository.SaveChangesAsync(cancellationToken);
        return result.SuccessValue().ToPersonResponse();
    }

    public async Task<Result<PersonNotFound, PersonResponse>> GetPersonAsync(Guid id, CancellationToken cancellationToken)
    {
        var person = await _repository.FindPersonAsync(id, cancellationToken);
        if (person is null)
        {
            return new PersonNotFound(id);
        }

        return person.ToPersonResponse();
    }

    public async Task<Result<PersonNotFound, PersonDeleted>> DeletePersonAsync(Guid id, CancellationToken cancellationToken)
    {
        var person = await _repository.FindPersonAsync(id, cancellationToken);
        if (person is null)
        {
            return new PersonNotFound(id);
        }

        await _repository.DeletePersonAsync(person, cancellationToken);
        return new PersonDeleted();
    }
}
