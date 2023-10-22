using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace HqPocket.Extensions.Localization;

public class Localizer
{
    private static readonly INotifiedStringLocalizerFactory _localizerFactory = Ioc.GetRequiredService<INotifiedStringLocalizerFactory>();

    public static event CultureChangedEventHandler? CultureChanged;

    public static void ChangeCulture(CultureInfo cultureInfo)
    {
        if (CultureInfo.CurrentCulture != cultureInfo || CultureInfo.CurrentUICulture != cultureInfo)
        {
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
            _localizerFactory.RefreshLocalizedString();
            CultureChanged?.Invoke(null, new CultureChangedEventArgs(cultureInfo));
        }
    }

    public static string GetString(Type viewType, string name) => _localizerFactory.Create(viewType)[name].Value;

    public static string GetString<T>(string name) => GetString(typeof(T), name);

    public static void Bind(Type resourceType, DependencyObject dependencyObject, DependencyProperty dependencyProperty, string name)
    {
        var localizer = _localizerFactory.Create(resourceType);
        BindingOperations.SetBinding(dependencyObject, dependencyProperty, new Binding($"[{name}].Value")
        {
            Source = localizer,
            Mode = BindingMode.OneWay
        });
    }

    public static void Bind<TResource>(DependencyObject dependencyObject, DependencyProperty dependencyProperty, string name)
        => Bind(typeof(TResource), dependencyObject, dependencyProperty, name);
}
