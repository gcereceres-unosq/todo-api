
using Todo.Api.Models.Response;

namespace Todo.Api.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<TodoResponseModel>> GetAll();
}