using System;
using System.Windows.Media;

namespace HqPocket.Extensions.Dialoging.ViewModels;

public class MessageDialogViewModel : IDialogViewModel
{
    public event EventHandler? RequestCloseDialog;
    public string? Message { get; set; }
    public ImageSource? Icon { get; set; }

    public virtual void OnDialogClosed()
    {

    }
}