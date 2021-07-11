using System;

namespace Gram.Rpg.Client.Core.IOC
{
    public class ContainerScope : IDisposable
    {
        private readonly Action action;

        public ContainerScope(Action action)
        {
            this.action = action;
        }

        public void Dispose()
        {
            action();
        }
    }
}