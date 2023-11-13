namespace People.Application;

public record PersonNotFound(Guid Id)
{
    public string Message = $"Person with Id {Id} wasn't found.";
}
