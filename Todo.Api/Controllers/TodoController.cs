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
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTodo(long id, Todo todo)
        // {
        //     if (id != todo.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(todo).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!TodoExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

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
