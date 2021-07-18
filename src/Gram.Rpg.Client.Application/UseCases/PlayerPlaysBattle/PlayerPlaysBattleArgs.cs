using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle
{
    public abstract class PlayerPlaysBattleArgs
    {
        protected PlayerPlaysBattleArgs(MatchResult matchResult, BattleHero[] battleHeroes)
        {
            MatchResult       = matchResult;
            BattleHeroes = battleHeroes;
        }

        public MatchResult  MatchResult  { get; }
        public BattleHero[] BattleHeroes { get; }
    }
}
