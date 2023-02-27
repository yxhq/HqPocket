
using System.Windows;

namespace HqPocket.Wpf.Windows;

public interface IDialogWindowOptions
{
    double Width { get; set; }
    double Height { get; set; }
    string Title { get; set; }
    ResizeMode ResizeMode { get; set; }
    SizeToContent SizeToContent { get; set; }
    WindowState WindowState { get; set; }
    WindowStyle WindowStyle { get; set; }


    private static HorizontalAlignment _buttonHorizontalAlignment;
    HorizontalAlignment ButtonHorizontalAlignment
    {
        get => _buttonHorizontalAlignment;
        set => _buttonHorizontalAlignment = value;
    }

    private static Thickness _buttonPadding;
    Thickness ButtonPadding
    {
        get => _buttonPadding;
        set => _buttonPadding = value;
    }

    private static Thickness _buttonMargin;
    Thickness ButtonMargin
    {
        get => _buttonMargin;
        set => _buttonMargin = value;
    }
    public void Initialize()
    {
        ResizeMode = ResizeMode.NoResize;
        SizeToContent = SizeToContent.WidthAndHeight;
        WindowState = WindowState.Normal;
        ButtonHorizontalAlignment = HorizontalAlignment.Center;
        ButtonPadding = new Thickness(20, 8, 20, 8);
        ButtonMargin = new Thickness(10);
    }
}