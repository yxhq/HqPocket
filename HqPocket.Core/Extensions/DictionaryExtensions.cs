// ReSharper disable CheckNamespace

namespace System.Collections.Generic;

public static class DictionaryExtensions
{
    public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class, new()
    {
        return dictionary.TryGetValue(key, out TValue? value) ? value : new TValue();
    }
}
