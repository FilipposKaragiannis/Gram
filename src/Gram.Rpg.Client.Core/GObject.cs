using System;

namespace Gram.Rpg.Client.Core
{
    public abstract partial class GObject : IDisposable, IDisposer
    {
        protected static void SetNullThen<T>(ref T v, Action<T> action)
        {
            if (v == null)
                return;

            var temp = v;
            v = default;
            action(temp);
        }

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
