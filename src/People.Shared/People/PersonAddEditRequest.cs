namespace People.Shared.People;
public class PersonAddEditRequest
{
    public string? Name { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public DateOnly? Birth { get; set; }
}
