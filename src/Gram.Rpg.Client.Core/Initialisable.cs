namespace Gram.Rpg.Client.Core
{
    public interface IInitialisable
    {
        void Initialise(IDisposer disposer);
    }

    public abstract class InitialisableBase : IInitialisable
    {
        protected IDisposer Disposer;

        public void Initialise(IDisposer disposer)
        {
            Disposer = disposer;

            Initialise();
        }

        protected virtual void Initialise()
        {
        }
    }
}
