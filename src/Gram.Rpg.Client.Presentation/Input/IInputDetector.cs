using System.Collections.Generic;
using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Input
{
    public interface IInputDetector
    {
        bool Enabled { get; }

        void    Disable();
        void    Enable();
        bool    TestForHitAt(Vector3  vector, IEnumerable<IInputReceiver> receivers, out IInputReceiver[] interestedReceivers, out HitInfo hitInfo);
        HitInfo TestIfHitting(Vector3 vector, IInstance                   instance);
    }
}
