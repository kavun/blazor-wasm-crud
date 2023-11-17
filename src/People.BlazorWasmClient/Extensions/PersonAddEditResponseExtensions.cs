using People.Shared.People;

namespace People.BlazorWasmClient.Extensions;

public static class PersonAddEditResponseExtensions
{
    public static PersonAddEditRequest ToRequest(this PersonAddEditResponse response)
    {
        if (response is null || response.Person is null)
        {
            return new();
        }

        var person = response.Person;

        return new PersonAddEditRequest()
        {
            Name = person.Name,
            Street = person.Address?.Street,
            City = person.Address?.City,
            State = person.Address?.State,
            Zip = person.Address?.Zip,
            Phone = person.Phone,
            Birth = new DateOnly(
                year: person.Birth.Year,
                month: person.Birth.Month,
                day: person.Birth.Day)
        };
    }
}
