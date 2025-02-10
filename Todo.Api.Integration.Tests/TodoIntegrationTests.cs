using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Todo.Api.Models;
using Todo.Api.Models.Response;

namespace Todo.Api.Integration.Tests;

[TestClass]
public class TodoIntegrationTests
{
    private static WebApplicationFactory<Program> _factory;
    private static HttpClient _client;

    private static Fixture _fixture = new Fixture();

    public TodoIntegrationTests()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task GetAllTasks_ShouldReturnExistingTask()
    {
        // Arrange
        var createdTodos = await SetupMultipleTasks();

        // Act
        var response = await _client.GetAsync($"/api/todo");
        var todos = await response.Content.ReadFromJsonAsync<TodoResponseModel[]>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.IsTrue(createdTodos.All(task => todos.Any(i => i.Id == task.Id)));
    }

    [TestMethod]
    public async Task GetTask_ShouldReturnExistingTask()
    {
        // Arrange
        var createdTodo = await SetupNewTask();

        // Act
        var response = await _client.GetAsync($"/api/todo/{createdTodo.Id}");
        var todo = await response.Content.ReadFromJsonAsync<TodoResponseModel>();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.AreEqual(createdTodo.Id, todo.Id);
        Assert.AreEqual(createdTodo.Title, todo.Title);

    }

    [TestMethod]
    public async Task PostTodo_CreatesNewTodo()
    {
        var newTodo = _fixture.Build<TodoPostModel>()
                        .With(prop => prop.DueDate, DateTime.Now.AddDays(1))
                        .Create();
        var response = await _client.PostAsJsonAsync("/api/todo", newTodo);

        response.EnsureSuccessStatusCode();
        var createdTodo = await response.Content.ReadFromJsonAsync<TodoResponseModel>();

        Assert.IsNotNull(createdTodo);
        Assert.AreEqual(newTodo.Title, createdTodo.Title);

        // New task should be created as "Todo"
        Assert.AreEqual(TaskStatus.ToDo, createdTodo.TaskStatus);
    }

    [TestMethod]
    public async Task DeleteTask_ShouldDeleteTask()
    {
        // Arrange
        var createdTodo = await SetupNewTask();

        // Act
        var response = await _client.DeleteAsync($"/api/todo/{createdTodo.Id}");
        var responseGet = await _client.GetAsync($"/api/todo/{createdTodo.Id}");

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, responseGet.StatusCode);

    }

    [TestMethod]
    public async Task Put_ShouldUpdateTask()
    {
        // Arrange
        var createdTodo = await SetupNewTask();
        var updateModel = _fixture.Build<TodoPutModel>()
            .With(model => model.DueDate, DateTime.Now.AddDays(1))
            .Create();

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(updateModel),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PutAsync($"/api/todo/{createdTodo.Id}", jsonContent);
        var responseGet = await _client.GetAsync($"/api/todo/{createdTodo.Id}");

        var modelGet = await responseGet.Content.ReadFromJsonAsync<TodoResponseModel>();

        // Assert
        Assert.AreEqual(createdTodo.Id, modelGet.Id);
        Assert.AreEqual(updateModel.Title, modelGet.Title);
        Assert.AreEqual(updateModel.Content, modelGet.Content);

    }

    private async Task<TodoResponseModel> SetupNewTask()
    {
        var newTodo = _fixture.Create<TodoPostModel>();
        var response = await _client.PostAsJsonAsync("/api/todo", newTodo);

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TodoResponseModel>();
    }

    private async Task<IEnumerable<TodoResponseModel>> SetupMultipleTasks()
    {
        var result = new List<TodoResponseModel>();
        for (var i = 0; i <= 2; i++)
        {
            var newTodo = _fixture.Create<TodoPostModel>();
            var response = await _client.PostAsJsonAsync("/api/todo", newTodo);
            result.Add(await response.Content.ReadFromJsonAsync<TodoResponseModel>());
        }

        return result;
    }
}