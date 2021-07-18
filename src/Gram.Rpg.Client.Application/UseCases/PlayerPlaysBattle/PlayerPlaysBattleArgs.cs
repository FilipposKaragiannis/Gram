using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle
{
    public class PlayerPlaysBattleArgs
    {
        public MatchResult  MatchResult  { get; set; }
        public BattleHero[] BattleHeroes { get; set; }
    }
}
