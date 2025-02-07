using Microsoft.AspNetCore.Mvc.Testing;

namespace Todo.Api.Integration.Tests;

[TestClass]
public class TodoIntegrationTests:WebApplicationFactory<Program>
{

    [TestMethod]
    public async Task GetAllTasks_ShouldReturnTasklist()
    {
        // Arrange
        // var client= CreateClient().GetAsync()

        // Act
        var response = await CreateClient().GetAsync("/api/todo");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual("application/json; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());

    }
}