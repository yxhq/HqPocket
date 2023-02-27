using HqPocket.Mvvm;

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HqPocket.Wpf.Commands;

public sealed class AsyncRelayCommand<T> : ObservableObject, IAsyncRelayCommand<T>
{
    private readonly bool _useCommandManager;
    private readonly Func<T, Task>? _execute;
    private readonly Func<T, CancellationToken, Task>? _cancelableExecute;
    private readonly Func<T, bool>? _canExecute;
    private CancellationTokenSource? _cancellationTokenSource;

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

    public bool CanBeCanceled => !(_cancelableExecute is null) && IsRunning;

    public bool IsCancellationRequested => _cancellationTokenSource?.IsCancellationRequested == true;

    public bool IsRunning => ExecutionTask?.IsCompleted == false;

    private TaskNotifier? _executionTask;
    public Task? ExecutionTask
    {
        get => _executionTask;
        private set
        {
            if (SetValueAndNotifyOnCompletion(ref _executionTask, value, _ =>
            {
                // When the task completes
                OnPropertyChanged(AsyncRelayCommand.IsRunningChangedEventArgs);
                OnPropertyChanged(AsyncRelayCommand.CanBeCanceledChangedEventArgs);
            }))
            {
                // When setting the task
                OnPropertyChanged(AsyncRelayCommand.IsRunningChangedEventArgs);
                OnPropertyChanged(AsyncRelayCommand.CanBeCanceledChangedEventArgs);
            }
        }
    }

    public AsyncRelayCommand(Func<T, Task>? execute, bool useCommandManager = true)
        : this(execute, null, useCommandManager)
    {
    }

    public AsyncRelayCommand(Func<T, Task>? execute, Func<T, bool>? canExecute, bool useCommandManager = true)
    {
        _execute = execute;
        _canExecute = canExecute;
        _useCommandManager = useCommandManager;
    }

    public AsyncRelayCommand(Func<T, CancellationToken, Task>? cancelableExecute, bool useCommandManager = true)
        : this(cancelableExecute, null, useCommandManager)
    {
    }

    public AsyncRelayCommand(Func<T, CancellationToken, Task>? cancelableExecute, Func<T, bool>? canExecute, bool useCommandManager = true)
    {
        _cancelableExecute = cancelableExecute;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        ExecuteAsync(parameter);
    }

    public void Execute(object? parameter)
    {
        ExecuteAsync((T)parameter!);
    }

    public Task ExecuteAsync(T parameter)
    {
        if (CanExecute(parameter))
        {
            // Non cancelable command delegate
            if (_execute is not null)
            {
                return ExecutionTask = _execute(parameter);
            }

            // Cancel the previous operation, if one is pending
            _cancellationTokenSource?.Cancel();

            var cancellationTokenSource = _cancellationTokenSource = new CancellationTokenSource();

            OnPropertyChanged(AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);

            // Invoke the cancelable command delegate with a new linked token
            return ExecutionTask = _cancelableExecute!(parameter, cancellationTokenSource.Token);
        }

        return Task.CompletedTask;
    }

    public Task ExecuteAsync(object? parameter)
    {
        return ExecuteAsync((T)parameter!);
    }

    public void Cancel()
    {
        _cancellationTokenSource?.Cancel();

        OnPropertyChanged(AsyncRelayCommand.IsCancellationRequestedChangedEventArgs);
        OnPropertyChanged(AsyncRelayCommand.CanBeCanceledChangedEventArgs);
    }
}