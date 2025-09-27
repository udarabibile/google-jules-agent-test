using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodoApi.Application;
using TodoApi.Application.Commands;
using TodoApi.Application.Queries;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public TodoItemsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var query = new GetAllTodosQuery();
            var result = await _dispatcher.Query(query);
            return Ok(result);
        }

        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var query = new GetTodoByIdQuery(id);
            var result = await _dispatcher.Query(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/TodoItems
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromBody] CreateTodoRequest request)
        {
            var command = new CreateTodoCommand(request.Name);
            await _dispatcher.Send(command);
            return CreatedAtAction(nameof(GetTodoItem), new { id = command.Id }, new { id = command.Id });
        }

        // PUT: api/TodoItems/{id}/name
        [HttpPut("{id}/name")]
        public async Task<IActionResult> UpdateTodoName(Guid id, [FromBody] UpdateTodoNameRequest request)
        {
            var command = new UpdateTodoNameCommand(id, request.NewName);
            await _dispatcher.Send(command);
            return NoContent();
        }

        // PUT: api/TodoItems/{id}/complete
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkTodoAsComplete(Guid id)
        {
            var command = new MarkTodoAsCompleteCommand(id);
            await _dispatcher.Send(command);
            return NoContent();
        }

        // PUT: api/TodoItems/{id}/incomplete
        [HttpPut("{id}/incomplete")]
        public async Task<IActionResult> MarkTodoAsIncomplete(Guid id)
        {
            var command = new MarkTodoAsIncompleteCommand(id);
            await _dispatcher.Send(command);
            return NoContent();
        }

        // DELETE: api/TodoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var command = new DeleteTodoCommand(id);
            await _dispatcher.Send(command);
            return NoContent();
        }
    }

    // DTOs for requests
    public class CreateTodoRequest
    {
    public string? Name { get; set; }
    }

    public class UpdateTodoNameRequest
    {
    public string? NewName { get; set; }
    }
}