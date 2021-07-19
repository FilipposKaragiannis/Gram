using System;

namespace Gram.Rpg.Client.Core
{
    public abstract partial class GObject : IDisposable, IDisposer
    {
        protected GObject(IWillDisposeYou owner) : this()
        {
            if (owner == null)
                return;

            this.owner           =  owner;
            this.owner.Disposing += Dispose;
        }

        protected GObject()
        {
            disposer = new Disposer();
        }
    }
}
