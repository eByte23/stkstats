namespace Web.Tests;

internal static class LocalFileHelpers
{
    const string DATA_PATH = "/workspace/stats/data/";

    internal static string GetFileText(string filePath)
    {
        var absolutePath = Path.Join(DATA_PATH, filePath);

        return File.ReadAllText(absolutePath);
    }

    internal static string GetFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }
}