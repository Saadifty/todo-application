using ToDosAdminSystem.Model;
using ToDosAdminSystem.Model.Entities;
using ToDosAdminSystem.Model.Repositories;
using ToDosAdminSystem.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: allow local dev + deployed frontend(s) via config
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddScoped<TodoRepository, TodoRepository>();
builder.Services.AddScoped<UserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseBasicAuthenticationMiddleware();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/config-check", (IConfiguration cfg) =>
{
    var cs = cfg.GetConnectionString("AppProgDb");
    return Results.Ok(new { hasConnectionString = !string.IsNullOrWhiteSpace(cs) });
});

app.Run();






