using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Application.UseCases.AppStarts;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Presentation.Gui.Screens;
using UApplication = UnityEngine.Application;


namespace Gram.Rpg.Client.Presentation
{
    public class MainGameMode : GObject, IGameMode
    {
        [Injected] public IAppStarts       AppStarts;
        [Injected] public IPlayer1Provider Player1Provider;
        
        private           IMainMenuScreen  menuScreen;

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

            menuScreen = MainMenuScreen.Create(v => new MenuScreenPresenter(v), new MenuScreenVm(Player1Provider.Get()));
            menuScreen.Open();
        }
    }
}
