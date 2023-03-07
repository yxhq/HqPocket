using HqPocket.Wpf.Windows;

using System.Windows;

namespace HqPocket.Extensions.Dialoging;

public class DefaultDialogWindow : Window, IDialogWindow
{
    public HorizontalAlignment ButtonHorizontalAlignment { get; set; }
    public Thickness ButtonPadding { get; set; }
    public Thickness ButtonMargin { get; set; }

    public DefaultDialogWindow()
    {
        ResizeMode = ResizeMode.NoResize;
        SizeToContent = SizeToContent.WidthAndHeight;
        WindowState = WindowState.Normal;
        ButtonHorizontalAlignment = HorizontalAlignment.Center;
        ButtonPadding = new Thickness(20, 8, 20, 8);
        ButtonMargin = new Thickness(10);
    }
}