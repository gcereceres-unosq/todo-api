using Microsoft.EntityFrameworkCore;

namespace Todo.Database.Models;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> TodoItems { get; set; } = null!;
}