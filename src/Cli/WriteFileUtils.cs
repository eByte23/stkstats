namespace Cli;


public static class WriteFileUtils
{
    public static void FolderSafeWriteAllText(string path, string data)
    {
        var folder = Path.GetDirectoryName(path);
        if (string.IsNullOrWhiteSpace(folder))
        {
            folder = Directory.GetCurrentDirectory();
        }

        var parent = Directory.GetParent(folder);

        if (!Directory.Exists(folder) && parent != null && parent.Exists)
        {
            Directory.CreateDirectory(folder);
        }

        File.WriteAllText(path, data);
    }
}