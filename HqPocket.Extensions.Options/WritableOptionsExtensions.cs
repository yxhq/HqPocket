﻿using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.Linq;

namespace HqPocket.Extensions.Options;

public static class WritableOptionsExtensions
{
    public static void Update<TOptions>(this IOptions<TOptions> options, string name, TOptions? instance = null) where TOptions : class
    {
        IOptionsWriter.Default.Add(instance ?? options.Value, name);
    }

    public static void Update<TOptions>(this IOptions<TOptions> options, TOptions? instance = null) where TOptions : class
    {
        Update(options, typeof(TOptions).Name, instance);
    }

    public static void Write<TOptions>(this IOptions<TOptions> _) where TOptions : class
    {
        IOptionsWriter.Default.Write();
    }

    public static void UpdateAndWrite<TOptions>(this IOptions<TOptions> options, string name, TOptions? instance = null) where TOptions : class
    {
        Update(options, name, instance);
        Write(options);
    }

    public static void UpdateAndWrite<TOptions>(this IOptions<TOptions> options, TOptions? instance = null) where TOptions : class
    {
        UpdateAndWrite(options, typeof(TOptions).Name, instance);
    }

    public static void Bind<TOptions, TTarget>(this IOptions<TOptions> options, TTarget target, string propertyName)
       where TOptions : class where TTarget : INotifyPropertyChanged
    {
        options.Bind(target, propertyName, propertyName);
    }

    public static void Bind<TOptions, TTarget>(this IOptions<TOptions> options, TTarget target, string sourcePropertyName, string targetPropertyName)
       where TOptions : class where TTarget : INotifyPropertyChanged
    {
        var sourcePropertyInfo = typeof(TOptions).GetProperty(sourcePropertyName);
        var targetPropertyInfo = typeof(TTarget).GetProperty(targetPropertyName);

        ArgumentNullException.ThrowIfNull(sourcePropertyInfo);
        ArgumentNullException.ThrowIfNull(targetPropertyInfo);

        targetPropertyInfo.SetValue(target, sourcePropertyInfo.GetValue(options.Value));

        target.PropertyChanged -= Target_PropertyChanged;
        target.PropertyChanged += Target_PropertyChanged;
        void Target_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == targetPropertyName)
            {
                sourcePropertyInfo.SetValue(options.Value, targetPropertyInfo.GetValue(target));
            }
        }
    }

    public static void Bind<TOptions, TTarget>(this IOptions<TOptions> options, TTarget target)
       where TOptions : class where TTarget : INotifyPropertyChanged
    {
        foreach (var sourcePropertyInfo in typeof(TOptions).GetProperties())
        {
            ArgumentNullException.ThrowIfNull(sourcePropertyInfo);

            var propertyName = sourcePropertyInfo.Name;
            var targetPropertyInfo = typeof(TTarget).GetProperty(propertyName);

            ArgumentNullException.ThrowIfNull(targetPropertyInfo);

            targetPropertyInfo.SetValue(target, sourcePropertyInfo.GetValue(options.Value));
        }

        target.PropertyChanged -= Target_PropertyChanged;
        target.PropertyChanged += Target_PropertyChanged;
        void Target_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var propertyName = e.PropertyName;
            var sourcePropertyInfo = typeof(TOptions).GetProperties().SingleOrDefault(p => p.Name == propertyName);
            sourcePropertyInfo?.SetValue(options.Value, typeof(TTarget).GetProperty(propertyName!)?.GetValue(target));
        }
    }
}
