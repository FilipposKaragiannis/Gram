using Gram.Rpg.Client.Core;

namespace Gram.Rpg.Client.Domain.Values
{
    public static class AttackPower
    {
        private static int Min = 20;
        private static int Max = 100;

        public static int Weak        => GRandom.Range(Min, 40);
        public static int AverageLow  => GRandom.Range(40,  60);
        public static int AverageHigh => GRandom.Range(60,  80);
        public static int Strong      => GRandom.Range(80,  Max);
    }
}
