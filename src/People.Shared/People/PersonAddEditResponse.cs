using System.Text.Json.Serialization;

namespace People.Shared.People;

public class PersonAddEditResponse
{
    public PersonResponse? Person { get; }
    public FieldErrors Errors { get; }
    public bool Error => Errors.Any();

    [JsonConstructor]
    public PersonAddEditResponse(
        PersonResponse? person,
        FieldErrors errors)
    {
        Person = person;
        Errors = errors;
    }
    public PersonAddEditResponse(FieldErrors errors) : this(null, errors) { }
    public PersonAddEditResponse(PersonResponse person) : this(person, new()) { }
}
