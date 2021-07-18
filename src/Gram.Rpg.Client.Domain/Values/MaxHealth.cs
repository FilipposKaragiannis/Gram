using Gram.Rpg.Client.Core;

namespace Gram.Rpg.Client.Domain.Values
{
    public static class MaxHealth
    {
        public static int Min = 30;
        public static int Max = 200;

        public static int WeakLow     => GRandom.Range(Min, 50);
        public static int WeakHigh    => GRandom.Range(50,  80);
        public static int AverageLow  => GRandom.Range(80,  105);
        public static int AverageHigh => GRandom.Range(105, 130);
        public static int StrongLow   => GRandom.Range(130, 160);
        public static int StrongHigh  => GRandom.Range(160, 180);
        public static int Immortal    => GRandom.Range(180, Max);
    }
}
