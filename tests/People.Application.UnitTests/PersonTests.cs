using People.Application.Abstract;
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
        var request = TestHelpers.GetPersonAddEditRequest(name: null);

        // act
        var result = Person.Add(_clock, request);

        // assert
        Assert.True(result.IsError());
        Assert.Contains(result.ErrorValue().Errors, e => e.Message == "Name is required.");
    }

    [Fact]
    public void Add_DateInFuture_ReturnsFieldErrors()
    {
        // arrange
        var testClock = new TestClock(utcNow: new DateTime(2023, 1, 1));
        var request = TestHelpers.GetPersonAddEditRequest(birth: new DateOnly(2023, 2, 1));

        // act
        var result = Person.Add(_clock, request);

        // assert
        Assert.True(result.IsError());
        Assert.Contains(result.ErrorValue().Errors, e => e.Message == "Humans cannot be born in the future.");
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
        Assert.Contains(result.ErrorValue().Errors, e => e.Message == "Name is required.");
    }
}
