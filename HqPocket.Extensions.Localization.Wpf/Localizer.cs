using System.Globalization;
using System.Windows.Data;
using System.Windows;

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

    public static string GetString<T>(string name) => _localizerFactory.Create(typeof(T))[name].Value;

    public static void Bind<T>(DependencyObject dependencyObject, DependencyProperty dependencyProperty, string name)
    {
        var localizer = _localizerFactory.Create(typeof(T));
        BindingOperations.SetBinding(dependencyObject, dependencyProperty, new Binding($"[{name}]")
        {
            Source = localizer,
            Mode = BindingMode.OneWay
        });
    }
}
