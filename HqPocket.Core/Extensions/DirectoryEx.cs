using System.IO;

namespace HqPocket.Extensions;

public static class DirectoryEx
{
    public static void CreateDirectoryIfNotExists(string path)
    {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
    }
}

