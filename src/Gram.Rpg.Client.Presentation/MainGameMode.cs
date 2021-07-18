using Gram.Rpg.Client.Application.UseCases.AppStarts;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using UApplication = UnityEngine.Application;


namespace Gram.Rpg.Client.Presentation
{
    public class MainGameMode : GObject, IGameMode
    {
        [Injected] public IAppStarts AppStarts;

        public MainGameMode(IWillDisposeYou disposer) : base(disposer)
        {
            UApplication.runInBackground = UApplication.isEditor;
        }
        
        public void Initialise()
        {
            
        }
        
        public void Start()
        {
            AppStarts.Execute(null);
        }
    }
}
