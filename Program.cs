using Microsoft.EntityFrameworkCore;
using ToDoAPI.Data;
using ToDoAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("api/todo", async (AppDbContext context) =>
{
	var todos = await context.Todos.ToListAsync();
	return Results.Ok(todos);
});

app.MapPost("api/todo", async (AppDbContext context, Todo todo) =>
{
	await context.Todos.AddAsync(todo);
	await context.SaveChangesAsync();
	return Results.Created($"api/todo/{todo.Id}", todo);
});

app.MapPut("api/todo/{id}", async (AppDbContext context, int id, Todo todo) =>
{
	var item = context.Todos.Where(t => t.Id == id).FirstOrDefault();
	if (item == null) return Results.NotFound();
	item.TodoName = todo.TodoName;
	context.Todos.Update(item);
	context.SaveChanges();
	return Results.Ok(item);
});

app.MapDelete("api/todo/{id}", async (AppDbContext context, int id) =>
{
	var item = await context.Todos.FirstOrDefaultAsync(t => t.Id == id);
	if (item == null) return Results.NotFound();
	context.Todos.Remove(item);
	await context.SaveChangesAsync();
	return Results.Ok("Deleted");
});

app.UseHttpsRedirection();

app.Run();


