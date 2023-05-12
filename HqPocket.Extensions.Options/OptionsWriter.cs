using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace HqPocket.Extensions.Options;

public abstract class OptionsWriter : IOptionsWriter
{
    private readonly string _filePhysicalPath;
    private readonly IDictionary<string, object?> _optionsFactory;

    protected OptionsWriter(IConfiguration configuration)
    {
        _filePhysicalPath = configuration.GetWritableConfigurationFilePhysicalPath();
        _optionsFactory = Load(_filePhysicalPath) ?? new Dictionary<string, object?>();
    }

    public virtual void Add<TOptions>(TOptions options, string name)
    {
        _optionsFactory[name] = options;
    }

    public void Write()
    {
        Write(_filePhysicalPath, _optionsFactory);
    }

    protected abstract IDictionary<string, object?>? Load(string path);
    protected abstract void Write(string path, IDictionary<string, object?> optionsFactory);
}
