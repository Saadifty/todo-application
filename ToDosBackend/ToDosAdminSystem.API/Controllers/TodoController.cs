using ToDosAdminSystem.Model.Entities;
using ToDosAdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ToDosAdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        protected TodoRepository Repository { get; }
        public TodoController(TodoRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> GetTodo([FromRoute] int id)
        {
            Todo todo = Repository.GetTodoById(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }
       [HttpGet]
        public ActionResult<IEnumerable<Todo>> GetTodos([FromQuery] int? userId)
        {
            if (userId.HasValue)
            {
                return Ok(Repository.GetTodosByUserId(userId.Value));
            }
            return Ok(Repository.GetTodos());
        }

        [HttpPost]
        public ActionResult Post([FromBody] Todo todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo info not correct");
            }
            bool status = Repository.InsertTodo(todo);
            if (status)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut]
        public ActionResult UpdateTodo([FromBody] Todo todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo info not correct");
            }
            Todo existingTodo = Repository.GetTodoById(todo.id);
            if (existingTodo == null)
            {
                return NotFound($"Todo with id {todo.id} not found");
            }
            bool status = Repository.UpdateTodo(todo);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");
        }
        [HttpDelete("{id}")]
        public ActionResult DeleteTodo([FromRoute] int id)
        {
            Todo existingTodo = Repository.GetTodoById(id);
            if (existingTodo == null)
            {
                return NotFound($"Todo with id {id} not found");
            }
            bool status = Repository.DeleteToDo(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete Todo with id {id}");
        }

        // Fetch all tasks for the logged-in user
        // [HttpGet("user/{userId}")]
        // public ActionResult<List<Todo>> GetUserTodos([FromQuery] int userId)
        // {
        //     // Validate the userId
        //     if (userId <= 0)
        //     {
        //         return BadRequest("Invalid user ID.");
        //     }

        //     // Fetch tasks for the userId
        //     var userTodos = Repository.GetTodosByUserId(userId);

        //     if (userTodos == null || userTodos.Count == 0)
        //     {
        //         return NotFound("No tasks found for this user.");
        //     }

        //     return Ok(userTodos);
        // }
    }
}