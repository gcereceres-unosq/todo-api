using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo.Api;
using Todo.Api.Interfaces;
using Todo.Api.Services;
using Todo.Database;
using Todo.Database.Repositories;
using Todo.Database.Repositories.Interfaces;

var  TodoAllowSpecificOrigins = "_localhost";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: TodoAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(TodoMapperProfile));

builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors(TodoAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
