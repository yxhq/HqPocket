
using HqPocket.Mvvm;
using HqPocket.Wpf.Commands;
using HqPocket.Wpf.Windows;

using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace HqPocket.Extensions.Dialoging;

public class Dialoger : IDialoger
{
    public static IDialoger Instance { get; } = Ioc.GetRequiredService<IDialoger>();

    public void Show<TViewModel>(Action<IDialogWindowOptions>? windowSetupAction, Action<TViewModel>? viewModelSetupAction, bool isModal = true, params DialogCommand<TViewModel>[]? commands)
        where TViewModel : class, IDialogViewModel
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            var dialog = Create(windowSetupAction, viewModelSetupAction, commands);
            if (isModal)
                dialog.ShowDialog();
            else
                dialog.Show();
        });
    }

    private IDialogWindow Create<TViewModel>(Action<IDialogWindowOptions>? windowSetupAction, Action<TViewModel>? viewModelSetupAction, params DialogCommand<TViewModel>[]? commands)
        where TViewModel : class, IDialogViewModel
    {
        var dialogWindow = Ioc.GetRequiredService<IDialogWindow>();

        var viewType = VvmTypeLocationProvider.GetMappedType<TViewModel>();
        ArgumentNullException.ThrowIfNull(viewType);

        var view = Ioc.GetRequiredService(viewType) as FrameworkElement;
        ArgumentNullException.ThrowIfNull(view);

        if (view.DataContext is not TViewModel viewModel)
        {
            viewModel = Ioc.GetRequiredService<TViewModel>();
            view.DataContext = viewModel;
        }

        var ownerWindow = (dialogWindow as FrameworkElement)?.GetOwnerWindow();
        ConfigureOwner(dialogWindow, ownerWindow);

        windowSetupAction?.Invoke(dialogWindow);

        viewModel.RequestCloseDialog += ViewModel_RequestClose;
        void ViewModel_RequestClose(object? sender, EventArgs e) => dialogWindow.Close();

        dialogWindow.Closing += DialogWindow_Closing;
        void DialogWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (!viewModel.CanCloseDialog())
            {
                e.Cancel = true;
            }
        }

        dialogWindow.Closed += DialogWindow_Closed;
        void DialogWindow_Closed(object? sender, EventArgs e)
        {
            dialogWindow.Closed -= DialogWindow_Closed;
            dialogWindow.Closing -= DialogWindow_Closing;
            viewModel.RequestCloseDialog -= ViewModel_RequestClose;
            viewModel.OnDialogClosed();
            dialogWindow.Content = null;
            ownerWindow?.Activate();
        }

        viewModelSetupAction?.Invoke(viewModel);

        DockPanel dockPanel = new();
        if (commands?.Length > 0)
        {
            UniformGrid grid = new()
            {
                Columns = commands.Length,
                HorizontalAlignment = dialogWindow.ButtonHorizontalAlignment,
                VerticalAlignment = VerticalAlignment.Center
            };

            foreach (var command in commands)
            {
                Button button = new()
                {
                    Content = command.Content,
                    IsDefault = command.IsDefault,
                    Padding = dialogWindow.ButtonPadding,
                    Margin = dialogWindow.ButtonMargin,
                    VerticalAlignment = VerticalAlignment.Center
                };

                void InteractCommandAction(TViewModel vm)
                {
                    bool allowCloseDialog = true;
                    if (command.Name is not null)
                    {
                        var methodInfo = vm.GetType().GetMethod($"On{command.Name}", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (methodInfo is not null && methodInfo.Invoke(vm, null) is bool b)
                        {
                            allowCloseDialog = b; //此处决定是否调用 dialogWindow.Close()
                        }
                    }
                    if (allowCloseDialog)
                    {
                        command.Execute?.Invoke(vm);
                        dialogWindow?.Close();
                    }
                }

                button.Command = new RelayCommand<TViewModel>(InteractCommandAction, command.CanExecute);
                button.CommandParameter = viewModel;
                grid.Children.Add(button);
            }
            dockPanel.Children.Add(grid);
            DockPanel.SetDock(grid, Dock.Bottom);
        }

        dockPanel.Children.Add(view);
        dialogWindow.Content = dockPanel;
        return dialogWindow;
    }

    private static void ConfigureOwner(IDialogWindow currentWindow, Window? ownerWindow)
    {
        if (ownerWindow is not null)
        {
            currentWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            currentWindow.Owner = ownerWindow;
        }
        else
        {
            currentWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}