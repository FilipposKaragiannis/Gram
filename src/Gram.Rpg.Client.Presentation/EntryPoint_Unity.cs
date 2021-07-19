using Gram.Rpg.Client.Application.Messages;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Messaging;
using JetBrains.Annotations;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation
{
    public abstract partial class EntryPoint : MonoBehaviour
    {
        private float last10thDelta;
        private float last60thDelta;
        private uint  updateCount;

        [UsedImplicitly]
        public void Awake()
        {
            disposer = new Disposer();

            Initialise();

            OnInitialised();
        }

        [UsedImplicitly]
        public void Update()
        {
            var dt = Time.deltaTime;

            MessageBus.Broadcast(UpdateMessage.Instance(dt));

            last10thDelta += dt;
            last60thDelta += dt;

            if (updateCount % 10 == 0)
            {
                MessageBus.Broadcast(Every10thUpdateMessage.Instance(last10thDelta));
                last10thDelta = 0;
            }

            if (updateCount % 60 == 0)
            {
                MessageBus.Broadcast(Every60thUpdateMessage.Instance(last60thDelta));
                last60thDelta = 0;
            }

            updateCount++;
        }

        [UsedImplicitly]
        public void FixedUpdate()
        {
            MessageBus.Broadcast(FixedUpdateMessage.Instance);
        }

        [UsedImplicitly]
        public void LateUpdate()
        {
            MessageBus.Broadcast(LateUpdateMessage.Instance);
        }

        [UsedImplicitly]
        public void OnDestroy()
        {
            disposer.Dispose();

            gameMode?.Dispose();

            OnDisposed();
        }

        [UsedImplicitly]
        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                G.Log("Application Paused");
                MessageBus.Broadcast(new ApplicationPaused());
            }
            else
            {
                G.Log("Application Resumed");
                MessageBus.Broadcast(new ApplicationResumed());
            }
        }
    }
}
