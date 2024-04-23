namespace Sellintegro.Domain.Entities.Tasks.Repositories;

public interface ITaskRepository
{
    TaskEntity Create(TaskEntity taskEntity);
    IEnumerable<TaskEntity> GetList();
}