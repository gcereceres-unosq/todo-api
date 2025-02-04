using Microsoft.AspNetCore.Mvc;
using Todo.Api.Interfaces;
using Todo.Api.Models;
using Todo.Api.Models.Response;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _service;
        public TodoController(ITodoService todoService)
        {
            _service = todoService;
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<TodoResponseModel[]>> GetTodoItems()
        {
            return await _service.GetAll();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoResponseModel>> GetTodo(long id)
        {
            var todo = await _service.GetById(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // // PUT: api/Todo/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo(long id, TodoPutModel model)
        {
            var result = await _service.Update(id, model);

            return result ? NoContent() : BadRequest();
        }

        // // POST: api/Todo
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoResponseModel>> PostTodo(TodoPostModel newTask)
        {
            var created = await _service.Create(newTask);

            return CreatedAtAction("GetTodo", new { id = created.Id }, created);
        }

        // // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(long id)
        {
            var result = await _service.Delete(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
