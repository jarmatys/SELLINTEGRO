using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sellintegro.Application;
using Sellintegro.Application.Tasks.Commands;
using Sellintegro.Application.Tasks.Queries;
using Sellintegro.Contracts.Tasks;
using Sellintegro.Contracts.Tasks.Create;
using Sellintegro.Contracts.Tasks.Events;
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
app.MapGet("/api/tasks/long-pooling", async (
    IMediator mediator,
    CancellationToken cancellationToken,
    [FromQuery] string lastEventId, 
    [FromQuery] int timeout = 5000) =>
{
    var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
    cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
    
    var startTime = DateTime.UtcNow;

    while (!cancellationTokenSource.IsCancellationRequested)
    {
        return (DateTime.UtcNow - startTime).TotalMilliseconds > timeout 
            ? Results.Ok(new GetEventsResponse([])) 
            : Results.Ok(await mediator.Send(GetEventsQuery.Create(lastEventId)));
    }

    return Results.NoContent();
});

app.Run();