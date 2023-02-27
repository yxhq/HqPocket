using System;
using System.ComponentModel;
using System.Windows;

namespace HqPocket.Wpf.Windows;

public interface IDialogWindow : IDialogWindowOptions
{
    event EventHandler Closed;
    event CancelEventHandler Closing;

    object? Content { get; set; }
    object DataContext { get; set; }
    Window Owner { get; set; }
    WindowStartupLocation WindowStartupLocation { get; set; }

    void Close();
    void Show();
    bool? ShowDialog();
}