using OneOf;
using People.Shared;

namespace People.Application;

public record PersonEditError(OneOf<PersonNotFound, FieldErrors> Error);
