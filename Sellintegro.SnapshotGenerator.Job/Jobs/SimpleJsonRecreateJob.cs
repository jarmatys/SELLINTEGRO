using Quartz;

namespace Sellintegro.SnapshotGenerator.Job.Jobs;

internal class SimpleJsonRecreateJob : IJob
{
    private const string EndpointPath = "api/tasks";
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
            var response = await Client.GetAsync(EndpointPath);

            switch (response)
            {
                case { IsSuccessStatusCode: true }:
                {
                    var content = await response.Content.ReadAsStringAsync();

                    await Files.SaveToFile(FilePath, content);

                    Console.WriteLine("Snapshot updated - new tasks added");
                    break;
                }
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