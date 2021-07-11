using System;

namespace Gram.Rpg.Client.Core
{
    public class GRandom
    {
          private static readonly Random random = new Random();

        public static bool Bool()
        {
            return Range(0, 2) == 0;
        }

        public static bool Bool(float n)
        {
            return Range(0, 1f) <= n;
        }

        public static T RandomChoice<T>(T obj1, T obj2)
        {
            return Range(0, 2) == 0 ? obj1 : obj2;
        }

        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }

        public static float Range(float min, float max)
        {
            var d = max - min;

            var r = (float)random.NextDouble();

            return min + d * r;
        }

        public static double Range(double min, double max)
        {
            var d = max - min;

            var r = random.NextDouble();

            return min + d * r;
        }

        public static int Sign()
        {
            return Range(0, 2) == 0 ? -1 : 1;
        }
    }
}
