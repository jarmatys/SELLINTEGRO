using Sellintegro.Domain.Entities.Tasks;
using Sellintegro.Domain.Entities.Tasks.Repositories;

namespace Sellintegro.Persistence.Repositories;

internal sealed class TaskRepository : ITaskRepository
{
    // Here normaly we should have Context to database
    private readonly List<TaskEntity> _inMemoryTask = [
        TaskEntity.Create("Test - 1", "1"),
        TaskEntity.Create("Test - 2", "2")
    ];
    
    public TaskEntity Create(TaskEntity taskEntity)
    {
        _inMemoryTask.Add(taskEntity);
        
        return taskEntity;
    }

    public IEnumerable<TaskEntity> GetList()
    {
        return _inMemoryTask;
    }
}