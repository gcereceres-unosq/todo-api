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
            TaskStatus = TaskIsOverDue(newTaskModel.DueDate) ? (int)TaskStatus.Overdue : (int)TaskStatus.ToDo, // Initial status: ToDo
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var todoTask = await _repo.Create(newTask);
        return _mapper.Map<TodoResponseModel>(todoTask);
    }

    public async Task<bool> Update(long id, TodoPutModel updatedTaskModel)
    {

        if (updatedTaskModel.TaskStatus == TaskStatus.Overdue && !TaskIsOverDue(updatedTaskModel.DueDate))
        {
            throw new InvalidTaskStatusException($"Status cannot be set to: {updatedTaskModel.TaskStatus}");
        }

        var existingTask = await _repo.GetById(id);

        if (existingTask == null)
        {
            throw new NotFoundException($"Task with id: {id}");
        }

        existingTask.Title = updatedTaskModel.Title;
        existingTask.Content = updatedTaskModel.Content;
        existingTask.DueDate = updatedTaskModel.DueDate;
        existingTask.TaskStatus = TaskIsOverDue(updatedTaskModel.DueDate) && updatedTaskModel.TaskStatus != TaskStatus.Done ? (int)TaskStatus.Overdue : (int)updatedTaskModel.TaskStatus;

        return await _repo.Update(existingTask);
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

    private bool TaskIsOverDue(DateTime dueDate) => dueDate.Date <= DateTime.Now.Date;
}