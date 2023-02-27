
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HqPocket.Mvvm;

public abstract class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event PropertyChangingEventHandler? PropertyChanging;

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, e);
    }

    protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
    {
        PropertyChanging?.Invoke(this, e);
    }

    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    protected virtual void RaisePropertyChanging([CallerMemberName] string? propertyName = null)
    {
        OnPropertyChanging(new PropertyChangingEventArgs(propertyName));
    }

    protected bool SetValue<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        return SetValue(ref storage, value, null, propertyName);
    }

    protected bool SetValue<T>(ref T storage, T value, Action<T>? onChanged, [CallerMemberName] string? propertyName = null)
    {
        return SetValue(ref storage, value, null, onChanged, propertyName);
    }

    protected bool SetValue<T>(ref T storage, T value, Action<T>? onChanging, Action<T>? onChanged, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        RaisePropertyChanging(propertyName);
        onChanging?.Invoke(storage);
        storage = value;
        RaisePropertyChanged(propertyName);
        onChanged?.Invoke(storage);
        return true;
    }

    /// <summary>
    /// Compares the current and new values for a given nested property. If the value has changed,
    /// raises the <see cref="PropertyChanging"/> event, updates the property and then raises the
    /// <see cref="PropertyChanged"/> event. The behavior mirrors that of <see cref="SetProperty{T}(ref T,T,string)"/>,
    /// with the difference being that this method is used to relay properties from a wrapped model in the
    /// current instance. This type is useful when creating wrapping, bindable objects that operate over
    /// models that lack support for notification (eg. for CRUD operations).
    /// Suppose we have this model (eg. for a database row in a table):
    /// <code>
    /// public class Person
    /// {
    ///     public string Name { get; set; }
    /// }
    /// </code>
    /// We can then use a property to wrap instances of this type into our observable model (which supports
    /// notifications), injecting the notification to the properties of that model, like so:
    /// <code>
    /// public class BindablePerson : ObservableObject
    /// {
    ///     public Model { get; }
    ///
    ///     public BindablePerson(Person model)
    ///     {
    ///         Model = model;
    ///     }
    ///
    ///     public string Name
    ///     {
    ///         get => Model.Name;
    ///         set => Set(Model.Name, value, Model, (model, name) => model.Name = name);
    ///     }
    /// }
    /// </code>
    /// This way we can then use the wrapping object in our application, and all those "proxy" properties will
    /// also raise notifications when changed. Note that this method is not meant to be a replacement for
    /// <see cref="SetProperty{T}(ref T,T,string)"/>, and it should only be used when relaying properties to a model that
    /// doesn't support notifications, and only if you can't implement notifications to that model directly (eg. by having
    /// it inherit from <see cref="ObservableObject"/>). The syntax relies on passing the target model and a stateless callback
    /// to allow the C# compiler to cache the function, which results in much better performance and no memory usage.
    /// </summary>
    /// <typeparam name="TModel">The type of model whose property (or field) to set.</typeparam>
    /// <typeparam name="T">The type of property (or field) to set.</typeparam>
    /// <param name="oldValue">The current property value.</param>
    /// <param name="newValue">The property's value after the change occurred.</param>
    /// <param name="model">The model containing the property being updated.</param>
    /// <param name="callback">The callback to invoke to set the target property value, if a change has occurred.</param>
    /// <param name="propertyName">(optional) The name of the property that changed.</param>
    /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
    /// <remarks>
    /// The <see cref="PropertyChanging"/> and <see cref="PropertyChanged"/> events are not
    /// raised if the current and new value for the target property are the same.
    /// </remarks>
    /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="model"/> or <paramref name="callback"/> are <see langword="null"/>.</exception>
    protected bool SetValue<TModel, T>(T oldValue, T newValue, TModel model, Action<TModel, T> callback, [CallerMemberName] string? propertyName = null)
        where TModel : class
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(callback);

        if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
        {
            return false;
        }

        RaisePropertyChanging(propertyName);

        callback(model, newValue);

        RaisePropertyChanged(propertyName);

        return true;
    }

    protected bool SetValueAndNotifyOnCompletion(ref TaskNotifier? taskNotifier, Task? value, [CallerMemberName] string? propertyName = null)
    {
        return SetValueAndNotifyOnCompletion(taskNotifier ??= new TaskNotifier(), value, _ => { }, propertyName);
    }

    protected bool SetValueAndNotifyOnCompletion(ref TaskNotifier? taskNotifier, Task? value, Action<Task?> callBack, [CallerMemberName] string? propertyName = null)
    {
        return SetValueAndNotifyOnCompletion(taskNotifier ??= new TaskNotifier(), value, callBack, propertyName);
    }

    protected bool SetValueAndNotifyOnCompletion<T>(ref TaskNotifier<T>? taskNotifier, Task<T>? value, [CallerMemberName] string? propertyName = null)
    {
        return SetValueAndNotifyOnCompletion(taskNotifier ??= new TaskNotifier<T>(), value, _ => { }, propertyName);
    }

    protected bool SetValueAndNotifyOnCompletion<T>(ref TaskNotifier<T>? taskNotifier, Task<T>? value, Action<Task<T>?> callBack, [CallerMemberName] string? propertyName = null)
    {
        return SetValueAndNotifyOnCompletion(taskNotifier ??= new TaskNotifier<T>(), value, callBack, propertyName);
    }

    private bool SetValueAndNotifyOnCompletion<TTask>(ITaskNotifier<TTask> taskNotifier, TTask? newValue, Action<TTask?> callBack, [CallerMemberName] string? propertyName = null)
        where TTask : Task
    {
        if (ReferenceEquals(taskNotifier.Task, newValue))
        {
            return false;
        }

        bool isAlreadyCompleteOrNull = newValue?.IsCompleted ?? true;
        RaisePropertyChanging(propertyName);
        taskNotifier.Task = newValue;
        RaisePropertyChanged(propertyName);

        if (isAlreadyCompleteOrNull)
        {
            callBack(newValue);
            return true;
        }

        async void MonitorTask()
        {
            try
            {
                //Await the task and ignore any exceptions
                await newValue!;
            }
            catch
            {

            }

            //Only notify if the property hasn't changed
            if (ReferenceEquals(taskNotifier.Task, newValue))
            {
                RaisePropertyChanged(propertyName);
            }
            callBack(newValue);
        }

        MonitorTask();
        return true;
    }

    private interface ITaskNotifier<TTask> where TTask : Task
    {
        TTask? Task { get; set; }
    }

    protected sealed class TaskNotifier : ITaskNotifier<Task>
    {
        private Task? _task;
        Task? ITaskNotifier<Task>.Task
        {
            get => _task;
            set => _task = value;
        }

        internal TaskNotifier()
        {
        }

        public static implicit operator Task?(TaskNotifier? notifier)
        {
            return notifier?._task;
        }
    }

    protected sealed class TaskNotifier<T> : ITaskNotifier<Task<T>>
    {
        private Task<T>? _task;
        Task<T>? ITaskNotifier<Task<T>>.Task
        {
            get => _task;
            set => _task = value;
        }

        internal TaskNotifier()
        {
        }

        public static implicit operator Task<T>?(TaskNotifier<T>? notifier)
        {
            return notifier?._task;
        }
    }
}
