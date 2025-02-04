using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models;

public class TodoPutModel
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; } 
    public DateTime DueDate { get; set; }
    
    [EnumDataType(typeof(TaskStatus))]
    public TaskStatus TaskStatus { get; set; }
}