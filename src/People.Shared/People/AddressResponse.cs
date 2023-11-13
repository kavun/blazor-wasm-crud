namespace PeopleBlazorWasm.Shared.People;

public record AddressResponse(
    string Street,
    string City,
    string State,
    string Zip);
