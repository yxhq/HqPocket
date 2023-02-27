using HqPocket.Extensions.Dialoging;
using HqPocket.Extensions.Dialoging.ViewModels;
using HqPocket.Extensions.Dialoging.Views;
using HqPocket.Wpf.Windows;

using Microsoft.Extensions.DependencyInjection.Extensions;

using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class DialogingServiceCollectionExtensions
{
    public static IServiceCollection AddDialoging<TWindow>(this IServiceCollection services)
        where TWindow : class, IDialogWindow
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddTransient<MessageDialogViewModel>();
        services.TryAddTransient<MessageDialogView>();
        services.TryAddTransient<IDialoger, Dialoger>();
        services.AddTransient<IDialogWindow, TWindow>();

        return services;
    }

    public static IServiceCollection AddDialoging(this IServiceCollection services)
    {
        return services.AddDialoging<DefaultDialogWindow>();
    }
}