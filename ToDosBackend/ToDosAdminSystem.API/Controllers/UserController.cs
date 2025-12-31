using ToDosAdminSystem.Model.Entities;
using ToDosAdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ToDosAdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected UserRepository Repository { get; }
        public UserController(UserRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser([FromRoute] int id)
        {
            User user = Repository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return Ok(Repository.GetAllUsers());
        }

        [HttpPost]
        public ActionResult Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User info not correct");
            }
            bool status = Repository.InsertUser(user);
            if (status)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        public ActionResult UpdateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User info not correct");
            }
            User existingUser = Repository.GetUserById(user.id);
            if (existingUser == null)
            {
                return NotFound($"User with id {user.id} not found");
            }
            bool status = Repository.UpdateUser(user);
            if (status)
            {
                return Ok();
            }
            return BadRequest("Something went wrong");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser([FromRoute] int id)
        {
            User existingUser = Repository.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound($"User with id {id} not found");
            }
            bool status = Repository.DeleteUser(id);
            if (status)
            {
                return NoContent();
            }
            return BadRequest($"Unable to delete User with id {id}");
        }

        // New Registration Endpoint
        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.username) || string.IsNullOrWhiteSpace(user.email) || string.IsNullOrWhiteSpace(user.password_hash))
            {
                return BadRequest("All fields are required.");
            }

            var existingUser = Repository.GetAllUsers().FirstOrDefault(u => u.username == user.username || u.email == user.email);
            if (existingUser != null)
            {
                return Conflict("Username or email already exists.");
            }

            bool result = Repository.InsertUser(user);

            if (result)
            {
                return Ok(new { message = "User registered successfully." }); 
            }

            return BadRequest("Failed to register the user.");
        }

    }
}
