using System;

namespace Gram.Rpg.Client.Core
{
    public class SubcsriptionToken : IDisposable
    {
        private readonly Action action;

        public SubcsriptionToken(Action action)
        {
            this.action = action;
        }

        public static SubcsriptionToken Null => new SubcsriptionToken(() => { });

        public void Dispose()
        {
            action();
        }

        public void Remove()
        {
            Dispose();
        }
    }
}
