using UnityEngine;
using UEInput = UnityEngine.Input;

namespace Gram.Rpg.Client.Presentation.Input
{
    public class MouseInput : IInput
    {
        public bool Detected => UEInput.GetButtonDown("Fire1");

        public bool Released => UEInput.GetButtonUp("Fire1");

        public bool Sustained => UEInput.GetButton("Fire1");

        public Vector3 Vector => UEInput.mousePosition;
    }

    public class TouchInput : IInput
    {
        public bool Detected => UEInput.touchCount >= 1 && UEInput.GetTouch(0).phase == TouchPhase.Began;

        public bool Released => UEInput.touchCount >= 1 && UEInput.GetTouch(0).phase == TouchPhase.Ended;

        public bool Sustained
        {
            get
            {
                if (UEInput.touchCount < 1)
                    return false;

                var touch = UEInput.GetTouch(0);

                return touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            }
        }

        public Vector3 Vector
        {
            get
            {
                if (UEInput.touchCount == 0)
                    return Vector3.zero;

                return UEInput.touches[0].position;
            }
        }
    }
}
