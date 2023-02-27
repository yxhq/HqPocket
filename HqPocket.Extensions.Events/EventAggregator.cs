using System;
using System.Collections.Concurrent;
using System.Threading;

namespace HqPocket.Extensions.Events;

/// <summary>
/// Implements <see cref="IEventAggregator"/>.
/// </summary>
public class EventAggregator : IEventAggregator
{
    private readonly ConcurrentDictionary<Type, EventBase> _events = new();
    // Captures the sync context for the UI thread when constructed on the UI thread 
    // in a platform agnositc way so it can be used for UI thread dispatching
    private readonly SynchronizationContext? _syncContext = SynchronizationContext.Current;

    /// <summary>
    /// Gets the single instance of the event managed by this EventAggregator. Multiple calls to this method with the same <typeparamref name="TEventType"/> returns the same event instance.
    /// </summary>
    /// <typeparam name="TEventType">The type of event to get. This must inherit from <see cref="EventBase"/>.</typeparam>
    /// <returns>A singleton instance of an event object of type <typeparamref name="TEventType"/>.</returns>
    public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
    {
        return (_events.GetOrAdd(typeof(TEventType), new TEventType { SynchronizationContext = _syncContext }) as TEventType)!;
    }
}
