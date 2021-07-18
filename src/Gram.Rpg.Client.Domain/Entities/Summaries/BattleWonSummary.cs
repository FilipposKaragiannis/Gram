using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Core.Domain.Values;

namespace Gram.Rpg.Client.Domain.Entities.Summaries
{
    public class BattleWonSummary
    {
        public             HeroUpgradeSummary[] HeroUpgradeSummaries { get; set; }
        public             PlayerStatsSummary   PlayerStatsSummary   { get; set; }
        [CanBeNull] public StringArraySummary   HeroRewardSummary    { get; set; }
    }
    
    public class BattleLostSummary
    {
        public             PlayerStatsSummary   PlayerStatsSummary   { get; set; }
        [CanBeNull] public StringArraySummary   HeroRewardSummary    { get; set; }
    }

    public class HeroUpgradeSummary
    {
        public HeroUpgradeSummary(string heroId)
        {
            HeroId = heroId;
        }

        public             string             HeroId             { get; }
        public             IntSummary         ExperienceSummary  { get; set; }
        [CanBeNull] public HeroLevelUpSummary HeroLevelUpSummary { get; set; }
    }

    public class HeroLevelUpSummary
    {
        public IntSummary LevelSummary       { get; set; }
        public IntSummary AttackPowerSummary { get; set; }
        public IntSummary MaxHealthSummary   { get; set; }
        public IntSummary ExperienceSummary  { get; set; }
    }

    public class PlayerStatsSummary
    {
        public IntSummary WinsSummary   { get; set; }
        public IntSummary LossesSummary { get; set; }
    }
}
