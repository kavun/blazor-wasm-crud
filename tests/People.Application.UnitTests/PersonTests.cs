using People.Shared.People;

namespace People.Application.UnitTests;
public class PersonTests
{
    private readonly IClock _clock;

    public PersonTests()
    {
        _clock = new TestClock();
    }

    [Fact]
    public void Add_MissingName_ReturnsFieldErrors()
    {
        // arrange
        var request = new PersonAddEditRequest();

        // act
        var result = Person.Add(_clock, request);

        // assert
        Assert.True(result.IsError());
        Assert.Contains(result.ErrorValue().Errors, e => e.Message == "Name is required");

        // TODO: assert other FieldErrors
    }

    [Fact]
    public void Edit_MissingName_ReturnsFieldErrors()
    {
        // arrange
        var person = TestHelpers.GetPerson(_clock);
        var request = new PersonAddEditRequest();

        // act
        var result = person.Edit(_clock, request);

        // assert
        Assert.True(result.IsError());
        Assert.Contains(result.ErrorValue().Errors, e => e.Message == "Name is required");

        // TODO: assert other FieldErrors
    }
}
