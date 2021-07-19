using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;
using UPhysics = UnityEngine.Physics;

namespace Gram.Rpg.Client.Presentation.Input
{
    public interface IRayCreationStrategy
    {
        Ray Create();
    }
    
    public class GuiRayCreationStrategy : IRayCreationStrategy
    {
        public Ray Create()
        {
            return new Ray(new Vector3(), Vector3.forward);
        }
    }


    public class StandardInputDetector : IInputDetector
    {
        private readonly int                  layerMask;
        private readonly int                  rayCastDistance;
        private readonly IRayCreationStrategy rayCreationStrategy;

        public StandardInputDetector(IRayCreationStrategy rayCreationStrategy,
            int                                           rayCastDistance,
            LayerMask                                     layerMask,
            bool                                          enabled = true)
        {
            this.rayCreationStrategy = rayCreationStrategy;
            this.rayCastDistance     = rayCastDistance;
            this.layerMask           = layerMask.value;

            Enabled = enabled;
        }

        public bool Enabled { get; private set; }

        public void Disable()
        {
            Enabled = false;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public bool TestForHitAt(Vector3 vector, IEnumerable<IInputReceiver> receivers, out IInputReceiver[] interestedReceivers, out HitInfo hitInfo)
        {
            hitInfo = default(HitInfo);

            var ray = rayCreationStrategy.Create();

            if (!UPhysics.Raycast(ray, out var raycastHit, rayCastDistance, layerMask))
            {
                interestedReceivers = new IInputReceiver[0];
                return false;
            }

            var hitInstance = raycastHit.collider.gameObject.GetInstance();
            if (hitInstance == null)
            {
                interestedReceivers = new IInputReceiver[0];
                return false;
            }

            interestedReceivers = receivers.Where(r => r.Instance.Equals(hitInstance)).ToArray();

            hitInfo = new HitInfo(hitInstance,
                screenPos: vector);

            return true;
        }

        public HitInfo TestIfHitting(Vector3 vector, IInstance instance)
        {
            var ray = rayCreationStrategy.Create();

            if (!UPhysics.Raycast(ray, out var hitInfo, rayCastDistance, layerMask))
                return new HitInfo(vector);

            var hitInstance = hitInfo.collider.gameObject.GetInstance();
            if (hitInstance == null)
                return new HitInfo(vector);

            return new HitInfo(instance,
                screenPos: vector);
        }
    }
}
