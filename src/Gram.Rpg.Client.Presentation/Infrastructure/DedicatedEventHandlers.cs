using System;
using System.Collections.Generic;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Messaging;

namespace Gram.Rpg.Client.Presentation.Infrastructure
{
    public class DedicatedEventHandlers : MessageBus.IDedicatedEventHandlers
    {
        private static uint idCounter;

        private static bool AlreadySubscribed<T>(Action<T> subscriber, ICollection<Wrapper> actions)
        {
            foreach (var wrapper in actions)
                if (wrapper.Action.Equals(subscriber))
                {
                    G.LogWarning($"Trying to subscribe a handler twice to: {typeof(T).Name}. Subscription failed.");
                    return true;
                }

            return false;
        }

        private static void InvokeSubscribers<T>(object e, IList<Wrapper> subscribers)
        {
            var t = (T)e;

            // The act of invoking could cause subscribers to be add/edremoved from our list.
            // We ensure we don't invoke new subscribers by comparing their ID with our startingId.
            // If their ID is >= the startingId then they were added during this invocation loop and
            // we should ignore them.
            // We also guard against many subscribers being removed during one invocation - in such 
            // a scenario we can try to access beyond the bounds of the list and error. 

            var startingId = idCounter;

            for (var i = subscribers.Count - 1; i >= 0; i--)
                if (i < subscribers.Count)
                {
                    var wrapper = subscribers[i];
                    if (wrapper.Id < startingId)
                    {
                        var action = (Action<T>)wrapper.Action;

                        action.Invoke(t);
                    }
                }
        }

        private static void UnsubscribeFrom(object subscriber, ICollection<Wrapper> subscribers)
        {
            foreach (var wrapper in subscribers)
                if (wrapper.Action.Equals(subscriber))
                {
                    subscribers.Remove(wrapper);
                    return;
                }
        }

        private readonly List<Wrapper> every10thUpdateSubscribers;
        private readonly List<Wrapper> every60thUpdateSubscribers;
        private readonly List<Wrapper> fixedUpdateSubscribers;
        private readonly List<Wrapper> lateUpdateSubscribers;
        private readonly List<Wrapper> updateSubscribers;

        public DedicatedEventHandlers()
        {
            every10thUpdateSubscribers = new List<Wrapper>();
            every60thUpdateSubscribers = new List<Wrapper>();
            fixedUpdateSubscribers     = new List<Wrapper>();
            updateSubscribers          = new List<Wrapper>();
            lateUpdateSubscribers      = new List<Wrapper>();
        }

        public bool TryBroadcastEvent<T>(T e)
        {
            var eventType = typeof(T);

            if (eventType == typeof(FixedUpdateMessage))
            {
                InvokeSubscribers<T>(e, fixedUpdateSubscribers);
                return true;
            }

            if (eventType == typeof(UpdateMessage))
            {
                InvokeSubscribers<T>(e, updateSubscribers);
                return true;
            }

            if (eventType == typeof(LateUpdateMessage))
            {
                InvokeSubscribers<T>(e, lateUpdateSubscribers);
                return true;
            }

            if (eventType == typeof(Every10thUpdateMessage))
            {
                InvokeSubscribers<T>(e, every10thUpdateSubscribers);
                return true;
            }

            if (eventType == typeof(Every60thUpdateMessage))
            {
                InvokeSubscribers<T>(e, every60thUpdateSubscribers);
                return true;
            }

            return false;
        }

        public bool TrySubscribeEventFor<T>(Action<T> subscriber, IDisposer disposer) 
        {
            var eventType = typeof(T);

            if (eventType == typeof(FixedUpdateMessage))
            {
                SubscribeTo<T>(subscriber, fixedUpdateSubscribers, disposer);
                return true;
            }

            if (eventType == typeof(UpdateMessage))
            {
                SubscribeTo<T>(subscriber, updateSubscribers, disposer);
                return true;
            }

            if (eventType == typeof(LateUpdateMessage))
            {
                SubscribeTo<T>(subscriber, lateUpdateSubscribers, disposer);
                return true;
            }

            if (eventType == typeof(Every10thUpdateMessage))
            {
                SubscribeTo<T>(subscriber, every10thUpdateSubscribers, disposer);
                return true;
            }

            if (eventType == typeof(Every60thUpdateMessage))
            {
                SubscribeTo<T>(subscriber, every60thUpdateSubscribers, disposer);
                return true;
            }

            return false;
        }

        public bool TryUnsubscribeEventFor<T>(Action<T> subscriber) 
        {
            var eventType = typeof(T);

            if (eventType == typeof(FixedUpdateMessage))
            {
                UnsubscribeFrom(subscriber, fixedUpdateSubscribers);
                return true;
            }

            if (eventType == typeof(UpdateMessage))
            {
                UnsubscribeFrom(subscriber, updateSubscribers);
                return true;
            }

            if (eventType == typeof(LateUpdateMessage))
            {
                UnsubscribeFrom(subscriber, lateUpdateSubscribers);
                return true;
            }

            if (eventType == typeof(Every10thUpdateMessage))
            {
                UnsubscribeFrom(subscriber, every10thUpdateSubscribers);
                return true;
            }

            if (eventType == typeof(Every60thUpdateMessage))
            {
                UnsubscribeFrom(subscriber, every60thUpdateSubscribers);
                return true;
            }

            return false;
        }

        private void SubscribeTo<T>(object subscriber, ICollection<Wrapper> subscribers, IDisposer disposer)
        {
            var action = (Action<T>)subscriber;

            if (AlreadySubscribed(action, subscribers))
                return;


            subscribers.Add(new Wrapper(action, idCounter++));

            disposer?.Add(() => TryUnsubscribeEventFor(action));
        }


        private struct Wrapper
        {
            public readonly object Action;
            public readonly uint   Id;

            public Wrapper(object action, uint id)
            {
                Action = action;
                Id     = id;
            }
        }
    }
}
