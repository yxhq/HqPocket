using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HqPocket.Wpf.Commands;

public sealed class RelayCommand : IRelayCommand
{
    private readonly bool _useCommandManager;
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;
    private event EventHandler? InternalCanExecuteChanged;

    public event EventHandler? CanExecuteChanged
    {
        add
        {
            if (_canExecute is null) return;
            if (_useCommandManager)
            {
                CommandManager.RequerySuggested += value;
            }
            else
            {
                InternalCanExecuteChanged += value;
            }
        }
        remove
        {
            if (_canExecute is null) return;
            if (_useCommandManager)
            {
                CommandManager.RequerySuggested -= value;
            }
            else
            {
                InternalCanExecuteChanged -= value;
            }
        }
    }

    public RelayCommand(Action execute, bool useCommandManager = true)
        : this(execute, null, useCommandManager)
    {
    }

    public RelayCommand(Action execute, Func<bool>? canExecute, bool useCommandManager = true)
    {
        _execute = execute;
        _canExecute = canExecute;
        _useCommandManager = useCommandManager;
    }

    public void RaiseCanExecuteChanged()
    {
        if (_useCommandManager)
        {
            CommandManager.InvalidateRequerySuggested();
        }
        else
        {
            InternalCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() != false;
    }

    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute();
        }
    }
}