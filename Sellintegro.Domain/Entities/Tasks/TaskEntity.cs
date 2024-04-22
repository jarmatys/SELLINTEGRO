namespace Sellintegro.Domain.Entities.Tasks;

public sealed class TaskEntity
{
    private TaskEntity(string title, string description)
    {
        Title = title;
        Description = description;
        IsDone = false;
        Id = Guid.NewGuid();
    }
    
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsDone { get; set; }

    public static TaskEntity Create(string title, string description)
    {
        return new TaskEntity(title, description);
    }
}