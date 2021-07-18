using Gram.Rpg.Client.Presentation.Instance;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation.Gui
{
    public interface IGuiCamera : IInstance
    {
        Camera    GetInnerCamera { get; }
        LayerMask CullingMask    { get; }
        int       Zdepth         { get; }
        Ray       ScreenPointToRay(Vector3 vector);
    }

    public class GuiCamera : Instance.Instance, IGuiCamera
    {
        private Camera _uCamera;


        public static IGuiCamera Create()
        {
            var go = new GameObject("GuiCamera");

            var gc = go.AddComponent<GuiCamera>();
            var uc = gc._uCamera = go.AddComponent<Camera>();
            DontDestroyOnLoad(go);
            
            uc.allowMSAA        = false;
            uc.orthographic     = true;
            uc.orthographicSize = 100;
            uc.clearFlags       = CameraClearFlags.SolidColor;
            uc.backgroundColor  = new Color(169, 169, 169);
            uc.farClipPlane     = 100;
            uc.nearClipPlane    = 0;
            uc.renderingPath    = RenderingPath.UsePlayerSettings;
            uc.rect             = new Rect(0, 0, 1, 1);

            return gc;
        }

        public Camera    GetInnerCamera => _uCamera;
        public LayerMask CullingMask    => _uCamera.cullingMask;
        public int       Zdepth         => (int)_uCamera.depth;

        public Ray ScreenPointToRay(Vector3 vector)
        {
            return _uCamera.ScreenPointToRay(vector);
        }
    }
}
