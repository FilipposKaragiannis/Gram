namespace Gram.Rpg.Client.Presentation.Gui
{
    public abstract class View<TPresenter> : Instance.Instance, IView where TPresenter : IPresenter
    {
        protected static void Configure(View<TPresenter> view,
            string                                       name     = null)
        {
            view.name = name ?? "View";
        }

        public TPresenter Presenter { get; set; }
    }
}
