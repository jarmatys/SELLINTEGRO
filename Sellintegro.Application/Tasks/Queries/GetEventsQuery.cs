using MediatR;
using Sellintegro.Contracts.Tasks;
using Sellintegro.Contracts.Tasks.Events;
using Sellintegro.Contracts.Tasks.List;
using Sellintegro.Domain.Entities.Tasks.Repositories;

namespace Sellintegro.Application.Tasks.Queries
{
    public sealed class GetEventsQuery : IRequest<GetEventsResponse>
    {
        private GetEventsQuery(string lastEventId)
        {
            LastEventId = lastEventId;
        }
        
        public string LastEventId { get; }
        
        public static GetEventsQuery Create(string lastEventId)
        {
            return new GetEventsQuery(lastEventId);
        }
    }
    
    public class GetEventsQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetEventsQuery, GetEventsResponse>
    {
        public Task<GetEventsResponse> Handle(GetEventsQuery query, CancellationToken cancellationToken)
        {
            var tasks = taskRepository
                .GetList()
                .Select(task => new TaskDto(task.Id, task.Title, task.Description))
                .Select(taskDto => new Event<TaskDto>(taskDto))
                .ToList();
            
            // TODO: Check lastEventId and extract only new items (if exist)
            
            return Task.FromResult(new GetEventsResponse(tasks));
        }
    }
}
