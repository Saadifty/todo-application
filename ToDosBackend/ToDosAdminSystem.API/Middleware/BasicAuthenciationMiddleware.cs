using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace ToDosAdminSystem.API.Middleware
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for public or anonymous endpoints
            if (context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Basic "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Authorization header not provided or invalid");
                return;
            }

            // // Decode the Base64 encoded string
            // var auth = authHeader.Substring("Basic ".Length).Trim();
            // var decodedBytes = Convert.FromBase64String(auth);
            // var decodedString = Encoding.UTF8.GetString(decodedBytes);
            // var credentials = decodedString.Split(':');

            // Use AuthenticationHelper for decryption
            AuthenticationHelper.Decrypt(authHeader, out string username, out string password);

            // Validate the username and password (hardcoded or using the database)
            // if (credentials.Length != 2 || !ValidateCredentials(credentials[0], credentials[1]))
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || !ValidateCredentials(username, password))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }

            await _next(context);
        }

        private bool ValidateCredentials(string username, string password)
        {
            // Validate against the database 
            using (var connection = new Npgsql.NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=;Database=ToDos"))
            {
                connection.Open();

                var command = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @password", connection);
                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);

                var result = (long)command.ExecuteScalar();

                return result > 0;
            }
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}
