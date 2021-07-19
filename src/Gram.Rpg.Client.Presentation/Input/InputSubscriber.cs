using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Presentation.Instance;
using Gram.Rpg.Client.Presentation.Instance.Components;

namespace Gram.Rpg.Client.Presentation.Input
{
    public class InputSubscriber : GMonoBehaviour, IInputReceiver
    {
        private bool             initialised;
        private IInputDispatcher inputDispatcher;
        private Event<HitInfo>   inputStarted;

        protected override void Awake()
        {
            base.Awake();

            Initialise();
        }

        public void OnDestroy()
        {
            inputDispatcher.RemoveReceiver(this);
        }


        IInstance IInputReceiver.Instance => instance;

        void IInputReceiver.InputStarted(HitInfo hitInfo)
        {
            inputStarted.Invoke(hitInfo);
        }


        public void Initialise()
        {
            if (initialised) return;


            inputDispatcher = SL.Get<IInputDispatcher>();
            inputDispatcher.AddReceiver(this);

            inputStarted = new Event<HitInfo>("InputStarted");

            initialised = true;
        }
    }
}
