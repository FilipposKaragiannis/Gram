using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Gui
{
    public interface ICanvas : IInstance
    {
        void Initialise(IGuiCamera camera);
    }

    public class Canvas : Instance.Instance, ICanvas
    {
        private UnityEngine.Canvas _uCanvas;

        public static ICanvas Create()
        {
            var go = new GameObject("Canvas");
            var gc = go.AddComponent<Canvas>();
            var uc = gc._uCanvas = go.AddComponent<UnityEngine.Canvas>();
            uc.renderMode    = RenderMode.ScreenSpaceCamera;
            uc.planeDistance = 50;
            DontDestroyOnLoad(go);

            return gc;
        }

        public void Initialise(IGuiCamera guiCamera)
        {
            _uCanvas.worldCamera = guiCamera.GetInnerCamera;
        }

    }
}
