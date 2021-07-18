using System.Linq;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;
using Gram.Rpg.Client.Presentation.Extensions;

namespace Gram.Rpg.Client.Presentation.Instance
{
    public interface IInstance : IBehaviour
    {
        bool   Enabled { get; set; }
        string Name    { get; set; }

        void Disable();
        void Enable();
    }

    public class Instance : GBehaviour, ITimeSource, IInstance
    {
        private static readonly Event<float> update = new Event<float>("Instance.Update");

        protected IEvent<float> Update => update;

        
        protected override void DoDispose()
        {
            StopAllCoroutines();

            disposer.Dispose();

            OnDispose();

            Destroy(base.gameObject);
        }
        
        private bool isEnabled;

        protected Instance()
        {
            isEnabled  = true;
        }
        
        public bool Enabled
        {
            get => isEnabled;
            set
            {
                if (value)
                    Enable();
                else
                    Disable();
            }
        }
        
        public void Disable()
        {
            isEnabled = false;

            base.gameObject.SetActive(isEnabled);

            OnDisabled();

            GetChildren<Instance>(true)
                .Where(c => c.StillExists)
                .ForEach(c => c.OnDisabled());
        }
        
        public void Enable()
        {
            isEnabled = true;

            base.gameObject.SetActive(isEnabled);

            OnEnabled();

            GetChildren<Instance>(true)
                .Where(c => c.StillExists)
                .ForEach(c => c.OnEnabled());
        }
        
        public T[] GetChildren<T>(bool includeDescendants = false) where T : Instance
        {
            return includeDescendants
                ? base.transform.GetDescendants<T>()
                : base.transform.GetChildren<T>();
        }

        public string Name
        {
            get => name;
            set => name = value;
        }
        
        protected virtual void OnDisabled()
        {
        }

        protected virtual void OnEnabled()
        {
        }

        public static void GUpdate(float deltaTime)
        {
            update.Invoke(deltaTime);
        }
    }
}
