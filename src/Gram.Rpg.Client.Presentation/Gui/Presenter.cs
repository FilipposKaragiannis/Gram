using Gram.Rpg.Client.Core;

namespace Gram.Rpg.Client.Presentation.Gui
{
    public interface IPresenter
    {
    }


    public class Presenter : GObject, IPresenter
    {
        protected Presenter(IWillDisposeYou view) : base(view)
        {
        }
    }


    public abstract class Presenter<TView> : Presenter where TView : IView
    {
        protected readonly TView View;

        protected Presenter(TView view) : base(view)
        {
            View = view;
        }
    }


    public interface IView : IDisposer
    {
    }
    
    public class EmptyPresenter : IPresenter
    {
    }
}
