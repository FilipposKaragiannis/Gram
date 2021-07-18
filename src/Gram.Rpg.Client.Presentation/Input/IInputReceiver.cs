using Gram.Rpg.Client.Presentation.Instance;

namespace Gram.Rpg.Client.Presentation.Input
{
    public interface IInputReceiver
    {
        IInstance Instance { get; }
        void      InputEndedInside(HitInfo  hitInfo);
        void      InputEndedOutside(HitInfo hitInfo);
        void      InputHeld(HitInfo         hitInfo);
        void      InputHeldInside(HitInfo   hitInfo);
        void      InputHeldOutside(HitInfo  hitInfo);
        void      InputInside(HitInfo       hitInfo);
        void      InputMovedInside(HitInfo  hitInfo);
        void      InputMovedOutside(HitInfo hitInfo);
        void      InputOutside(HitInfo      hitInfo);
        void      InputStarted(HitInfo      hitInfo);
    }
}
