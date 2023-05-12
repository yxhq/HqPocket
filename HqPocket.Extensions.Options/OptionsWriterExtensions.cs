namespace HqPocket.Extensions.Options;

public static class OptionsWriterExtensions
{
    public static void Add<TOptions>(this IOptionsWriter optionsWriter, TOptions options)
    {
        optionsWriter.Add(options, typeof(TOptions).Name);
    }
}
