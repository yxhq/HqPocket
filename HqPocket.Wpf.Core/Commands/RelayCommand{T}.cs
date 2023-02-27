using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace HqPocket.Wpf.Commands;

public sealed class RelayCommand<T> : IRelayCommand<T>
{
    private readonly bool _useCommandManager;
    private readonly Action<T> _execute;
    private readonly Func<T, bool>? _canExecute;

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

    public RelayCommand(Action<T> execute, bool useCommandManager = true)
        : this(execute, null, useCommandManager)
    {
    }

    public RelayCommand(Action<T> execute, Func<T, bool>? canExecute, bool useCommandManager = true)
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
    public bool CanExecute(T parameter)
    {
        return _canExecute?.Invoke(parameter) != false;
    }

    public bool CanExecute(object? parameter)
    {
        if (typeof(T).IsValueType && parameter is null && _canExecute is null)
        {
            return true;
        }

        return CanExecute((T)parameter!);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(T parameter)
    {
        if (CanExecute(parameter))
        {
            _execute(parameter);
        }
    }

    public void Execute(object? parameter)
    {
        Execute((T)parameter!);
    }
}