using HqPocket.Wpf.Windows;

using System;

namespace HqPocket.Extensions.Dialoging;

public interface IDialoger
{
    void Show<TViewModel>(Action<IDialogWindowOptions>? windowSetupAction, Action<TViewModel>? viewModelSetupAction, bool isModal = true, params DialogCommand<TViewModel>[]? commands)
        where TViewModel : class, IDialogViewModel;
}