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
            // Retrieve the database connection string from configuration
            _connectionString = configuration.GetConnectionString("AppProgDb");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] Login credentials)
        {
            // Validate user and fetch user_id
            var userId = GetUserId(credentials.Username, credentials.Password);

            if (userId != null)
            {
                // // Generate Basic Auth header
                // var text = $"{credentials.Username}:{credentials.Password}";
                // var bytes = System.Text.Encoding.Default.GetBytes(text);
                // var encodedCredentials = Convert.ToBase64String(bytes);
                // var headerValue = $"Basic {encodedCredentials}";

                // Use AuthenticationHelper to handle the encoding/encryption
                var headerValue = AuthenticationHelper.Encrypt(credentials.Username, credentials.Password);


                return Ok(new 
                {
                    message = "Login Successfully",
                    userId = userId,          // Return user_id in the response
                    headerValue = headerValue // Return Basic Auth header
                });
            }
            else
            {
                return Unauthorized("Invalid username or password");
            }
        }

        private int? GetUserId(string username, string password)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Query the database to retrieve the user_id for the username/password
                var command = new NpgsqlCommand(
                    "SELECT id FROM users WHERE username = @username AND password_hash = @password",
                    connection
                );
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);

                var result = command.ExecuteScalar();

                // If a match is found, return the user_id; otherwise, return null
                return result == null ? (int?)null : (int)result;
            }
        }
    }
}
