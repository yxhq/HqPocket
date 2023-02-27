using System;
using System.Windows;

using DialogString = HqPocket.Extensions.Dialoging.Resources.DialogSharedResource;

namespace HqPocket.Extensions.Dialoging;

public class DialogCommandHelper
{
    public static DialogCommand<TViewModel>[] CreateDialogCommands<TViewModel>(Action<TViewModel>? yesOkCallback, Action<TViewModel>? noCallback, MessageBoxButton button)
        where TViewModel : IDialogViewModel
    {
        DialogCommand<TViewModel> ok = Ok(yesOkCallback);
        DialogCommand<TViewModel> yes = Yes(yesOkCallback);
        DialogCommand<TViewModel> no = No(noCallback);
        DialogCommand<TViewModel> cancel = Cancel<TViewModel>();

        return button switch
        {
            MessageBoxButton.OK => new[] { ok },
            MessageBoxButton.OKCancel => new[] { ok, cancel },
            MessageBoxButton.YesNoCancel => new[] { yes, no, cancel },
            MessageBoxButton.YesNo => new[] { yes, no },
            _ => Array.Empty<DialogCommand<TViewModel>>()
        };
    }

    public static DialogCommand<TViewModel> Ok<TViewModel>(Action<TViewModel>? callback = null, Func<TViewModel, bool>? canExecute = null)
        where TViewModel : IDialogViewModel
    {
        return new()
        {
            Name = "Ok",
            Content = DialogString.Ok,
            Execute = callback,
            CanExecute = canExecute,
            IsDefault = true
        };
    }

    public static DialogCommand<TViewModel> Yes<TViewModel>(Action<TViewModel>? callback = null, Func<TViewModel, bool>? canExecute = null)
        where TViewModel : IDialogViewModel
    {
        return new()
        {
            Name = "Yes",
            Content = DialogString.Yes,
            Execute = callback,
            CanExecute = canExecute,
            IsDefault = true
        };
    }

    public static DialogCommand<TViewModel> No<TViewModel>(Action<TViewModel>? callback = null, Func<TViewModel, bool>? canExecute = null)
        where TViewModel : IDialogViewModel
    {
        return new()
        {
            Name = "No",
            Content = DialogString.No,
            Execute = callback,
            CanExecute = canExecute
        };
    }

    public static DialogCommand<TViewModel> Cancel<TViewModel>(Action<TViewModel>? callback = null)
        where TViewModel : IDialogViewModel
    {
        return new()
        {
            Name = "Cancel",
            Content = DialogString.Cancel,
            Execute = callback
        };
    }
}