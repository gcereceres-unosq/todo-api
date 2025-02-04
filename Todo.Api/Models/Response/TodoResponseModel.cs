namespace Todo.Api.Models.Response;

public class TodoResponseModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } 
    public DateTime DueDate { get; set; } 
    public TaskStatus TaskStatus { get; set; }

}