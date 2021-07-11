using System;
using System.Linq;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core
{
    public static partial class YMath
    {
        public const float Deg2Rad = 0.01745329f;
        public const float PI      = 3.141593f;
        public const float Rad2Deg = 57.29578f;
        public static float Acos(float radians)
        {
            return (float) Math.Acos(radians);
        }
        public static float Atan(float radians)
        {
            return (float) Math.Atan(radians);
        }
        public static float Atan2(float y, float x)
        {
            return (float) Math.Atan2(y, x);
        }
        public static float Cos(float radians)
        {
            return (float) Math.Cos(radians);
        }
        public static float Sin(float radians)
        {
            return (float) Math.Sin(radians);
        }
        public static float Tan(float radians)
        {
            return (float) Math.Tan(radians);
        }

        public static float Abs(float value)
        {
            return Math.Abs(value);
        }

        public static long Abs(long value)
        {
            return Math.Abs(value);
        }

        public static int Average(int value1, params int[] otherValues)
        {
            if (value1 == 0 && otherValues.Sum() == 0)
                return 0;

            return ((float) (value1 + otherValues.Sum()) / (1 + otherValues.Length)).RoundToInt();
        }

        public static float Max(float val1, float val2)
        {
            return Math.Max(val1, val2);
        }

        public static int Max(int val1, int val2)
        {
            return Math.Max(val1, val2);
        }

        public static decimal Max(decimal val1, decimal val2)
        {
            return Math.Max(val1, val2);
        }

        public static int? Max(int? val1, int? val2)
        {
            if (!val1.HasValue)
                return val2;

            if (!val2.HasValue)
                return val1;

            return Math.Max(val1.Value, val2.Value);
        }

        public static DateTime Max(DateTime val1, DateTime val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static float Min(float val1, float val2)
        {
            return Math.Min(val1, val2);
        }

        public static int Min(int val1, int val2)
        {
            return Math.Min(val1, val2);
        }

        public static int? Min(int? val1, int? val2)
        {
            if (!val1.HasValue)
                return val2;

            if (!val2.HasValue)
                return val1;

            return Math.Min(val1.Value, val2.Value);
        }

        public static float Pow(float value, float power)
        {
            return (float) Math.Pow(value, power);
        }

        /// <summary>
        /// Given the length of two sides (of a right triangle), calculates by Pythagorean Theorem the length of the hypotenuse.
        /// </summary>
        public static float Pythag(float side1, float side2)
        {
            return (side1 * side1 + side2 * side2).Sqrt();
        }

        public static float RoundToNearestMultiple(float val, float multiple)
        {
            var factor = val / multiple;
            return factor.RoundToInt() * multiple;
        }

        public static long RoundDownToNearestMultiple(long val, long multiple)
        {
            var factor = val / multiple;
            return factor * multiple;
        }

        public static float Sqrt(float value)
        {
            return (float) Math.Sqrt(value);
        }
    }
}
