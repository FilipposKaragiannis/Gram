using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gram.Rpg.Client.Core.Collections
{
    public interface IShuffleBag<out T>
    {
        bool IsEmpty { get; }
        T    Next();
    }


    public class ShuffleBag<T> : IEnumerable<T>, IShuffleBag<T>
    {
        private readonly IList<T> bag;
        private          int      currentIndex;

        public ShuffleBag(IEnumerable<T> items)
        {
            bag = items.ToList();

            Count = bag.Count;

            currentIndex = Count - 1;
        }

        public ShuffleBag(params T[] items)
        {
            bag = items.ToList();

            Count = bag.Count;

            currentIndex = Count - 1;
        }

        public int Count { get; }

        public bool IsEmpty => Count == 0;

        public IEnumerator<T> GetEnumerator()
        {
            return bag.GetEnumerator();
        }

        public T Next()
        {
            if (currentIndex < 1)
            {
                currentIndex = Count - 1;

                return currentIndex < 0
                    ? default(T)
                    : bag[0];
            }

            var minIndex = currentIndex < Count - 1 ? 0 : 1;

            var index = GRandom.Range(minIndex, currentIndex + 1);

            var curItem = bag[index];
            bag[index]        = bag[currentIndex];
            bag[currentIndex] = curItem;

            currentIndex--;

            return curItem;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public static class ShuffleBagExtensions
    {
        public static ShuffleBag<T> ToShuffleBag<T>(this IEnumerable<T> enumerable)
        {
            return new ShuffleBag<T>(enumerable);
        }
    }
}
