using People.Application.Abstract;

namespace People.Application.UnitTests;

public partial class PeopleServiceTests
{
    private readonly TestPeopleRepository _repository;
    private readonly IClock _clock;
    private readonly PeopleService _service;
    private readonly CancellationToken _cancellationToken;

    public PeopleServiceTests()
    {
        _clock = new TestClock();
        _repository = new TestPeopleRepository();
        _service = new PeopleService(_repository, _clock);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task GetPeople_ShouldReturnResponse()
    {
        // arrange
        var expectedName = "test person";
        _repository.WithPeople(new Person[]
        {
            TestHelpers.GetPerson(
                clock: _clock,
                name: expectedName

                // TODO: arrange other person properties
            )
        });

        // act
        var result = await _service.GetPeopleAsync(_cancellationToken);

        // assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        var person = result.First();
        Assert.NotNull(person);
        Assert.Equal(expectedName, person.Name);

        // TODO: assert other person properties
    }
}
