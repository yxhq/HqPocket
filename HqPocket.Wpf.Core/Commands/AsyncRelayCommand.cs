using HqPocket.Mvvm.ComponentModel;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HqPocket.Wpf.Commands;

public sealed class AsyncRelayCommand : ObservableObject, IAsyncRelayCommand
{
    internal static readonly PropertyChangedEventArgs CanBeCanceledChangedEventArgs = new(nameof(CanBeCanceled));
    internal static readonly PropertyChangedEventArgs IsCancellationRequestedChangedEventArgs = new(nameof(IsCancellationRequested));
    internal static readonly PropertyChangedEventArgs IsRunningChangedEventArgs = new(nameof(IsRunning));

    private readonly bool _useCommandManager;
    private readonly Func<Task>? _execute;
    private readonly Func<CancellationToken, Task>? _cancelableExecute;
    private readonly Func<bool>? _canExecute;
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

    public bool CanBeCanceled => _cancelableExecute is not null && IsRunning;

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
                OnPropertyChanged(IsRunningChangedEventArgs);
                OnPropertyChanged(CanBeCanceledChangedEventArgs);
            }))
            {
                // When setting the task
                OnPropertyChanged(IsRunningChangedEventArgs);
                OnPropertyChanged(CanBeCanceledChangedEventArgs);
            }
        }
    }

    public AsyncRelayCommand(Func<Task>? execute, bool useCommandManager = true)
        : this(execute, null, useCommandManager)
    {
    }

    public AsyncRelayCommand(Func<Task>? execute, Func<bool>? canExecute, bool useCommandManager = true)
    {
        _execute = execute;
        _canExecute = canExecute;
        _useCommandManager = useCommandManager;
    }

    public AsyncRelayCommand(Func<CancellationToken, Task>? cancelableExecute, bool useCommandManager = true)
        : this(cancelableExecute, null, useCommandManager)
    {
    }

    public AsyncRelayCommand(Func<CancellationToken, Task>? cancelableExecute, Func<bool>? canExecute, bool useCommandManager = true)
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
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() != false;
    }

    public void Execute(object? parameter)
    {
        ExecuteAsync(parameter);
    }

    public Task ExecuteAsync(object? parameter)
    {
        if (CanExecute(parameter))
        {
            if (_execute is not null)
            {
                return ExecutionTask = _execute();
            }
            _cancellationTokenSource?.Cancel();
            var cancellationTokenSource = _cancellationTokenSource = new CancellationTokenSource();
            OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
            return ExecutionTask = _cancelableExecute!(cancellationTokenSource.Token);
        }
        return Task.CompletedTask;
    }

    public void Cancel()
    {
        _cancellationTokenSource?.Cancel();
        OnPropertyChanged(IsCancellationRequestedChangedEventArgs);
        OnPropertyChanged(CanBeCanceledChangedEventArgs);
    }
}