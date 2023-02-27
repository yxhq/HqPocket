using System.Collections.Generic;
using System.IO;
// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.Configuration;

public static class WritableConfigurationExtensions
{
    private const string FilePhysicalPathKey = "HQPOCKET_WritableOptionsFile";
    public static IConfigurationBuilder SetWritable(this IConfigurationBuilder builder, string path)
    {
        string? filePhysicalPath = builder.GetFileProvider().GetFileInfo(path).PhysicalPath;
        Dictionary<string, string?> dict = new() { { FilePhysicalPathKey, filePhysicalPath } };

        return builder.AddInMemoryCollection(dict);
    }

    public static string GetWritableConfigurationFilePhysicalPath(this IConfiguration configuration)
    {
        var path = configuration.GetSection(FilePhysicalPathKey).Value;
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new FileNotFoundException("Must set writable file first.");
        }

        return path;
    }
}
