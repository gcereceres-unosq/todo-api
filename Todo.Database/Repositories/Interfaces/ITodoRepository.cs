using System.Collections;
using Todo.Database.Models;

namespace Todo.Database.Repositories.Interfaces;

public interface ITodoRepository
{
    Task<TodoItem[]> GetAll();
    Task<TodoItem> GetById(long id);
    Task<TodoItem> Create(TodoItem newTask);
    Task<bool> Update(TodoItem updatedModel);
    Task<bool> Delete(long id);
    Task<bool> TaskExists(string title, string content, DateTime dueDate);
}