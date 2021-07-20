using System;
using System.Collections.Generic;
using System.Linq;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var items = enumerable as T[] ?? enumerable.ToArray();

            foreach (var item in items)
                action(item);

            return items;
        }

        public static T PickRandom<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable as IList<T> ?? enumerable.ToArray();

            if (list.Count == 0)
                return default;

            var r = GRandom.Range(0, list.Count);

            return list[r];
        }

        public static TR[] ToArrayOf<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> action)
        {
            return enumerable.Select(action).ToArray();
        }
    }
}
