using Microsoft.AspNetCore.Mvc;
using People.Application;
using People.Shared.People;
using PeopleBlazorWasm.Shared.People;

namespace PeopleBlazorWasm.Server.Controllers;

[Route("api/people")]
[ApiController]
public class PeopleController : ControllerBase
{
    private readonly IPeopleService _peopleService;

    public PeopleController(IPeopleService peopleService)
    {
        _peopleService = peopleService;
    }

    [HttpGet("")]
    public async Task<IResult> GetPeople(CancellationToken cancellationToken)
    {
        var result = await _peopleService.GetPeopleAsync(cancellationToken);
        return Results.Ok(result);
    }

    [HttpPost("")]
    public async Task<IResult> AddPerson([FromBody] PersonAddEditRequest request, CancellationToken cancellationToken)
    {
        var result = await _peopleService.AddPersonAsync(request, cancellationToken);
        return result.Match(
            (error) => Results.BadRequest(new PersonAddEditResponse(error.Value.Errors)),
            (person) => Results.Created($"/api/people/{person.Value.Id}", new PersonAddEditResponse(person.Value)));
    }

    [HttpGet("{id:guid}")]
    public async Task<IResult> GetPerson(Guid id, CancellationToken cancellationToken)
    {
        var result = await _peopleService.GetPersonAsync(id, cancellationToken);
        return result.Match(
            (notFound) => Results.NotFound(notFound.Value),
            (person) => Results.Ok(new PersonAddEditResponse(person.Value)));
    }

    [HttpPut("{id:guid}")]
    public async Task<IResult> EditPerson(Guid id, [FromBody] PersonAddEditRequest request, CancellationToken cancellationToken)
    {
        var result = await _peopleService.EditPersonAsync(id, request, cancellationToken);
        return result.Match(
            (error) => error.Value.Error.Match(
                (notFound) => Results.NotFound(notFound),
                (fieldErrors) => Results.BadRequest(new PersonAddEditResponse(fieldErrors))),
            (person) => Results.Ok(new PersonAddEditResponse(person.Value)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IResult> DeletePerson(Guid id, CancellationToken cancellationToken)
    {
        var result = await _peopleService.DeletePersonAsync(id, cancellationToken);
        return result.Match(
            (notFound) => Results.NotFound(notFound.Value),
            (deleted) => Results.NoContent());
    }

}
