using AutoMapper;
using Todo.Api.Interfaces;
using Todo.Api.Models;
using Todo.Api.Models.Response;
using Todo.Database.Models;
using Todo.Database.Repositories.Interfaces;

namespace Todo.Api.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repo;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TodoResponseModel[]> GetAll()
    {
        var todoTasks = await _repo.GetAll();
        return _mapper.Map<TodoResponseModel[]>(todoTasks);
    }

    public async Task<TodoResponseModel> GetById(long id)
    {
        var todoTask = await _repo.GetById(id);
        return _mapper.Map<TodoResponseModel>(todoTask);
    }

    public async Task<TodoResponseModel> Create(TodoPostModel newTaskModel)
    {
        if (await _repo.TaskExists(newTaskModel.Title, newTaskModel.Content, newTaskModel.DueDate))
        {
            throw new DuplicateTodoException($"A task with title: {newTaskModel.Title}, content: {newTaskModel.Content}, due date: {newTaskModel.DueDate}. Already exists");
        }
        var newTask = new TodoItem
        {
            Title = newTaskModel.Title,
            Content = newTaskModel.Content,
            DueDate = newTaskModel.DueDate,
            IsComplete = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var todoTask = await _repo.Create(newTask);
        return _mapper.Map<TodoResponseModel>(todoTask);
    }

    public async Task<bool> Delete(long id)
    {
        var todo = await _repo.GetById(id);
        if (todo == null)
        {
            return false;
        }

        return await _repo.Delete(id);
    }
}