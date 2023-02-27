namespace HqPocket.Extensions.Options;

public interface IOptionsWriter
{
    void Add<TOptions>(TOptions options);
    void Write();
}
