namespace Sellintegro.SnapshotGenerator.Job;

internal static class ApiHealthCheck
{
    private const string EndpointPath = "api/tasks";

    private static readonly HttpClient Client = new()
    {
        BaseAddress = new Uri("http://localhost:5137")
    };

    public static async Task<bool> IsApiAlive()
    {
        try
        {
            var response = await Client.GetAsync(EndpointPath);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}