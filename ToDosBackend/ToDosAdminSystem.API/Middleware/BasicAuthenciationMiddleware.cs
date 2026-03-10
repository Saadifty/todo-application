using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace ToDosAdminSystem.API.Middleware
{
    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public BasicAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 1) Allow anonymous endpoints by attribute
            if (context.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            // 2) Allow anonymous paths (health/config/swagger) for cloud diagnostics
            var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
            if (path == "/health"
                || path == "/config-check"
                || path == "/swagger"
                || path.StartsWith("/swagger/"))
            {
                await _next(context);
                return;
            }

            // 3) Require Authorization: Basic ...
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"ToDosAdminSystem\"";
                await context.Response.WriteAsync("Authorization header not provided or invalid");
                return;
            }

            // 4) Decode credentials (your helper)
            AuthenticationHelper.Decrypt(authHeader, out string username, out string password);

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"ToDosAdminSystem\"";
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }

            // 5) Validate against database using configured connection string
            var connectionString = _configuration.GetConnectionString("AppProgDb");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // Misconfiguration is a server error, not an auth error
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Database connection string is not configured.");
                return;
            }

            var isValid = await ValidateCredentialsAsync(connectionString, username, password);

            if (!isValid)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"ToDosAdminSystem\"";
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }

            await _next(context);
        }

        private static async Task<bool> ValidateCredentialsAsync(string connectionString, string username, string password)
        {
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                await using var command = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM users WHERE username = @username AND password_hash = @password",
                    connection);

                command.Parameters.AddWithValue("username", username);
                command.Parameters.AddWithValue("password", password);

                var resultObj = await command.ExecuteScalarAsync();
                var result = Convert.ToInt64(resultObj);

                return result > 0;
            }
            catch
            {
                // If DB is down/misconfigured, treat as not valid (or you can return 500)
                return false;
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
