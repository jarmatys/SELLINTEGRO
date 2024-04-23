namespace Sellintegro.Contracts.Tasks.Events;

public sealed class GetEventsResponse
{
    public GetEventsResponse(List<Event<TaskDto>> events)
    {
        Events = events;
    }

    public List<Event<TaskDto>> Events { get; set; }
}

public class Event<T>
{
    public Event(T data)
    {
        Id = Guid.NewGuid().ToString();
        Type = "org.http-feeds.example.tasks";
        Source = "https://example.http-feeds.org/api/tasks/long-pooling";
        Time = DateTime.UtcNow.AddMinutes(-5);
        Subject = "9521234567899";
        Data = data;
    }

    public string Id { get; set; }
    public string Type { get; set; }
    public string Source { get; set; }
    public DateTime Time { get; set; }
    public string Subject { get; set; }
    public T Data { get; set; }
}