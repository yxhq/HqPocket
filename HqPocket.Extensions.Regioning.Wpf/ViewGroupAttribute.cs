using System;

namespace HqPocket.Extensions.Regioning;

[AttributeUsage(AttributeTargets.Class)]
public class ViewGroupAttribute : Attribute
{
    public string GroupName { get; }

    public ViewGroupAttribute(string groupName)
    {
        GroupName = groupName;
    }
}
