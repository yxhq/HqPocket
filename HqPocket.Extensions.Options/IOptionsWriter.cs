namespace HqPocket.Extensions.Options;

public interface IOptionsWriter
{
    static IOptionsWriter Default => Ioc.GetRequiredService<IOptionsWriter>();
    void Add<TOptions>(TOptions options, string name);
    void Write();
}
