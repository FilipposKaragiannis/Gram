using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Presentation.Instance;
using Gram.Rpg.Client.Presentation.Instance.Components;

namespace Gram.Rpg.Client.Presentation.Input
{
    public class InputSubscriber : GMonoBehaviour, IInputReceiver
    {
        private bool             initialised;
        private IInputDispatcher inputDispatcher;
        private Event<HitInfo>   inputEnded;
        private Event<HitInfo>   inputEndedInside;
        private Event<HitInfo>   inputEndedOutside;
        private Event<HitInfo>   inputHeld;
        private Event<HitInfo>   inputHeldInside;
        private Event<HitInfo>   inputHeldOutside;
        private Event<HitInfo>   inputInside;
        private Event<HitInfo>   inputMoved;
        private Event<HitInfo>   inputMovedInside;
        private Event<HitInfo>   inputMovedOutside;
        private Event<HitInfo>   inputOutside;
        private Event<HitInfo>   inputStarted;


        public IEvent<HitInfo> InputEnded        => inputEnded;
        public IEvent<HitInfo> InputEndedInside  => inputEndedInside;
        public IEvent<HitInfo> InputEndedOutside => inputEndedOutside;
        public IEvent<HitInfo> InputHeld         => inputHeld;
        public IEvent<HitInfo> InputHeldInside   => inputHeldInside;
        public IEvent<HitInfo> InputHeldOutside  => inputHeldOutside;
        public IEvent<HitInfo> InputInside       => inputInside;
        public IEvent<HitInfo> InputMoved        => inputMoved;
        public IEvent<HitInfo> InputMovedInside  => inputMovedInside;
        public IEvent<HitInfo> InputMovedOutside => inputMovedOutside;
        public IEvent<HitInfo> InputOutside      => inputOutside;
        public IEvent<HitInfo> InputStarted      => inputStarted;


        public bool Paused { get; private set; }

        public InputSubscriber Initialise()
        {
            // Historically, and in normal operation, the input subscriber creates its event handlers
            // when it "Awakes". When we came to use it with ListItems this caused problems because the
            // ListItem is generally in a disabled state when we come to configure it and consequently,
            // the initialisation code below hasn't been run. This leads to exceptions or messy
            // workarounds. We're making the ability to Initialise the InputSubscriber public to avoid this.

            if (initialised)
                return this;


            inputDispatcher = SL.Get<IInputDispatcher>();
            inputDispatcher.AddReceiver(this);

            inputEnded        = new Event<HitInfo>("InputEnded");
            inputEndedInside  = new Event<HitInfo>("InputEndedInside");
            inputEndedOutside = new Event<HitInfo>("InputEndedOutside");
            inputInside       = new Event<HitInfo>("InputInside");
            inputMoved        = new Event<HitInfo>("InputMoved");
            inputMovedInside  = new Event<HitInfo>("InputMovedInside");
            inputMovedOutside = new Event<HitInfo>("InputMovedOutside");
            inputHeld         = new Event<HitInfo>("InputHeld");
            inputHeldInside   = new Event<HitInfo>("InputHeldInside");
            inputHeldOutside  = new Event<HitInfo>("InputHelOutside");
            inputOutside      = new Event<HitInfo>("InputOutside");
            inputStarted      = new Event<HitInfo>("InputStarted");

            initialised = true;

            return this;
        }

        [UsedImplicitly]
        public void OnDestroy()
        {
            inputDispatcher.RemoveReceiver(this);
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            Paused = false;
        }


        protected override void Awake()
        {
            base.Awake();

            Initialise();
        }


        IInstance IInputReceiver.Instance => instance;

        void IInputReceiver.InputEndedInside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputEnded.Invoke(hitInfo);
            inputEndedInside.Invoke(hitInfo);
        }

        void IInputReceiver.InputEndedOutside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputEnded.Invoke(hitInfo);
            inputEndedOutside.Invoke(hitInfo);
        }

        void IInputReceiver.InputHeld(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputHeld.Invoke(hitInfo);
        }

        void IInputReceiver.InputHeldInside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputHeldInside.Invoke(hitInfo);
        }

        void IInputReceiver.InputHeldOutside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputHeldOutside.Invoke(hitInfo);
        }

        void IInputReceiver.InputInside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputInside.Invoke(hitInfo);
        }

        void IInputReceiver.InputMovedInside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputMoved.Invoke(hitInfo);
            inputMovedInside.Invoke(hitInfo);
        }

        void IInputReceiver.InputMovedOutside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputMoved.Invoke(hitInfo);
            inputMovedOutside.Invoke(hitInfo);
        }

        void IInputReceiver.InputOutside(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputOutside.Invoke(hitInfo);
        }

        void IInputReceiver.InputStarted(HitInfo hitInfo)
        {
            if (Paused)
                return;

            inputStarted.Invoke(hitInfo);
        }
    }
}
