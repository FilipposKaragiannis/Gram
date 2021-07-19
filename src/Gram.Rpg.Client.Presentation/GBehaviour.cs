using System;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Core.Extensions;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation
{
    public interface IBehaviour : IDisposable, IDisposer
    {
        bool Paused      { get; }
        bool StillExists { get; }
        void Pause();
        void Resume();
    }

    public abstract class GBehaviour : MonoBehaviour, IBehaviour
    {
        protected readonly Disposer disposer;

        protected GBehaviour()
        {
            disposer = new Disposer();
        }

        protected bool Disposed { get; private set; }

        [UsedImplicitly]
        public void OnDestroy()
        {
            Dispose();
        }

        public event Action Disposing
        {
            add => disposer.Disposing += value;
            remove => disposer.Disposing -= value;
        }
        
        public bool Paused { get; private set; }

        public bool StillExists => !Disposed;

        public virtual void Dispose()
        {
            if (Disposed)
                return;

            Disposed = true;

            DoDispose();
        }

        public void Pause()
        {
            Paused = true;
            OnPause();
        }

        public void Resume()
        {
            Paused = false;
            OnResume();
        }

        bool IDisposer.IsDisposed => disposer.IsDisposed;

        IDisposerToken IDisposer.Add(Action action)
        {
            return disposer.Add(action);
        }

        protected void DisposeOf(params IDisposable[] disposables)
        {
            disposables.ForEach(d => d?.Dispose());
        }

        protected abstract void DoDispose();

        protected virtual void OnDispose()
        {
        }

        protected virtual void OnPause()
        {
        }

        protected virtual void OnResume()
        {
        }
    }
}
