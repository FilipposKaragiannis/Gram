using System.Threading;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core.Threading
{
    [PublicAPI]
    public struct ThreadSafeInt
    {
        public static implicit operator int(ThreadSafeInt i)
        {
            return i.Get();

            // Note: we have no implicit setter because this would break
            // integrity by creating a new instance of ThreadSafeInt. 
        }

        private long backing;

        public ThreadSafeInt(int initial = 0)
        {
            backing = initial;
        }

        public int Decrement()
        {
            return (int)Interlocked.Decrement(ref backing);
        }

        public int DecrementBy(int value)
        {
            return (int)Interlocked.Add(ref backing, -value);
        }

        public int Get()
        {
            return (int)Interlocked.Read(ref backing);
        }

        public int Increment()
        {
            return (int)Interlocked.Increment(ref backing);
        }

        public int IncrementBy(int value)
        {
            return (int)Interlocked.Add(ref backing, value);
        }

        public int Set(int value)
        {
            return (int)Interlocked.Exchange(ref backing, value);
        }
    }
}
