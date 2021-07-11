using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class NumberExtensions
    {
        private const float Deg2Rad = YMath.Deg2Rad;
        private const float Rad2Deg = YMath.Rad2Deg;


        public static double Abs(this double n)
        {
            return Math.Abs(n);
        }

        public static float Abs(this float n)
        {
            return Math.Abs(n);
        }

        public static int Abs(this int n)
        {
            if (n == int.MinValue)
            {
                G.LogError("Min value ABS!!!");
                return int.MaxValue;
            }

            return Math.Abs(n);
        }

        public static long Abs(this long n)
        {
            return Math.Abs(n);
        }

        public static float Acos(this float f)
        {
            return (float)Math.Acos(f);
        }

        public static float Asin(this float f)
        {
            return (float)Math.Asin(f);
        }

        public static float Atan(this float f)
        {
            return (float)Math.Atan(f);
        }

        public static float AtanD(this float f)
        {
            return ((float)Math.Atan(f)).Degs();
        }

        public static bool BetweenInc(this decimal n, decimal lowerBound, decimal upperBound)
        {
            return n >= lowerBound && n <= upperBound;
        }

        public static bool BetweenExcl(this double n, double lowerBound, double upperBound)
        {
            return n >= lowerBound && n < upperBound;
        }

        public static bool BetweenInc(this float n, float lowerBound, float upperBound)
        {
            return n >= lowerBound && n <= upperBound;
        }

        public static bool BetweenInc(this int n, int lowerBound, int upperBound)
        {
            return n >= lowerBound && n <= upperBound;
        }

        public static bool BetweenInc(this long n, long lowerBound, long upperBound)
        {
            return n >= lowerBound && n <= upperBound;
        }

        public static double Cap(this double n, double upperBound)
        {
            return n > upperBound ? upperBound : n;
        }

        public static float Cap(this float n, float upperBound)
        {
            return n > upperBound ? upperBound : n;
        }

        public static int Cap(this int n, int upperBound)
        {
            return n > upperBound ? upperBound : n;
        }

        public static long Cap(this long n, long upperBound)
        {
            return n > upperBound ? upperBound : n;
        }

        public static uint Cap(this uint n, uint upperBound)
        {
            return n > upperBound ? upperBound : n;
        }

        public static int CeilToInt(this double n)
        {
            return Convert.ToInt32(Math.Ceiling(n));
        }

        public static double Collar(this double n, double lowerBound, double upperBound)
        {
            var capped = n.Cap(upperBound);

            return Floor(capped, lowerBound);
        }

        public static float Collar(this float n, float lowerBound, float upperBound)
        {
            var capped = n.Cap(upperBound);

            return Floor(capped, lowerBound);
        }

        public static int Collar(this int n, int lowerBound, int upperBound)
        {
            var capped = n.Cap(upperBound);

            return Floor(capped, lowerBound);
        }

        public static float Cos(this float f)
        {
            return (float)Math.Cos(f);
        }

        public static float Degs(this float f)
        {
            return f * Rad2Deg;
        }

        public static double Denorm(this double d, double min, double max)
        {
            return min + (max - min) * d;
        }

        public static float Denorm(this int i, float a, float b)
        {
            return Denorm(i / 100f, a, b);
        }

        public static float Denorm(this float f, float a, float b, bool constrain = false)
        {
            var v = a + (b - a) * f;

            if (!constrain)
                return v;

            if (a < b)
                return v.Collar(a, b);

            return v.Collar(b, a);
        }

        public static double Floor(this double n, double lowerBound)
        {
            return n < lowerBound ? lowerBound : n;
        }

        public static float Floor(this float n, float lowerBound)
        {
            return n < lowerBound ? lowerBound : n;
        }

        public static int Floor(this int n, int lowerBound)
        {
            return n < lowerBound ? lowerBound : n;
        }

        public static uint Floor(this uint n, uint lowerBound)
        {
            return n < lowerBound ? lowerBound : n;
        }

        public static ushort Floor(this ushort n, ushort lowerBound)
        {
            return n < lowerBound ? lowerBound : n;
        }

        public static int FloorToInt(this float n)
        {
            return Convert.ToInt32(Math.Floor(n));
        }

        public static decimal FloorY(this decimal n, decimal lowerBound)
        {
            return n < lowerBound ? lowerBound : n;
        }

        public static byte[] FromBase64(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Length == 0)
                return new byte[0];

            return Convert.FromBase64String(Encoding.UTF8.GetString(bytes));
        }

        public static byte[] GetBytes(this float n)
        {
            return BitConverter.GetBytes(n);
        }

        public static byte[] GetBytes(this int n)
        {
            return BitConverter.GetBytes(n);
        }

        public static byte[] GetBytes(this short n)
        {
            return BitConverter.GetBytes(n);
        }

        public static byte[] GetBytes(this ushort n)
        {
            return BitConverter.GetBytes(n);
        }

        public static byte[] GetBytes(this uint n)
        {
            return BitConverter.GetBytes(n);
        }

        public static byte[] GetBytes(this ulong n)
        {
            return BitConverter.GetBytes(n);
        }

        public static float InvariantParseFloat(this string s)
        {
            return float.Parse(s, CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this float f, string formatString)
        {
            return f.ToString(formatString, CultureInfo.InvariantCulture);
        }

        public static bool IsPowerOfTwo(this int n)
        {
            return n != 0 && (n & (n - 1)) == 0;
        }

        public static float Mult(this float n, float m)
        {
            return n * m;
        }

        public static float Norm(this double n, double min, double max)
        {
            return Norm((float)n, (float)min, (float)max);
        }

        public static float Norm(this double n, float min, float max)
        {
            return Norm((float)n, min, max);
        }

        public static float Norm(this float f, float min, float max)
        {
            var d = max - min;

            return (f - min).Over(d);
        }

        public static float Norm(this int n, int min, int max)
        {
            return Norm((float)n, min, max);
        }

        public static float Norm(this long n, long min, long max)
        {
            return Norm((float)n, min, max);
        }

        public static float Over(this float numerator, float denominator, float safeValue = 0)
        {
            try
            {
                if (denominator.RoughlyEquals(0))
                    return safeValue;

                return numerator / denominator;
            }
            catch (DivideByZeroException e)
            {
                G.LogWarning($"You're dividing by zero! Returning {safeValue} instead.\n{e}");
                return safeValue;
            }
        }

        public static float Over(this float numerator, double denominator, float safeValue = 0)
        {
            return Over(numerator, (float)denominator, safeValue);
        }

        public static float Over(this int numerator, float denominator, float safeValue = 0)
        {
            return Over((float)numerator, denominator, safeValue);
        }

        public static double Pow(this int n, double p)
        {
            return Math.Pow(n, p);
        }

        public static float Pow(this float n, double p)
        {
            return (float)Math.Pow(n, p);
        }

        public static string PP(this float f, int precision = 4, int scale = 2)
        {
            return f.PrettyPrint(precision, scale);
        }

        public static string PrettyPrint(this decimal d, int precision = 4, int scale = 2)
        {
            var formatString = "{{0,{0}:F{1}}}".Fill(precision + 2, scale);

            return formatString.Fill(d);
        }

        public static string PrettyPrint(this float f, int precision = 4, int scale = 2)
        {
            var formatString = "{{0,{0}:F{1}}}".Fill(precision + 2, scale);

            return formatString.Fill(f);
        }

        public static string PrettyPrint(this float? f, int precision = 4, int scale = 2)
        {
            if (f == null)
                return "{{0,{0}}}".Fill(precision + 2 + scale).Fill("?");

            return f.Value.PrettyPrint(precision, scale);
        }

        public static float Rads(this float f)
        {
            return f * Deg2Rad;
        }

        public static bool RoughlyEquals(this double n, double other)
        {
            return Math.Abs(n - other) < 0.01;
        }

        public static bool RoughlyEquals(this float n, float other, int precision = 5)
        {
            if (precision < 0)
            {
                G.LogWarning("Precision must not be less than 0. Defaulting to 0.");
                precision = 0;
            }

            if (precision > 20)
            {
                G.LogWarning("Precision must not be greater than 20. Defaulting to 20.");
                precision = 20;
            }

            float epsilon;

            switch (precision)
            {
                case 0:
                    // This was changed on 20/10/20 from 0f to 1f. Zero was just wrong. I'm nervous to change it as it
                    // could upset features but a search shows no obvious uses of (precision = 0). 
                    epsilon = 1f;
                    break;
                case 1:
                    epsilon = 0.1f;
                    break;
                case 2:
                    epsilon = 0.01f;
                    break;
                case 3:
                    epsilon = 0.001f;
                    break;
                case 4:
                    epsilon = 0.0001f;
                    break;
                case 5:
                    epsilon = 0.00001f;
                    break;
                default:
                    epsilon = (float)Math.Pow(10, -precision);
                    break;
            }

            return Math.Abs(n - other) < epsilon;

            var d = Math.Abs(n - other);

            var a = Math.Abs(n);
            var b = Math.Abs(other);

            var l = b > a ? b : a;

            return d <= l * epsilon;
        }

        public static float Round(this float n, int precision, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (float)Math.Round(n, precision, mode);
        }

        public static decimal Round(this decimal n, int precision, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return Math.Round(n, precision, mode);
        }

        public static double Round(this double n, int precision, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return Math.Round(n, precision, mode);
        }

        public static int RoundDown(this float n)
        {
            //  1.0 ->  1.0
            //  1.1 ->  1.0
            //  1.9 ->  1.0
            // -1.0 -> -1.0
            // -1.1 -> -2.0
            // -1.9 -> -2.0

            var t = (int)n.Truncate();

            if (n > 0)
                return t;

            if (n.RoughlyEquals(t))
                return t;

            return t - 1;
        }

        public static int RoundToInt(this float n, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (int)Round(n, 0, mode);
        }

        public static int RoundToInt(this double n, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (int)Round(n, 0, mode);
        }

        public static int RoundToInt(this decimal n, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (int)Round(n, 0, mode);
        }

        public static int RoundToMultipleOf(this float n, int multiple, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (int)Round(n / multiple, 0, mode) * multiple;
        }

        public static int RoundToMultipleOf(this double n, int multiple, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (int)Round(n / multiple, 0, mode) * multiple;
        }

        public static int RoundUp(this float n)
        {
            //  1.0 ->  1.0
            //  1.1 ->  2.0
            // -1.0 -> -1.0
            // -1.1 -> -1.0

            var t = (int)n.Truncate();

            if (n < 0)
                return t;

            if (n.RoughlyEquals(t))
                return t;

            return t + 1;
        }

        public static int Sign(this decimal n)
        {
            return Math.Sign(n);
        }

        public static int Sign(this double n)
        {
            return Math.Sign(n);
        }

        public static int Sign(this float n)
        {
            return Math.Sign(n);
        }

        public static int Sign(this int n)
        {
            return Math.Sign(n);
        }
        
        public static float Sin(this float f)
        {
            return (float)Math.Sin(f);
        }

        public static float Sqr(this float n)
        {
            return n * n;
        }

        public static float Sqrt(this float n)
        {
            return (float)Math.Sqrt(n);
        }

        public static float Subtract(this float n, float subtractant)
        {
            return n - subtractant;
        }


        public static float SwapSign(this float n)
        {
            return n * -1;
        }

        public static float SwapSign(this int n)
        {
            return n * -1;
        }

        public static bool TryInvariantParseFloat(this string s, out float retVal)
        {
            return float.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out retVal);
        }

        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static IEnumerable<byte> ToBytes(this IEnumerable<int> ints)
        {
            return ints?.Select(i => (byte)i);
        }

        public static IEnumerable<int> ToInts(this IEnumerable<byte> bytes)
        {
            return bytes?.Select(b => (int)b);
        }

        public static string ToUTF8(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Length == 0)
                return string.Empty;

            return Encoding.UTF8.GetString(bytes);
        }

        public static float Truncate(this float n)
        {
            return (float)Math.Truncate(n);
        }
    }
}
