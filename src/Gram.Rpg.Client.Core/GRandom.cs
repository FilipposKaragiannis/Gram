using System;

namespace Gram.Rpg.Client.Core
{
    public class GRandom
    {
        private static readonly Random random = new Random();

        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
