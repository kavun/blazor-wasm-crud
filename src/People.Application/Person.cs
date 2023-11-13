using OneOf.Monads;
using People.Shared;
using People.Shared.People;

namespace People.Application;

public class Person
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Person() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Zip { get; private set; }
    public string Phone { get; private set; }
    public DateOnly Birth { get; private set; }

    public static Result<FieldErrors, Person> Add(
        IClock clock,
        PersonAddEditRequest request)
    {
        var fieldErrors = ValidateAddEditRequest(clock, request);
        if (fieldErrors.Any())
        {
            return fieldErrors;
        }

        return new Person
        {
            Name = request.Name!,
            Street = request.Street!,
            City = request.City!,
            State = request.State!,
            Zip = request.Zip!,
            Phone = request.Phone!,
            Birth = request.Birth!.Value
        };
    }

    public static FieldErrors ValidateAddEditRequest(IClock clock, PersonAddEditRequest request)
    {
        var fieldErrors = new FieldErrors();
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            fieldErrors.Add("Name is required");
        }

        if (string.IsNullOrWhiteSpace(request.Street))
        {
            fieldErrors.Add("Street is required");
        }

        if (string.IsNullOrWhiteSpace(request.City))
        {
            fieldErrors.Add("City is required");
        }

        if (string.IsNullOrWhiteSpace(request.State))
        {
            fieldErrors.Add("State is required");
        }

        if (string.IsNullOrWhiteSpace(request.Zip))
        {
            fieldErrors.Add("Zip is required");
        }

        if (string.IsNullOrWhiteSpace(request.Phone))
        {
            fieldErrors.Add("Phone is required");
        }

        if (request.Birth is null)
        {
            fieldErrors.Add("Birth is required");
        }
        else if (request.Birth.Value.ToDateTime(new TimeOnly()) > clock.UtcNow)
        {
            fieldErrors.Add("Humans cannot be born in the future.");
        }

        return fieldErrors;
    }

    public Result<FieldErrors, Person> Edit(IClock clock, PersonAddEditRequest request)
    {
        var fieldErrors = ValidateAddEditRequest(clock, request);
        if (fieldErrors.Any())
        {
            return fieldErrors;
        }

        Name = request.Name!;
        Street = request.Street!;
        City = request.City!;
        State = request.State!;
        Zip = request.Zip!;
        Phone = request.Phone!;
        Birth = request.Birth!.Value;

        return this;
    }
}
