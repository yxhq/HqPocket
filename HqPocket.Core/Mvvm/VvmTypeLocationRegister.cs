using System;
using System.Collections.Concurrent;

namespace HqPocket.Mvvm;

public class VvmTypeLocationRegister
{
    private readonly Type _mappedType;
    private readonly ConcurrentDictionary<string, Type> _types;

    internal VvmTypeLocationRegister(ConcurrentDictionary<string, Type> types, Type mappedType)
    {
        _types = types;
        _mappedType = mappedType;
    }

    public VvmTypeLocationRegister For(Type type)
    {
        string key = type.ToString();
        if (!_types.ContainsKey(key))
        {
            _types[key] = _mappedType;
            return this;
        }
        throw new Exception($"Key {key} allready exists.");
    }

    public VvmTypeLocationRegister For<T>() => For(typeof(T));
}
