using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sellintegro.Application;
using Sellintegro.Application.Tasks.Commands;
using Sellintegro.Application.Tasks.Queries;
using Sellintegro.Contracts.Tasks.Create;
using Sellintegro.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddPersistence();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/tasks", (IMediator mediator) => mediator.Send(GetTasksQuery.Create()))
    .WithName("GetTasks")
    .WithOpenApi();

app.MapPost(
        "/api/tasks", 
        (IMediator mediator, [FromBody] CreateTaskRequest request) => mediator.Send(CreateTaskCommand.Create(request))
    )
    .WithName("PostTasks")
    .WithOpenApi();

// INFO: This mechanism should be reanalyzed with more time to obtain profits related to long pooling
app.MapGet("/api/tasks/long-pooling", async (CancellationToken userCt, IMediator mediator) =>
{
    var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(userCt);
    cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

    while (!cancellationTokenSource.IsCancellationRequested)
    {
        var result = await mediator.Send(AnyNewTasksQuery.Create());
        if (result.AnyNewTask)
        {
            return Results.Ok(mediator.Send(GetTasksQuery.Create()));
        }
    }

    return Results.NoContent();
});

app.Run();