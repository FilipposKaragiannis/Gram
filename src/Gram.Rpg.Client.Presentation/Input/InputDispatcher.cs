using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Messaging;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Input
{
    public interface IInputDispatcher
    {
        void AddDetector(IDisposer          disposer, string name, IInputDetector detector);
        void AddReceiver(IInputReceiver     receiver);
        void RemoveReceiver(IInputReceiver  receiver);
    }

    public class InputDispatcher : IInputDispatcher
    {
        private readonly IInput                            input;
        private readonly IMessageBus                       messageBus;
        private readonly List<IInputDetector>              detectors;
        private readonly Dictionary<string,IInputDetector> detectorLookup;
        private readonly List<IInputReceiver>              receivers;

        public InputDispatcher(IInput input, IMessageBus messageBus)
        {
            this.input      = input;
            this.messageBus = messageBus;

            detectors      = new List<IInputDetector>();
            detectorLookup = new Dictionary<string, IInputDetector>();
            receivers      = new List<IInputReceiver>();

            messageBus.SubscribeTo<UpdateMessage>(OnUpdate);
        }
        
        private void OnUpdate(UpdateMessage e)
        {
            if (input.Detected)
                OnInputDetected(input.Vector);
        }
        
        public void AddDetector(IDisposer disposer, string name, IInputDetector detector)
        {
            detectorLookup.Add(name, detector);
            detectors.Add(detector);

            disposer.Add(() => RemoveDetector(name));
        }
        
        public void AddReceiver(IInputReceiver receiver)
        {
            if (receiver == null || !receiver.Instance.StillExists)
                return;

            receiver.Instance.Add(() => RemoveReceiver(receiver));
            receivers.Add(receiver);
        }
        
        public void RemoveReceiver(IInputReceiver receiver)
        {
            receivers.Remove(receiver);
        }
        
        public void RemoveDetector(string name)
        {
            var detector = GetDetector(name);

            if (detector != null)
                detectors.Remove(detector);

            detectorLookup.Remove(name);
        }
        
        private IInputDetector GetDetector(string name)
        {
            if (detectorLookup.ContainsKey(name))
                return detectorLookup[name];

            return null;
        }
        
        private void OnInputDetected(Vector3 inputVector)
        {
            if (receivers.Count == 0)
                return;

            foreach (var detector in detectors.Where(d => d.Enabled))
            {
                var didHit = detector.TestForHitAt(inputVector, receivers, out var interestedReceivers, out var hitInfo);

                if (didHit && interestedReceivers.Length > 0)
                {
                    foreach (var receiver in interestedReceivers)
                        receiver.InputStarted(hitInfo);
                }

                if (didHit)
                    return;
            }
        }
    }
}
