using System;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class NumberExtensions
    {
        private static float Round(this float n, int precision, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (float)Math.Round(n, precision, mode);
        }

        public static int RoundToInt(this float n, MidpointRounding mode = MidpointRounding.ToEven)
        {
            return (int)Round(n, 0, mode);
        }
    }
}
