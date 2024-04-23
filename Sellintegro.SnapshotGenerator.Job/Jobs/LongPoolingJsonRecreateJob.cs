using System.Net;
using System.Text.Json;
using Quartz;
using Sellintegro.Contracts.Tasks.Events;

namespace Sellintegro.SnapshotGenerator.Job.Jobs;

internal class LongPoolingJsonRecreateJob : IJob
{
    private const string EndpointPath = "api/tasks/long-pooling";
    private const string FilePath = "ToDoItemsCurrentSnapshot.json";

    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("http://localhost:5137")
    };

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("Recreate snapshot triggered: " + DateTime.Now);

        try
        {
            // IMPROVEMENT: here we should get only new (or updated) tasks, not all - and append/update *.json file
            var response = await Client.GetAsync($"{EndpointPath}?lastEventId=TODO");

            switch (response)
            {
                case { IsSuccessStatusCode: true, StatusCode: HttpStatusCode.OK }:
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var deserializedEvents = JsonSerializer.Deserialize<GetEventsResponse>(content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    var tasks = JsonSerializer.Serialize(deserializedEvents!.Events.Select(x => x.Data));

                    await Files.SaveToFile(FilePath, tasks);

                    Console.WriteLine("Snapshot updated - new tasks added");
                    break;
                }
                case { IsSuccessStatusCode: true, StatusCode: HttpStatusCode.NoContent }:
                    Console.WriteLine("No new tasks");
                    break;
                default:
                    Console.WriteLine($"ERROR - {response.StatusCode}");
                    // IMPROVEMENT: VERY IMPORTANT! Alerts here should be added - when API returns 400/500 status codes
                    break;
            }
        }
        catch (Exception exception)
        {
            // IMPROVEMENT: VERY IMPORTANT! Alerts here should be added - when API down, we should get information about this
        }

        await Task.CompletedTask;
    }
}