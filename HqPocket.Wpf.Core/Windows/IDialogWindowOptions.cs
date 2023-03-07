
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
    HorizontalAlignment ButtonHorizontalAlignment { get; set; }
    Thickness ButtonPadding { get; set; }
    Thickness ButtonMargin { get; set; }
}