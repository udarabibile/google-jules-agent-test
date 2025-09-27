using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using TodoApi.Application;
using TodoApi.Application.Commands;
using TodoApi.Application.Commands.Handlers;
using TodoApi.Application.Queries;
using TodoApi.Application.Queries.Handlers;
using TodoApi.Domain;
using TodoApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register CQRS and Event Sourcing services
builder.Services.AddSingleton<IDispatcher, Dispatcher>();
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
builder.Services.AddSingleton<IEventStore, InMemoryEventStore>();
builder.Services.AddSingleton(new TodoProjection()); // Register as singleton to maintain state

// Register generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register Command Handlers
builder.Services.AddScoped<ICommandHandler<CreateTodoCommand>, CreateTodoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateTodoNameCommand>, UpdateTodoNameCommandHandler>();
builder.Services.AddScoped<ICommandHandler<MarkTodoAsCompleteCommand>, MarkTodoAsCompleteCommandHandler>();
builder.Services.AddScoped<ICommandHandler<MarkTodoAsIncompleteCommand>, MarkTodoAsIncompleteCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteTodoCommand>, DeleteTodoCommandHandler>();

// Register Query Handlers
builder.Services.AddScoped<IQueryHandler<GetAllTodosQuery, IEnumerable<TodoReadModel>>, GetAllTodosQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetTodoByIdQuery, TodoReadModel>, GetTodoByIdQueryHandler>();

// Register Event Handlers (the projection)
builder.Services.AddSingleton<IEventHandler<TodoCreated>>(sp => sp.GetRequiredService<TodoProjection>());
builder.Services.AddSingleton<IEventHandler<TodoNameUpdated>>(sp => sp.GetRequiredService<TodoProjection>());
builder.Services.AddSingleton<IEventHandler<TodoCompleted>>(sp => sp.GetRequiredService<TodoProjection>());
builder.Services.AddSingleton<IEventHandler<TodoIncompleted>>(sp => sp.GetRequiredService<TodoProjection>());
builder.Services.AddSingleton<IEventHandler<TodoDeleted>>(sp => sp.GetRequiredService<TodoProjection>());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();