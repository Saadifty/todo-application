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

// Add CORS policy to allow requests from Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular frontend URL
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

app.MapGet("/config-check", (IConfiguration config) =>
{
    var cs = config.GetConnectionString("AppProgDb");
    return Results.Ok(new { hasConnectionString = !string.IsNullOrWhiteSpace(cs) });
});

app.MapGet("/health", () => Results.Ok("ok"));

app.MapGet("/config-check", (IConfiguration cfg) =>
{
    var cs = cfg.GetConnectionString("AppProgDb");
    return Results.Ok(new { hasConnectionString = !string.IsNullOrWhiteSpace(cs) });
});

app.Run();




