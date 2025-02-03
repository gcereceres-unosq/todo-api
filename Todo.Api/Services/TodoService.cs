using Microsoft.EntityFrameworkCore;
using Todo.Api.Interfaces;
using Todo.Api.Models.Response;
using Todo.Database.Models;

namespace Todo.Api.Services;

public class TodoService : ITodoService
{
    private readonly TodoContext _context;

    public TodoService(TodoContext context)
    {
        _context = context;
    }

    public async Task<TodoResponseModel[]> GetAll()
    {
        return await _context.TodoItems.AsNoTracking().ToArrayAsync();
    }
}