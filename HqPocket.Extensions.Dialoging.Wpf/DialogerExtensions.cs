using HqPocket.Extensions.Dialoging.ViewModels;

using System;
using System.Media;
using System.Windows;
using System.Windows.Media;

using DialogerString = HqPocket.Extensions.Dialoging.Resources.DialogSharedResource;

namespace HqPocket.Extensions.Dialoging;

public static partial class DialogerExtensions
{
    public static void Show<TViewModel>(this IDialoger dialoger, string title, Action<TViewModel>? viewModelSetupAction, bool isModal = true, params DialogCommand<TViewModel>[]? commands)
        where TViewModel : class, IDialogViewModel
    {
        dialoger.Show(w => w.Title = title, viewModelSetupAction, isModal, commands);
    }

    public static void ShowYesNoCancel<TViewModel>(this IDialoger dialoger, string title, Action<TViewModel>? viewModelSetupAction, Action<TViewModel>? yesCallback = null, Action<TViewModel>? noCallback = null, bool isModal = true)
        where TViewModel : class, IDialogViewModel
    {
        Show(dialoger, title, viewModelSetupAction, isModal, DialogCommandHelper.CreateDialogCommands(yesCallback, noCallback, MessageBoxButton.YesNoCancel));
    }

    public static void ShowYesNo<TViewModel>(this IDialoger dialoger, string title, Action<TViewModel>? viewModelSetupAction, Action<TViewModel>? yesCallback = null, Action<TViewModel>? noCallback = null, bool isModal = true)
        where TViewModel : class, IDialogViewModel
    {
        Show(dialoger, title, viewModelSetupAction, isModal, DialogCommandHelper.CreateDialogCommands(yesCallback, noCallback, MessageBoxButton.YesNo));
    }

    public static void ShowOkCancel<TViewModel>(this IDialoger dialoger, string title, Action<TViewModel>? viewModelSetupAction, Action<TViewModel>? okCallback = null, bool isModal = true)
        where TViewModel : class, IDialogViewModel
    {
        Show(dialoger, title, viewModelSetupAction, isModal, DialogCommandHelper.CreateDialogCommands(okCallback, null, MessageBoxButton.OKCancel));
    }

    public static void ShowOk<TViewModel>(this IDialoger dialoger, string title, Action<TViewModel>? viewModelSetupAction, Action<TViewModel>? okCallback = null, bool isModal = true)
        where TViewModel : class, IDialogViewModel
    {
        Show(dialoger, title, viewModelSetupAction, isModal, DialogCommandHelper.CreateDialogCommands(okCallback, null, MessageBoxButton.OK));
    }

    public static void ShowMessage(this IDialoger dialoger, string title, Action<MessageDialogViewModel>? viewModelSetupAction, Action<MessageDialogViewModel>? yesOkCallback, MessageBoxButton button)
    {
        var commands = DialogCommandHelper.CreateDialogCommands(yesOkCallback, null, button);
        SystemSounds.Asterisk.Play();

        Show(dialoger, title, viewModelSetupAction, true, commands);
    }

    public static void ShowMessage(this IDialoger dialoger, string title, string message, Action<MessageDialogViewModel>? okCallBack = null, ImageSource? image = null, MessageBoxButton button = MessageBoxButton.OK)
    {
        ShowMessage(dialoger, title, vm =>
        {
            vm.Message = message;
            if (image is not null)
            {
                vm.Icon = image;
            }
        }, okCallBack, button);
    }

    public static void ShowError(this IDialoger dialoger, string message, Action<MessageDialogViewModel>? okCallBack, ImageSource? image = null)
    {
        ShowMessage(dialoger, DialogerString.Error, message, okCallBack, image);
    }

    public static void ShowWarning(this IDialoger dialoger, string message, Action<MessageDialogViewModel>? okCallBack, ImageSource? image = null)
    {
        ShowMessage(dialoger, DialogerString.Warning, message, okCallBack, image);
    }

    public static void ShowNotification(this IDialoger dialoger, string message, Action<MessageDialogViewModel>? okCallBack, ImageSource? image = null)
    {
        ShowMessage(dialoger, DialogerString.Notification, message, okCallBack, image);
    }

    public static void ShowConfirmation(this IDialoger dialoger, string message, Action<MessageDialogViewModel>? okCallBack, ImageSource? image = null)
    {
        ShowMessage(dialoger, DialogerString.Confirmation, message, okCallBack, image, MessageBoxButton.OKCancel);
    }
}