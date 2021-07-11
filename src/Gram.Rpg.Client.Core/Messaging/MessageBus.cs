using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core.Messaging
{
    public interface IMessageBus
    {
        void Broadcast<T>(T                      msg);
        void SubscribeOnceTo<T>(Action<T>        subscriber, IDisposer disposer = null);
        void SubscribeTo<T>(Action<T>            subscriber, IDisposer disposer = null);
        void UnsubscribeFrom<T>(Action<T>        subscriber);
        void UnsubscribeFromOneTime<T>(Action<T> subscriber);
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

        public void SubscribeOnceTo<T>(Action<T> subscriber, IDisposer disposer = null)
        {
            if (oneTimeSubscribers == null)
                oneTimeSubscribers = new Dictionary<Type, ArrayList>();

            ArrayList list;
            var       eventType = typeof(T);

            if (oneTimeSubscribers.ContainsKey(eventType))
            {
                list = oneTimeSubscribers[eventType];
            }
            else
            {
                list = new ArrayList();
                oneTimeSubscribers.Add(eventType, list);
            }

            if (AlreadySubscribed(subscriber, list, eventType))
                return;

            list.Add(subscriber);

            if (disposer != null)
                disposer.Add(() => UnsubscribeFromOneTime(subscriber));
        }

        [PublicAPI]
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

        [PublicAPI]
        public void UnsubscribeFrom<T>(Action<T> subscriber)
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

        [PublicAPI]
        public void UnsubscribeFromOneTime<T>(Action<T> subscriber)
        {
            if (oneTimeSubscribers == null)
                return;

            var eventType = typeof(T);

            if (!oneTimeSubscribers.ContainsKey(eventType))
                return;

            var list = oneTimeSubscribers[eventType];

            list.Remove(subscriber);

            if (list.Count == 0)
                oneTimeSubscribers.Remove(eventType);
        }

        private static void InvokeAction<T>(T msg, object action)
        {
            // If we broadcast a message whose instance is declared as the type of the Message 
            // (unsure on terminology: where typeof(T) == msg.GetType()), then all is good. 

            // If the message was cast to a different type (eg. IMyIntermediateMessage), which 
            // is still valid, then typeof(T) != msg.GetType(), so we can't simply cast 
            // actions[i] to Action<T> as we once did.

            // Instead we must detect this situation and use reflection to invoke the delegate.
            // The reflection approach is slower so we only use it when required.

            // This problem first occurred when we stored a message as an interface and then
            // later broadcast that interface-casted instance. <T> was the interface but the 
            // subscriber wanted to receive the concrete instance. The reflection approach 
            // solved the problem.

            if (action is Action<T> a)
            {
                a.Invoke(msg);
                return;
            }

            // action is not of type Action<T>
            // action is Action<C> where C is the concrete, declared type

            // eg

            // SubscribeTo<C>(m => {});

            // public interface I : IMessage
            // public class C : I

            // I m = new C();
            // Broadcast(m);

            // We subscribed to C so action is Action<C> but 
            // typeof(T) == typeof(I)
            // so we can't just cast, hence the reflection.

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
