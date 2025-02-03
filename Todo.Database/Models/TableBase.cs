using System;

namespace Todo.Database.Models;

public class TableBase
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }    
}