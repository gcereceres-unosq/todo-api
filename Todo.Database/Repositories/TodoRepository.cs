using Todo.Database.Models;
using Todo.Database.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Todo.Database.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task<TodoItem[]> GetAll()
    {
        return await _context.TodoItems.AsNoTracking().ToArrayAsync();
    }

    public async Task<TodoItem> GetById(long id)
    {
        return await _context.TodoItems.AsNoTracking().FirstOrDefaultAsync(task => task.Id == id);
    }

    public async Task<TodoItem> Create(TodoItem newTask)
    {
        _context.TodoItems.Add(newTask);
        await _context.SaveChangesAsync();

        return newTask;
    }

    public async Task<bool> TaskExists(string title, string content, DateTime dueDate)
    {
        var existing = await _context.TodoItems.FirstOrDefaultAsync(task =>
                        task.Title == title &&
                        task.Content == content &&
                        task.DueDate.Date == dueDate.Date);

        return existing != null;
    }

    public async Task<bool> Delete(long id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null)
        {
            return false;
        }

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Update(TodoItem updatedModel)
    {
        _context.Entry(updatedModel).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return true;
    }
}