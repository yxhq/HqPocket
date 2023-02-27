using Microsoft.Extensions.Options;

using System.Diagnostics.CodeAnalysis;

namespace HqPocket.Extensions.Options;

public interface IWritableOptions<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions> : IOptions<TOptions> where TOptions : class
{
}
