using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models;

public class TodoPostModel
{
    [Required]
    public string Title { get; set; }
    [Required]
    [StringLength(250, ErrorMessage = "Name length can't be more than 250.")]
    public string Content { get; set; } 
    public DateTime DueDate { get; set; }   
}