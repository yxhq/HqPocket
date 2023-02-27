using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;

namespace HqPocket.Extensions.Regioning;

public class RegionAdapterFactory : IRegionAdapterFactory
{
    private readonly ConcurrentDictionary<Type, Type> _adapterTypeFactory = new();

    public void AddAdapter<TAdapter>() where TAdapter : IRegionAdapter
    {
        Type currentType = typeof(TAdapter);
        Type? baseType = currentType.BaseType;
        while (baseType is not null)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(RegionAdapterBase<>))
            {
                var argumentType = baseType.GetGenericArguments()[0];
                _adapterTypeFactory[argumentType] = currentType;
                return;
            }
            baseType = baseType.BaseType;
        }

        throw new InvalidOperationException("Region adapter must inherit from class RegionAdapterBase<>.");
    }

    public IRegionAdapter Create(FrameworkElement element)
    {
        Type? currentType = element.GetType();

        while (currentType is not null)
        {
            if (_adapterTypeFactory.ContainsKey(currentType))
            {
                if (Activator.CreateInstance(_adapterTypeFactory[currentType], element) is IRegionAdapter adapter)
                {
                    return adapter;
                }
            }
            currentType = currentType.BaseType;
        }

        throw new KeyNotFoundException($"No region adapter found for type {element.GetType()}.");
    }
}