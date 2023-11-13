using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using People.Infrastructure;
using People.Shared.People;
using PeopleBlazorWasm.Shared.People;
using System.Data.Common;
using Xunit.Abstractions;

namespace People.BlazorWasmServer.IntegrationTests;

public class PeopleControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public PeopleControllerTests(ITestOutputHelper testOutputHelper)
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
            });

            builder.ConfigureServices(services =>
            {
                services.RemoveAll<DbContextOptions<PeopleDbContext>>();
                services.RemoveAll<DbConnection>();

                // Create open SqliteConnection so EF won't automatically close it.
                services.AddSingleton<DbConnection>(container =>
                {
                    var connection = new SqliteConnection("DataSource=file::memory:?cache=shared");
                    connection.Open();

                    return connection;
                });

                services.AddDbContext<PeopleDbContext>((container, options) =>
                {
                    var connection = container.GetRequiredService<DbConnection>();
                    options.UseSqlite(connection);
                });

                ServiceProvider serviceProvider = services.BuildServiceProvider();
                IServiceScope serviceScope = serviceProvider.CreateScope();
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<PeopleDbContext>();
                dbContext.Database.EnsureCreated();
            });

            builder.UseEnvironment("Development");
        });
    }

    [Theory]
    [InlineData("/api/people")]
    public async Task GetPeople_ReturnsEmptyArray(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("/api/people/8c38c56a-3cdf-4614-8024-0f4d85ef4878")]
    public async Task GetPerson_Bogus_Returns404(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData("/api/people")]
    public async Task AddPerson_Empty_Returns400(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync(url, new PersonAddEditRequest() { });

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CRUD_HappyPath()
    {
        // Arrange
        var client = _factory.CreateClient();

        // CREATE
        var createResponse = await client.PostAsJsonAsync("/api/people", new PersonAddEditRequest()
        {
            Name = "test",
            City = "test",
            State = "test",
            Zip = "test",
            Phone = "test",
            Street = "test",
            Birth = new()
        });
        Assert.Equal(System.Net.HttpStatusCode.Created, createResponse.StatusCode);
        var createdPerson = await createResponse.Content.ReadFromJsonAsync<PersonAddEditResponse>();
        Assert.NotNull(createdPerson);
        Assert.NotNull(createdPerson.Person);
        Assert.NotEqual(Guid.Empty, createdPerson.Person.Id);

        // READ
        var readResponse = await client.GetAsync($"/api/people/{createdPerson.Person.Id}");
        readResponse.EnsureSuccessStatusCode();
        var readPerson = await readResponse.Content.ReadFromJsonAsync<PersonAddEditResponse>();
        Assert.NotNull(readPerson);
        Assert.NotNull(readPerson.Person);
        Assert.Equal(readPerson.Person.Id, createdPerson.Person.Id);

        // UPDATE
        var updateResponse = await client.PutAsJsonAsync($"/api/people/{createdPerson.Person.Id}", new PersonAddEditRequest()
        {
            Name = "updated name",
            City = "test",
            State = "test",
            Zip = "test",
            Phone = "test",
            Street = "test",
            Birth = new()
        });
        updateResponse.EnsureSuccessStatusCode();
        var updatedPerson = await updateResponse.Content.ReadFromJsonAsync<PersonAddEditResponse>();
        Assert.NotNull(updatedPerson);
        Assert.NotNull(updatedPerson.Person);
        Assert.Equal(updatedPerson.Person.Id, createdPerson.Person.Id);
        Assert.Equal("updated name", updatedPerson.Person.Name);

        // DELETE
        var deleteResponse = await client.DeleteAsync($"/api/people/{createdPerson.Person.Id}");
        deleteResponse.EnsureSuccessStatusCode();
    }
}
