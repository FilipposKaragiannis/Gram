using System;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core
{
   public interface IEvent<T> : IDisposable
    {
        int Count { get; }
        SubcsriptionToken Add(IDisposer disposer, Action<T> action);
        void Remove(Action<T>           action);
    }


    public class Event<T> : IEvent<T>
    {
        private readonly string    name;
        private          Action<T> handlers;
        private          Action    onAddFirst;
        private          Action    onRemoveLast;

        public Event(string name, IWillDisposeYou owner = null, Action onAddFirst = null, Action onRemoveLast = null)
        {
            this.name         = name;
            this.onAddFirst   = onAddFirst;
            this.onRemoveLast = onRemoveLast;
            
            if (owner != null)
                owner.Disposing += Dispose;
        }

        public int Count => handlers?.GetInvocationList().Length ?? 0;

        public SubcsriptionToken Add(IDisposer disposer, Action<T> action)
        {
            if (handlers != null && handlers.AlreadyHasSubscriber(action, "Event [{0}] already has this subscriber.", name))
                return SubcsriptionToken.Null;

            disposer.Add(() => Remove(action));

            var length = handlers?.GetInvocationList().Length ?? 0;

            handlers += action;

            if (length == 0)
                onAddFirst?.Invoke();

            return new SubcsriptionToken(() => Remove(action));
        }

        public void Clear()
        {
            handlers = null;

            onRemoveLast?.Invoke();
        }

        public void Dispose()
        {
            onAddFirst   = null;
            onRemoveLast?.Invoke();
            onRemoveLast = null;
            handlers     = null;
        }

        public void Invoke(T arg)
        {
            handlers?.Invoke(arg);
        }

        public void Remove(Action<T> action)
        {
            var length = handlers?.GetInvocationList().Length ?? 0;

            handlers -= action;

            if (length > 0 && (handlers == null || handlers.GetInvocationList().Length == 0))
            {
                handlers = null;

                onRemoveLast?.Invoke();
            }
        }
    }
}
