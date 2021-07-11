using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int BinarySearch<T>(this IEnumerable<T> list, T item, IComparer<T> comparer)
        {
            return Array.BinarySearch(list.ToArray(), item, comparer);
        }

        /// <summary>
        /// Like SequenceEqual, if the order of elements does not matter and you know there are no duplicates.
        /// </summary>
        public static bool ContainsTheSameElements<T>(this IEnumerable<T> one, IEnumerable<T> other)
        {
            if (one == null || other == null) return one == null && other == null;

            var oneItems   = one as T[]   ?? one.ToArray();
            var otherItems = other as T[] ?? other.ToArray();

            if (oneItems.Length != otherItems.Length)
                return false;

            return !oneItems.Except(otherItems).Any() && !otherItems.Except(oneItems).Any();
        }

        public static IEnumerable<T> Except<T, T2>(this IEnumerable<T> enumerable, IEnumerable<T2> enumerable2, Func<T, T2> selector)
        {
            return enumerable.Where(t => !enumerable2.Contains(selector(t)));
        }

        public static (T, int) FirstOrDefaultWithIndex<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var items = enumerable as T[] ?? enumerable.ToArray();

            for (var i = 0; i < items.Length; i++)
                if (predicate(items[i]))
                    return (items[i], i);

            return (default, -1);
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var items = enumerable as T[] ?? enumerable.ToArray();

            foreach (var item in items)
                action(item);

            return items;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            var items = enumerable as T[] ?? enumerable.ToArray();

            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                action(item, i);
            }

            return items;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            return enumerable == null || !enumerable.GetEnumerator().MoveNext();
        }

        public static (T, int) LastOrDefaultWithIndex<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            var items = enumerable as T[] ?? enumerable.ToArray();

            for (var i = items.Length - 1; i > -1; i--)
                if (predicate(items[i]))
                    return (items[i], i);

            return (default, -1);
        }

        public static IEnumerable<TR> SelectIfNotNull<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> action)
        {
            return enumerable.Select(action)
                .WhereNotNull();
        }

        [Pure]
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable.ToArray();

            for (var i = array.Length - 1; i > 0; i--)
            {
                var randIndex = GRandom.Range(0, i + 1);

                var temp = array[randIndex];
                array[randIndex] = array[i];
                array[i]         = temp;
            }

            return array;
        }

        public static T PickRandom<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable as IList<T> ?? enumerable.ToArray();

            if (list.Count == 0)
                return default(T);

            var r = GRandom.Range(0, list.Count);

            return list[r];
        }


        public static TR[] ToArrayOf<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> action)
        {
            return enumerable.Select(action).ToArray();
        }

        public static IDictionary<TKey, TSource> ToDictionary<TKey, TSource>(this TSource[] array, Func<TSource, int, TKey> selector)
        {
            var dictionary = new Dictionary<TKey, TSource>(array.Length);

            for (var i = 0; i < array.Length; i++)
                dictionary.Add(selector(array[i], i), array[i]);

            return dictionary;
        }

        public static TR[] ToNotNullArrayOf<T, TR>(this IEnumerable<T> enumerable, Func<T, TR> action)
        {
            return enumerable.Select(action).WhereNotNull().ToArray();
        }

        public static TR[] ToOrderedArrayOf<T, TR, TOr>(this IEnumerable<T> enumerable, Func<T, TR> projector, Func<TR, TOr> orderer)
        {
            return enumerable.Select(projector).OrderBy(orderer).ToArray();
        }

        public static bool TryGetFirstOfType<TSource, TDest>(this IEnumerable<TSource> enumerable, out TDest item) where TDest : TSource
        {
            var found = enumerable.FirstOrDefault(i => i is TDest);

            if (found == null)
            {
                item = default(TDest);
                return false;
            }

            item = (TDest) found;
            return true;
        }

        public static async Task<IEnumerable<T>> Where<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            // https://stackoverflow.com/a/14895226/4571656
            var results = new ConcurrentQueue<T>();
            var tasks = source.Select(
                async x =>
                {
                    if (await predicate(x))
                        results.Enqueue(x);
                });
            await Task.WhenAll(tasks);
            return results;
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Where(t => t != null);
        }

        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> collection, int batchSize)
        {
            var nextbatch = new List<T>(batchSize);

            foreach (var item in collection)
            {
                nextbatch.Add(item);

                if (nextbatch.Count == batchSize)
                {
                    yield return nextbatch;

                    nextbatch = new List<T>(batchSize);
                }
            }

            if (nextbatch.Count > 0)
                yield return nextbatch;
        }

        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            return collection.Select((value, index) => new {value, index})
                .Where(x => predicate(x.value))
                .Select(x => x.index);
        }

        [NotNull]
        [Pure]
        public static T[] SafeConcat<T>([CanBeNull] this T[] one, [CanBeNull] T[] other)
        {
            if (one == null)
                return other ?? Array.Empty<T>();

            return other == null ? one : one.Concat(other).ToArray();
        }
    }
}
