using MediatR;
using Sellintegro.Contracts.Tasks.AnyNewTask;
using Sellintegro.Domain.Entities.Tasks.Repositories;

namespace Sellintegro.Application.Tasks.Queries
{
    public sealed class AnyNewTasksQuery : IRequest<AnyNewTaskResponse>
    {
        private AnyNewTasksQuery()
        {
        }
        
        public static AnyNewTasksQuery Create()
        {
            return new AnyNewTasksQuery();
        }
    }
    
    public class AnyNewTasksQueryHandler(ITaskRepository taskRepository) : IRequestHandler<AnyNewTasksQuery, AnyNewTaskResponse>
    {
        public Task<AnyNewTaskResponse> Handle(AnyNewTasksQuery query, CancellationToken cancellationToken)
        {
            var anyNewItem = taskRepository.ListUpdated();
            
            return Task.FromResult(new AnyNewTaskResponse(anyNewItem));
        }
    }
}
