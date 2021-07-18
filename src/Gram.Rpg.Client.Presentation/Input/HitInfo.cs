using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Input
{
    public struct HitInfo
    {
        public readonly IInstance Instance;
        public readonly Vector3  ScreenPos;
       

        public HitInfo(Vector3 screenPos)
        {
            ScreenPos = screenPos;
            Instance = null;
        }

        public HitInfo(IInstance instance, Vector3 screenPos)
        {
            Instance = instance;
            ScreenPos = screenPos;
        }
    }
}
