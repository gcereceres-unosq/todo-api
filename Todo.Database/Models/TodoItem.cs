namespace Todo.Database.Models;
public class TodoItem : TableBase
{
    public string Title { get; set; }
    public string Content { get; set; } 
    public int TaskStatus { get; set; }
    public DateTime DueDate { get; set; }   
}