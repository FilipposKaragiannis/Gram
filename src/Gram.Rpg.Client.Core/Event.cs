using System;
using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core
{
    [PublicAPI]
    public interface IEvent : IDisposable
    {
        int  Count { get; }
        void Add(IDisposer     disposer, Action action);
        void Once(IDisposer    disposer, Action action);
        void Remove(Action     action);
        void RemoveOnce(Action action);
    }


    public class Event : IEvent
    {
        private readonly string name;
        private          Action handlers;
        private          Action onAddFirst;
        private          Action onceHandlers;
        private          Action onRemoveLast;

        [Obsolete("Please pass a disposer to the Event.")]
        public Event(string name, Action onAddFirst = null, Action onRemoveLast = null)
        {
            this.name         = name;
            this.onAddFirst   = onAddFirst;
            this.onRemoveLast = onRemoveLast;
        }

        public Event(IWillDisposeYou owner, string name, Action onAddFirst = null, Action onRemoveLast = null)
        {
            this.name         = name;
            this.onAddFirst   = onAddFirst;
            this.onRemoveLast = onRemoveLast;

            if (owner != null)
                owner.Disposing += Dispose;
        }

        public int Count
        {
            get
            {
                var hc = handlers?.GetInvocationList().Length     ?? 0;
                var oc = onceHandlers?.GetInvocationList().Length ?? 0;

                return hc + oc;
            }
        }

        public void Add(IDisposer disposer, Action action)
        {
            if (AlreadyHasSubscriber(action))
                return;

            disposer.Add(() => Remove(action));

            var before = Count;

            handlers += action;

            if (before == 0 && Count > 0)
                onAddFirst?.Invoke();
        }

        public void Dispose()
        {
            onAddFirst   = null;
            onRemoveLast = null;
            handlers     = null;
        }

        public void Once(IDisposer disposer, Action action)
        {
            if (AlreadyHasSubscriber(action))
                return;

            disposer.Add(() => RemoveOnce(action));

            var before = Count;

            onceHandlers += action;

            if (before == 0 && Count > 0)
                onAddFirst?.Invoke();
        }

        public void Remove(Action action)
        {
            var before = Count;

            handlers -= action;

            if (before > 0 && Count == 0)
                Clear();
        }

        public void RemoveOnce(Action action)
        {
            var before = Count;

            onceHandlers -= action;

            if (before > 0 && Count == 0)
                Clear();
        }

        [PublicAPI]
        public void Clear()
        {
            handlers     = null;
            onceHandlers = null;

            onRemoveLast?.Invoke();
        }

        public void Invoke()
        {
            if (onceHandlers != null)
            {
                var before = Count;

                onceHandlers();
                onceHandlers = null;

                if (Count == 0 && before > 0)
                    Clear();
            }


            handlers?.Invoke();
        }

        private bool AlreadyHasSubscriber(Action action)
        {
            if (handlers != null && handlers.AlreadyHasSubscriber(action, "Event [{0}] already has this subscriber.", name))
                return true;

            if (onceHandlers != null && onceHandlers.AlreadyHasSubscriber(action, "Event [{0}] already has this (once) subscriber.", name))
                return true;

            return false;
        }
    }
}
