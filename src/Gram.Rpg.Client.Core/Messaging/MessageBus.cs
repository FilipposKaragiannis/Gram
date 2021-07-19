using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Gram.Rpg.Client.Core.Messaging
{
    public interface IMessageBus
    {
        void Broadcast<T>(T                      msg);
        void SubscribeTo<T>(Action<T>            subscriber, IDisposer disposer = null);
    }


    public class MessageBus : IMessageBus
    {
        private readonly IDedicatedEventHandlers     dedicated;
        private          Dictionary<Type, ArrayList> oneTimeSubscribers;
        private          Dictionary<Type, ArrayList> subscribers;

        public MessageBus(IDedicatedEventHandlers dedicated = null)
        {
            this.dedicated = dedicated ?? new NullDedicatedEventHandlers();
        }

        public void Broadcast<T>(T msg)
        {
            if (msg == null)
                return;


            BroadcastToOneTimeSubscribers(msg);


            if (dedicated.TryBroadcastEvent(msg))
                return;


            if (subscribers == null)
                return;

            var type = msg.GetType();

            if (!subscribers.ContainsKey(type))
                return;

            var actions = subscribers[type];

            for (var i = actions.Count - 1; i >= 0; i--)
                InvokeAction(msg, actions[i]);
        }

        public void SubscribeTo<T>(Action<T> subscriber, IDisposer disposer = null)
        {
            if (dedicated.TrySubscribeEventFor(subscriber, disposer))
                return;


            if (subscribers == null)
                subscribers = new Dictionary<Type, ArrayList>();

            ArrayList list;
            var       eventType = typeof(T);

            if (subscribers.ContainsKey(eventType))
            {
                list = subscribers[eventType];
            }
            else
            {
                list = new ArrayList();
                subscribers.Add(eventType, list);
            }

            if (AlreadySubscribed(subscriber, list, eventType))
                return;

            list.Add(subscriber);

            disposer?.Add(() => UnsubscribeFrom(subscriber));
        }

        private void UnsubscribeFrom<T>(Action<T> subscriber)
        {
            if (dedicated.TryUnsubscribeEventFor(subscriber))
                return;


            if (subscribers == null)
                return;

            var eventType = typeof(T);

            if (!subscribers.ContainsKey(eventType))
                return;

            var list = subscribers[eventType];

            list.Remove(subscriber);

            if (list.Count == 0)
                subscribers.Remove(eventType);
        }

        private static void InvokeAction<T>(T msg, object action)
        {
            if (action is Action<T> a)
            {
                a.Invoke(msg);
                return;
            }

            action.GetType()
                .GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public)
                ?.Invoke(action, new object[] {msg});
        }

        private static bool AlreadySubscribed<T>(Action<T> subscriber, IList actions, MemberInfo eventType)
        {
            if (!actions.Contains(subscriber))
                return false;

            G.LogWarning($"Trying to subscribe a handler twice to: {eventType.Name}. Subscription failed.");
            return true;
        }

        private void BroadcastToOneTimeSubscribers<T>(T msg)
        {
            if (oneTimeSubscribers == null)
                return;

            var type = msg.GetType();

            if (!oneTimeSubscribers.ContainsKey(type))
                return;

            try
            {
                var a = oneTimeSubscribers[type];

                for (var i = a.Count - 1; i >= 0; i--)
                    InvokeAction(msg, a[i]);
            }
            finally
            {
                oneTimeSubscribers.Remove(type);

                if (oneTimeSubscribers.Count == 0)
                    oneTimeSubscribers = null;
            }
        }


        public interface IDedicatedEventHandlers
        {
            bool TryBroadcastEvent<T>(T              e);
            bool TrySubscribeEventFor<T>(Action<T>   subscriber, IDisposer disposer);
            bool TryUnsubscribeEventFor<T>(Action<T> subscriber);
        }


        private class NullDedicatedEventHandlers : IDedicatedEventHandlers
        {
            public bool TryBroadcastEvent<T>(T e)
            {
                return false;
            }

            public bool TrySubscribeEventFor<T>(Action<T> subscriber, IDisposer disposer)
            {
                return false;
            }

            public bool TryUnsubscribeEventFor<T>(Action<T> subscriber)
            {
                return false;
            }
        }
    }
}
