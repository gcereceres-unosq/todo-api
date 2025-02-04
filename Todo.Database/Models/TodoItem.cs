namespace Todo.Database.Models;
public class TodoItem : TableBase
{
    public string Title { get; set; }
    public string Content { get; set; } 
    public bool IsComplete { get; set; }
    public DateTime DueDate { get; set; }   
}