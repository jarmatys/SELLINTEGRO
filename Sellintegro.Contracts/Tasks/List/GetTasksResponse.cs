namespace Sellintegro.Contracts.Tasks.List;

public sealed class GetTasksResponse(List<TaskDto> tasks)
{
    public List<TaskDto> Tasks { get; set; } = tasks;
}

public sealed class TaskDto(Guid id, string title, string description)
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
}