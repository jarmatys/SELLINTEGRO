using System.Text;

namespace Sellintegro.SnapshotGenerator.Job;

internal static class Files
{
    internal static async Task SaveToFile(string filePath, string content)
    {
        await using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

        await writer.WriteAsync(content);
    }
}