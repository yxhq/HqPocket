using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HqPocket.Extensions.Events;

///<summary>
/// Defines a base class to publish and subscribe to events.
///</summary>
public abstract class EventBase
{
    private readonly List<IEventSubscription> _subscriptions = new();

    /// <summary>
    /// Allows the SynchronizationContext to be set by the EventAggregator for UI Thread Dispatching
    /// </summary>
    public SynchronizationContext? SynchronizationContext { get; set; }

    /// <summary>
    /// Gets the list of current subscriptions.
    /// </summary>
    /// <value>The current subscribers.</value>
    protected ICollection<IEventSubscription> Subscriptions => _subscriptions;

    /// <summary>
    /// Adds the specified <see cref="IEventSubscription"/> to the subscribers' collection.
    /// </summary>
    /// <param name="eventSubscription">The subscriber.</param>
    /// <returns>The <see cref="SubscriptionToken"/> that uniquely identifies every subscriber.</returns>
    /// <remarks>
    /// Adds the subscription to the internal list and assigns it a new <see cref="SubscriptionToken"/>.
    /// </remarks>
    protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
    {
        ArgumentNullException.ThrowIfNull(eventSubscription);

        eventSubscription.SubscriptionToken = new SubscriptionToken(Unsubscribe);

        lock (Subscriptions)
        {
            Subscriptions.Add(eventSubscription);
        }
        return eventSubscription.SubscriptionToken;
    }

    /// <summary>
    /// Calls all the execution strategies exposed by the list of <see cref="IEventSubscription"/>.
    /// </summary>
    /// <param name="arguments">The arguments that will be passed to the listeners.</param>
    /// <remarks>Before executing the strategies, this class will prune all the subscribers from the
    /// list that return a <see langword="null" /> <see cref="Action{T}"/> when calling the
    /// <see cref="IEventSubscription.GetExecutionStrategy"/> method.</remarks>
    protected virtual void InternalPublish(params object?[] arguments)
    {
        List<Action<object?[]>> executionStrategies = PruneAndReturnStrategies();
        foreach (var executionStrategy in executionStrategies)
        {
            executionStrategy(arguments);
        }
    }

    /// <summary>
    /// Removes the subscriber matching the <see cref="SubscriptionToken"/>.
    /// </summary>
    /// <param name="token">The <see cref="SubscriptionToken"/> returned by <see cref="EventBase"/> while subscribing to the event.</param>
    public virtual void Unsubscribe(SubscriptionToken token)
    {
        lock (Subscriptions)
        {
            IEventSubscription? subscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
            if (subscription is not null)
            {
                Subscriptions.Remove(subscription);
            }
        }
    }

    /// <summary>
    /// Returns <see langword="true"/> if there is a subscriber matching <see cref="SubscriptionToken"/>.
    /// </summary>
    /// <param name="token">The <see cref="SubscriptionToken"/> returned by <see cref="EventBase"/> while subscribing to the event.</param>
    /// <returns><see langword="true"/> if there is a <see cref="SubscriptionToken"/> that matches; otherwise <see langword="false"/>.</returns>
    public virtual bool Contains(SubscriptionToken token)
    {
        lock (Subscriptions)
        {
            IEventSubscription? subscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
            return subscription is not null;
        }
    }

    private List<Action<object?[]>> PruneAndReturnStrategies()
    {
        List<Action<object?[]>> returnList = new();

        lock (Subscriptions)
        {
            for (var i = Subscriptions.Count - 1; i >= 0; i--)
            {
                Action<object?[]>? listItem = _subscriptions[i].GetExecutionStrategy();

                if (listItem is null)
                {
                    // Prune from main list. Log?
                    _subscriptions.RemoveAt(i);
                }
                else
                {
                    returnList.Add(listItem);
                }
            }
        }

        return returnList;
    }

    /// <summary>
    /// Forces the PubSubEvent to remove any subscriptions that no longer have an execution strategy.
    /// </summary>
    public void Prune()
    {
        lock (Subscriptions)
        {
            for (var i = Subscriptions.Count - 1; i >= 0; i--)
            {
                if (_subscriptions[i].GetExecutionStrategy() is null)
                {
                    _subscriptions.RemoveAt(i);
                }
            }
        }
    }
}