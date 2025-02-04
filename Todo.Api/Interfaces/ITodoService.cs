
using Todo.Api.Models;
using Todo.Api.Models.Response;

namespace Todo.Api.Interfaces;

public interface ITodoService
{
    Task<TodoResponseModel[]> GetAll();
    Task<TodoResponseModel> GetById(long id);
    Task<TodoResponseModel> Create(TodoPostModel newTaskModel);
}