using System;
using System.Collections.Generic;

namespace Gram.Rpg.Client.Core
{
    public interface IWillDisposeYou
    {
        event Action Disposing;
    }


    public interface IDisposer : IWillDisposeYou
    {
        bool           IsDisposed { get; }
        IDisposerToken Add(Action action);
    }


    public interface IDisposerToken : IDisposable
    {
    }


    public class Disposer : IDisposer, IDisposable
    {
        private Action       disposing;
        private List<Action> list;

        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            disposing?.Invoke();


            if (list == null)
                return;


            var l = list.ToArray();

            list.Clear();

            foreach (var a in l)
            {
                if (a == null)
                    continue;

                try
                {
                    a.Invoke();
                }
                catch (Exception e)
                {
                    G.LogException("Error calling dispose callback.", e);
                }
            }
        }

        public event Action Disposing
        {
            add
            {
                if (IsDisposed)
                    throw new InvalidOperationException("You are subscribing to an already disposed Disposer.");

                disposing += value;
            }
            remove { disposing -= value; }
        }

        public bool IsDisposed { get; private set; }

        public IDisposerToken Add(Action action)
        {
            if (IsDisposed)
                throw new InvalidOperationException("You are adding an action to an already disposed Disposer.");

            if (list == null)
                list = new List<Action>();

            list.Add(action);

            return new Token(list, action);
        }


        private class Token : IDisposerToken
        {
            private readonly Action       action;
            private readonly List<Action> list;

            public Token(List<Action> list, Action action)
            {
                this.list   = list;
                this.action = action;
            }

            public void Dispose()
            {
                try
                {
                    list.Remove(action);
                }
                catch (Exception e)
                {
                    G.LogException("Unable to remove disposal action.", e);
                }
            }
        }
    }
}
