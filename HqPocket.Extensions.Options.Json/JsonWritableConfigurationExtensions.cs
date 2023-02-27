// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.Configuration;

public static class JsonWritableConfigurationExtensions
{
    public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder, string path)
    {
        return AddWritableJsonFile(builder, path, true);
    }

    public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder, string path, bool optional)
    {
        return AddWritableJsonFile(builder, path, optional, true);
    }

    public static IConfigurationBuilder AddWritableJsonFile(this IConfigurationBuilder builder, string path, bool optional, bool reloadOnChange)
    {
        builder.AddJsonFile(path, optional, reloadOnChange);
        return builder.SetWritable(path);
    }
}
