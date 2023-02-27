using Microsoft.Win32;

using System;

namespace HqPocket.Extensions.Dialoging;

public static partial class DialogerExtensions
{
    public static void ShowSaveFile(this IDialoger dialoger, Action<SaveFileDialog> confirmedCallback, string filter = Constants.TxtAndAllFilter)
    {
        ShowSaveFile(dialoger, s => s.Filter = filter, confirmedCallback);
    }

    public static void ShowOpenFile(this IDialoger dialoger, Action<OpenFileDialog> confirmedCallback, string filter = Constants.TxtAndAllFilter)
    {
        ShowOpenFile(dialoger, s => s.Filter = filter, confirmedCallback);
    }

    public static void ShowSaveFile(this IDialoger dialoger, Action<SaveFileDialog> setupAction, Action<SaveFileDialog> confirmedCallback)
    {
        ShowFileDialog(dialoger, setupAction, confirmedCallback);
    }

    public static void ShowOpenFile(this IDialoger dialoger, Action<OpenFileDialog> setupAction, Action<OpenFileDialog> confirmedCallback)
    {
        ShowFileDialog(dialoger, setupAction, confirmedCallback);
    }

    public static void ShowFileDialog<TFileDialog>(this IDialoger dialoger, Action<TFileDialog>? setupAction, Action<TFileDialog>? confirmedCallback)
        where TFileDialog : FileDialog, new()
    {
        _ = dialoger;
        TFileDialog dialog = new();
        setupAction?.Invoke(dialog);

        if (dialog.ShowDialog() == true)
        {
            confirmedCallback?.Invoke(dialog);
        }
    }
}