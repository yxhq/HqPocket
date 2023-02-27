using Microsoft.Extensions.Options;

using System.Diagnostics.CodeAnalysis;

namespace HqPocket.Extensions.Options;

public class WritableOptionsManager<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions> : OptionsManager<TOptions>, IWritableOptions<TOptions> where TOptions : class
{
    public WritableOptionsManager(IOptionsFactory<TOptions> factory) : base(factory)
    {
    }

    public WritableOptionsManager(IOptionsFactory<TOptions> factory, IOptionsWriter optionsWriter) : base(factory)
    {
        optionsWriter.Add(Value);
    }
}
