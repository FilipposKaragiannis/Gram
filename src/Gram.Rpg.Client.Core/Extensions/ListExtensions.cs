using System;
using System.Collections.Generic;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class ListExtensions
    {
        public static void AddRanges<T>(this List<T> list, params IEnumerable<T>[] collection)
        {
            foreach (var c in collection)
                list.AddRange(c);
        }

        public static void RemoveFirst<T>(this LinkedList<T> list, Func<T, bool> predicate, int n = 1)
        {
            var i = 0;
            for (var cur = list.First; cur != null; cur = cur.Next)
                if (predicate(cur.Value))
                {
                    list.Remove(cur);
                    if (++i == n)
                        return;
                }
        }

        public static void RemoveLast<T>(this LinkedList<T> list, Func<T, bool> predicate, int n = 1)
        {
            var i = 0;
            for (var cur = list.Last; cur != null; cur = cur.Previous)
                if (predicate(cur.Value))
                {
                    list.Remove(cur);
                    if (++i == n)
                        return;
                }
        }

        public static LinkedListNode<T> RemoveThenReturnNext<T>(this LinkedList<T> list, LinkedListNode<T> cur)
        {
            if (cur == null)
                return null;

            var tmp = cur.Next;
            list.Remove(cur);
            return tmp;
        }

        public static LinkedListNode<T> RemoveThenReturnPrevious<T>(this LinkedList<T> list, LinkedListNode<T> cur)
        {
            if (cur == null)
                return null;

            var tmp = cur.Previous;
            list.Remove(cur);
            return tmp;
        }

        public static void RemoveWhere<T>(this LinkedList<T> list, Func<T, bool> predicate)
        {
            for (var cur = list.First; cur != null; cur = cur.Next)
                if (predicate(cur.Value))
                    list.Remove(cur);
        }
    }
}
