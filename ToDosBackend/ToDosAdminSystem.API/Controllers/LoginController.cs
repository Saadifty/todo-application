using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using ToDosAdminSystem.API.Model;

namespace ToDosAdminSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _connectionString;

        public LoginController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AppProgDb");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] Login credentials)
        {
            var userId = GetUserId(credentials.Username, credentials.Password);

            if (userId != null)
            {
                var headerValue = AuthenticationHelper.Encrypt(credentials.Username, credentials.Password);

                return Ok(new
                {
                    message = "Login Successfully",
                    userId = userId,
                    headerValue = headerValue
                });
            }

            return Unauthorized("Invalid username or password");
        }

        private long? GetUserId(string username, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                var command = new NpgsqlCommand(
                    "SELECT id FROM public.users WHERE username = @username AND password_hash = @password",
                    connection
                );

                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);

                var result = command.ExecuteScalar();

                // BIGSERIAL => Int64
                return result == null ? (long?)null : Convert.ToInt64(result);
            }
        }
    }
}
