using System;
using System.Collections.Concurrent;

namespace HqPocket.Mvvm;

public class VvmTypeLocationProvider
{
    private static Func<Type, Type> _defaultTypeProvider = DefaultTypeProvider;
    private static readonly ConcurrentDictionary<string, Type> _types = new();

    public static VvmTypeLocationRegister Register<TMaped>() => Register(typeof(TMaped));
    public static VvmTypeLocationRegister Register(Type mappedType) => new(_types, mappedType);

    public static Type GetMappedType<T>() => GetMappedType(typeof(T));
    public static Type GetMappedType(Type type)
    {
        var key = type.ToString();

        if (_types.ContainsKey(key))
        {
            return _types[key];
        }
        return _defaultTypeProvider(type);
    }

    /// <summary>
    /// 自定义 TypeProvider
    /// </summary>
    /// <param name="typeProvider"></param>
    public static void SetTypeProvider(Func<Type, Type> typeProvider)
    {
        _defaultTypeProvider = typeProvider;
    }

    private static Type DefaultTypeProvider(Type type)
    {
        string typeName = type.FullName ?? type.ToString();
        string? resultTypeName = null;

        if (typeName.Contains(Conventions.ViewDotFolder))
        {
            resultTypeName = GetViewModelTypeName(typeName);
        }
        else if (typeName.Contains(Conventions.ViewModelDotFolder))
        {
            resultTypeName = GetViewTypeName(typeName);
        }

        if (resultTypeName is null)
        {
            throw new ArgumentNullException($"Type {type.FullName} must in {Conventions.ViewDotFolder} or {Conventions.ViewModelDotFolder}");
        }

        Type? resultType = type.Assembly.GetType(resultTypeName); //Type.GetType()只能查找当前程序集，在Plugin中无法使用

        return resultType is null ? throw new ArgumentException($"Can't find type '{resultTypeName}'.") : resultType;
    }

    public static string GetViewModelTypeName(string viewTypeName)
    {
        string replacedTypeName = viewTypeName.Replace(Conventions.ViewDotFolder, Conventions.ViewModelDotFolder);
        if (replacedTypeName.EndsWith(Conventions.ViewSuffix))
        {
            replacedTypeName = replacedTypeName[..^Conventions.ViewSuffix.Length];
        }
        return $"{replacedTypeName}{Conventions.ViewModelSuffix}";
    }

    public static string GetViewTypeName(string viewModelTypeName)
    {
        string replacedTypeName = viewModelTypeName.Replace(Conventions.ViewModelDotFolder, Conventions.ViewDotFolder);
        if (replacedTypeName.EndsWith(Conventions.ViewModelSuffix))
        {
            replacedTypeName = replacedTypeName[..^Conventions.ViewModelSuffix.Length];
        }
        if (!replacedTypeName.EndsWith(Conventions.WindowViewSuffix))
        {
            replacedTypeName = $"{replacedTypeName}{Conventions.ViewSuffix}";
        }
        return replacedTypeName;
    }
}
