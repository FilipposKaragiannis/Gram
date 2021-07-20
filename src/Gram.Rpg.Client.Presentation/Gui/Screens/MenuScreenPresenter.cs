namespace Gram.Rpg.Client.Presentation.Gui.Screens
{
    public interface IMainMenuView : IView
    {
    }
    
    public class MenuScreenPresenter : Presenter<IMainMenuView>
    {
        public MenuScreenPresenter(IMainMenuView view) : base(view)
        {
        }
    }
}
