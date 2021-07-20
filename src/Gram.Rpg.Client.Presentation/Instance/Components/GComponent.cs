using System;
using Gram.Rpg.Client.Core;
using UnityEngine;
using Event = Gram.Rpg.Client.Core.Event;
using Object = UnityEngine.Object;

namespace Gram.Rpg.Client.Presentation.Instance.Components
{
    public interface IGComponent : IDisposable
    {
        IEvent Disposed   { get; }
        bool   Enabled    { get; }
        bool   IsDisposed { get; }
    }

    public abstract class GComponent : MonoBehaviour, IGComponent
    {
        private readonly Event disposed;

        protected GComponent(GameObject gameObject)
        {
            instance = gameObject.GetComponent<Instance>();
            disposed = new Event(name: "YComponent.Disposed");
        }

        public IEvent Disposed => disposed;

        public abstract bool     Enabled    { get; }
        public          Instance instance   { get; }
        public          bool     IsDisposed { get; private set; }

        public void Dispose()
        {
            OnDispose();
            IsDisposed = true;
            disposed.Invoke();
        }

        protected void Destroy(Object obj)
        {
            Object.Destroy(obj: obj);
        }

        protected virtual void OnDispose()
        {
        }
    }
    
    public abstract class GMonoBehaviour : MonoBehaviour, IGComponent
    {
        private readonly Event disposed;

        protected GMonoBehaviour()
        {
            disposed = new Event(name: "YComponent.Disposed");
        }

        public IEvent Disposed => disposed;
        public bool   Enabled  => enabled;
        
        public Instance instance   { get; private set; }
        public bool     IsDisposed { get; private set; }

        public void Dispose()
        {
            OnDispose();
            IsDisposed = true;
            disposed.Invoke();
        }

        protected virtual void Awake()
        {
            instance = GetComponent<Instance>();
        }

        protected virtual void OnDispose()
        {
            Destroy(this);
        }
    }
}
