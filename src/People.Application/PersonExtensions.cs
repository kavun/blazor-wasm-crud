using People.Shared.People;

namespace People.Application;

public static class PersonExtensions
{
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse(
            Id: person.Id,
            Name: person.Name,
            Address: new AddressResponse(
                Street: person.Street,
                City: person.City,
                State: person.State,
                Zip: person.Zip),
            Phone: person.Phone,
            Birth: person.Birth.ToDateTime(new TimeOnly())
        );
    }
}
