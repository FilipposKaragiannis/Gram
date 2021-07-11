using System;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core
{
    public abstract partial class GObject
    {
        private readonly Disposer        disposer;
        private readonly IWillDisposeYou owner;
        private          Action          disposing;

        public event Action Disposing
        {
            add
            {
                if (IsDisposed)
                    throw new InvalidOperationException("You are subscribing to an already disposed Disposer.");
                disposing += value;
            }
            remove => disposing -= value;
        }

        [PublicAPI]
        public bool IsDisposed { get; private set; }

        [PublicAPI]
        public bool NotDisposed => !IsDisposed;

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (owner != null)
                owner.Disposing -= Dispose;

            disposer.Dispose();

            disposing?.Invoke();

            OnDispose();
        }

        protected void DisposeAndSetNull(ref IDisposable disposable)
        {
            if (disposable == null)
                return;

            disposable.Dispose();
            disposable = null;
        }

        protected virtual void OnDispose()
        {
        }

        IDisposerToken IDisposer.Add(Action action)
        {
            return disposer.Add(action);
        }
    }
}
