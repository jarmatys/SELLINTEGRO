using MediatR;
using Sellintegro.Contracts.Tasks;
using Sellintegro.Contracts.Tasks.List;
using Sellintegro.Domain.Entities.Tasks.Repositories;

namespace Sellintegro.Application.Tasks.Queries
{
    public sealed class GetTasksQuery : IRequest<GetTasksResponse>
    {
        private GetTasksQuery()
        {
        }
        
        public static GetTasksQuery Create()
        {
            return new GetTasksQuery();
        }
    }
    
    public class GetTaskQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTasksQuery, GetTasksResponse>
    {
        public Task<GetTasksResponse> Handle(GetTasksQuery query, CancellationToken cancellationToken)
        {
            var tasks = taskRepository
                .GetList()
                .Select(task => new TaskDto(task.Id, task.Title, task.Description))
                .ToList();
            
            return Task.FromResult(new GetTasksResponse(tasks));
        }
    }
}
