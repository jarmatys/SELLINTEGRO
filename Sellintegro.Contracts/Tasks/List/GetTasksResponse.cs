namespace Sellintegro.Contracts.Tasks.List;

public sealed class GetTasksResponse(List<TaskDto> tasks)
{
    public List<TaskDto> Tasks { get; set; } = tasks;
}