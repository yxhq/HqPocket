using System;

namespace HqPocket.Extensions.Regioning;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ViewSortHintAttribute : Attribute
{
    public string Hint { get; }
    public ViewSortHintAttribute(string hint)
    {
        Hint = hint;
    }
}