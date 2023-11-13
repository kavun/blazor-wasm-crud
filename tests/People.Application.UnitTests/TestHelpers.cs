using People.Shared.People;

namespace People.Application.UnitTests;
public static class TestHelpers
{
    public static Person GetPerson(
        IClock clock,
        string name = "test")
    {
        var result = Person.Add(
            clock: clock,
            request: new PersonAddEditRequest()
            {
                Name = name,
                City = "test",
                State = "test",
                Zip = "test",
                Birth = new DateOnly(1990, 1, 1),
                Phone = "test",
                Street = "test"
            });

        return result.SuccessValue();
    }
}
