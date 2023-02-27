using System;

namespace HqPocket.Extensions.Dialoging;

public interface IDialogViewModel
{
    event EventHandler? RequestCloseDialog;
    bool CanCloseDialog() => true;

    void OnDialogClosed()
    {
    }
}