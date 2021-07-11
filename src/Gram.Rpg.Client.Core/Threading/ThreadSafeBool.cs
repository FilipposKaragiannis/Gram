using System.Threading;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core.Threading
{
    public struct ThreadSafeBool
    {
        public static implicit operator bool(ThreadSafeBool b)
        {
            return b.Get();
            
            // Note: we have no implicit setter because this would break
            // integrity by creating a new instance of ThreadSafeBool. 
        }

        private int backing;

        public ThreadSafeBool(bool initial = false)
        {
            backing = initial ? 1 : 0;
        }

        [PublicAPI]
        public bool Get() => Interlocked.CompareExchange(ref backing, 1, 1) == 1;

        public void Set(bool value)
        {
            if (value)
                Interlocked.CompareExchange(ref backing, 1, 0);
            else
                Interlocked.CompareExchange(ref backing, 0, 1);
        }
    }
}
