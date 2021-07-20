using Gram.Rpg.Client.Presentation.Instance;

namespace Gram.Rpg.Client.Presentation.Input
{
    public interface IInputReceiver
    {
        IInstance Instance { get; }
        void      InputStarted(HitInfo      hitInfo);
    }
}
