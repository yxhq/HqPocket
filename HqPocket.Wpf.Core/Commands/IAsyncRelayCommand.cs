﻿using System.ComponentModel;
using System.Threading.Tasks;

namespace HqPocket.Wpf.Commands;

public interface IAsyncRelayCommand : IRelayCommand, INotifyPropertyChanged
{
    Task? ExecutionTask { get; }
    bool CanBeCanceled { get; }
    bool IsCancellationRequested { get; }
    bool IsRunning { get; }
    Task ExecuteAsync(object? parameter);
    void Cancel();
}