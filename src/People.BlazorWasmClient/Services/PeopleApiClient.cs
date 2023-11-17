using People.Shared.People;
using System.Net.Http.Json;

namespace People.BlazorWasmClient.Services;

public record PeopleApiResponse<T>(HttpResponseMessage HttpResponseMessage, T Content);

public class PeopleApiClient
{
    private readonly HttpClient _httpClient;

    public PeopleApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PeopleApiResponse<PersonResponse[]?>> GetPeople()
    {
        var response = await _httpClient.GetAsync("api/people");
        var content = await response.Content.ReadFromJsonAsync<PersonResponse[]>();
        return new PeopleApiResponse<PersonResponse[]?>(response, content);
    }

    public async Task<PeopleApiResponse<PersonAddEditResponse?>> PostPerson(PersonAddEditRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/people", request);
        var content = await response.Content.ReadFromJsonAsync<PersonAddEditResponse?>();
        return new PeopleApiResponse<PersonAddEditResponse?>(response, content);
    }

    public async Task<PeopleApiResponse<PersonAddEditResponse?>> GetPerson(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/people/{id}");
        var content = await response.Content.ReadFromJsonAsync<PersonAddEditResponse?>();
        return new PeopleApiResponse<PersonAddEditResponse?>(response, content);
    }

    public async Task<PeopleApiResponse<PersonAddEditResponse?>> PutPerson(Guid id, PersonAddEditRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/people/{id}", request);
        var content = await response.Content.ReadFromJsonAsync<PersonAddEditResponse?>();
        return new PeopleApiResponse<PersonAddEditResponse?>(response, content);
    }

    public async Task<HttpResponseMessage> DeletePerson(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"/api/people/{id}");
        return response;
    }
}
