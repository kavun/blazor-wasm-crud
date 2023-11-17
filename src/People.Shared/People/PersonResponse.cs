namespace People.Shared.People;

public record PersonResponse(
    Guid Id,
    string Name,
    AddressResponse Address,
    string Phone,
    DateTime Birth);
