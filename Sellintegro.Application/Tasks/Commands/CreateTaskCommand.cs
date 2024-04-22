using MediatR;
using Sellintegro.Contracts.Tasks.Create;
using Sellintegro.Domain.Entities.Tasks;
using Sellintegro.Domain.Entities.Tasks.Repositories;

namespace Sellintegro.Application.Tasks.Commands
{
    public sealed class CreateTaskCommand : IRequest
    {
        private CreateTaskCommand(CreateTaskRequest payload)
        {
            Title = payload.Title;
            Description = payload.Description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        
        public static CreateTaskCommand Create(CreateTaskRequest payload)
        {
            return new CreateTaskCommand(payload);
        }
    }

    public class CreateTaskCommandHandler(ITaskRepository taskRepository): IRequestHandler<CreateTaskCommand>
    {
        public Task Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = TaskEntity.Create(request.Title, request.Description);

            taskRepository.Create(task);

            return Task.CompletedTask;
        }
    }
}