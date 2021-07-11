using System;
using System.IO;
using System.Linq;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Add<T>(this T[] array1, T[] array2)
        {
            // We are not adding to array1, we are combining the arrays to form a new one.
            // We always return an array instance different from the two passed in.
            // This is in keeping with methods like String.Replace

            if (array1 == null && array2 == null)
                throw new InvalidOperationException("Both arrays can't be null.");

            if (array1 == null && !array2.IsEmpty())
            {
                var result = new T[array2.Length];
                Array.Copy(array2, result, array2.Length);
                return result;
            }

            if (array2 == null && !array1.IsEmpty())
            {
                var result = new T[array1.Length];
                Array.Copy(array1, result, array1.Length);
                return result;
            }

            if (array1.IsEmpty() && array2.IsEmpty())
                return new T[0];

            if (array1.IsEmpty())
            {
                var result = new T[array2.Length];
                Array.Copy(array2, result, array2.Length);
                return result;
            }

            if (array2.IsEmpty())
            {
                var result = new T[array1.Length];
                Array.Copy(array1, result, array1.Length);
                return result;
            }


            var a1 = array1;
            var a2 = array2;

            var l1 = a1.Length;
            var l2 = a2.Length;

            var results = new T[l1 + l2];

            Array.Copy(a1, results, l1);
            Array.Copy(a2, 0,       results, l1, l2);

            return results;
        }

        public static T[] Combine<T>(params T[][] arrays)
        {
            // Combines an array of arrays into one array without the overhead of creating multiple
            // temp arrays as would happen when chaining Add<T>.

            if (arrays.Length == 0)
                return new T[0];

            var len = 0;
            for (var i = 0; i < arrays.Length; i++)
                len += arrays[i].Length;

            var dst = new T[len];
            var pos = 0;

            for (var i = 0; i < arrays.Length; i++)
            {
                var src = arrays[i];
                Array.Copy(src, 0, dst, pos, src.Length);
                pos += src.Length;
            }

            return dst;
        }

        public static T[] CombineWithArrays<T>(this T first, params T[][] arrays)
        {
            if (arrays.Length == 0)
                return new[] {first};

            var len = 0;
            for (var i = 0; i < arrays.Length; i++)
                len += arrays[i].Length;

            len = len + 1;

            var dst = new T[len];
            var pos = 1;

            dst[0] = first;

            foreach (var src in arrays)
            {
                Array.Copy(src, 0, dst, pos, src.Length);
                pos += src.Length;
            }

            return dst;
        }

        public static T[] CombineWithArrays<T>(this T[] first, params T[][] arrays)
        {
            if (arrays.Length == 0)
                return first;

            int pos;
            int len;
            T[] dst;

            if (first == null)
            {
                pos = 0;
                len = arrays.Sum(t => t?.Length ?? 0);
                dst = new T[len];
            }
            else
            {
                pos = first.Length;
                len = first.Length + arrays.Sum(t => t?.Length ?? 0);
                dst = new T[len];

                Array.Copy(first, 0, dst, 0, first.Length);
            }

            foreach (var src in arrays)
            {
                if (src == null)
                    continue;

                Array.Copy(src, 0, dst, pos, src.Length);
                pos += src.Length;
            }

            return dst;
        }

        public static bool HasElements(this Array array)
        {
            return !IsEmpty(array);
        }

        public static bool IsEmpty(this Array array)
        {
            if (array == null)
                return true;

            return array.Length == 0;
        }

        public static T[] Push<T>(this T[] array, T value)
        {
            var newArray = new T[array.Length + 1];

            Array.Copy(array, 0, newArray, 0, array.Length);

            newArray[array.Length] = value;

            return newArray;
        }

        public static T SafelyGet<T>(this T[] array, int index)
        {
            if (array == null || array.Length == 0)
            {
                G.LogWarning("Array was null. SafelyGet is returning default(T).");
                return default(T);
            }

            if (index < 0)
                index = 0;

            var i = Math.Min(index, array.Length - 1);

            return array[i];
        }

        public static int[] SortDesc(this int[] array)
        {
            var cloned = array.ToArray();

            Array.Sort(cloned, (m, n) => n - m);

            return cloned;
        }

        public static T[] Take<T>(this T[] array, int length)
        {
            if (array == null)
                throw new ArgumentException("array must not be null");

            if (array.Length < length)
                throw new InvalidOperationException("Taking more elements than in array.");


            if (length == 0)
                return new T[0];

            var results = new T[length];

            Array.Copy(array, results, length);

            return results;
        }

        public static T[] Take<T>(this T[] array, int index, int length)
        {
            if (array == null)
                throw new ArgumentException("array must not be null");

            if (array.Length - index < length)
                throw new InvalidDataException("There is not enough data to read a {0} elements from the array starting at {1}.".Fill(length, index));


            if (length == 0)
                return new T[0];

            var results = new T[length];

            Array.Copy(array, index, results, 0, length);

            return results;
        }

        public static string ToHexString(this byte[] bytes)
        {
            var  c = new char[bytes.Length * 2];
            byte b;

            for (var i = 0; i < bytes.Length; i++)
            {
                var a = bytes[i];

                b        = (byte) (a >> 4);
                c[i * 2] = (char) (b > 9 ? b + 0x57 : b + 0x30);

                b            = (byte) (a & 0xF);
                c[i * 2 + 1] = (char) (b > 9 ? b + 0x57 : b + 0x30);
            }

            return new string(c);
        }

        public static T[] TrimRight<T>(this T[] source, int trim)
        {
            if (source == null)
                throw new ArgumentException("source must not be null");

            if (source.Length == 0)
                throw new InvalidOperationException("source must not be empty");

            if (trim > source.Length)
                throw new InvalidOperationException("Trimming more elements than in source array.");


            var dest = new T[source.Length - trim];

            Array.Copy(source, 0, dest, 0, dest.Length);

            return dest;
        }
    }
}
