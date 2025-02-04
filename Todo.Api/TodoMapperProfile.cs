using AutoMapper;
using Todo.Api.Models.Response;
using Todo.Database.Models;

namespace Todo.Api;
public class TodoMapperProfile:Profile
{
    public TodoMapperProfile()
    {
        CreateMap<TodoItem, TodoResponseModel>();
    }
}