namespace People.Shared;

public record FieldErrors(List<FieldError> Errors)
{
    public FieldErrors() : this(new List<FieldError>())
    {
    }

    public void Add(string error)
    {
        Errors.Add(new FieldError(error));
    }

    public bool Any()
    {
        return Errors.Count > 0;
    }
};
