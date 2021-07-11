using Gram.Rpg.Client.Core;
using UApplication = UnityEngine.Application;


namespace Gram.Rpg.Client.Presentation
{
    public class MainGameMode : GObject, IGameMode
    {
        public MainGameMode(IWillDisposeYou disposer) : base(disposer)
        {
            UApplication.runInBackground = UApplication.isEditor;
        }
        
        public void Initialise()
        {
        }
        
        public void Start()
        {
        }
    }
}
