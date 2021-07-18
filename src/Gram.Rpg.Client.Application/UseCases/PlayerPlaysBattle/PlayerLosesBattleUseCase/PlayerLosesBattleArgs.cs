using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerLosesBattleUseCase
{
    public class PlayerLosesBattleArgs : PlayerPlaysBattleArgs
    {
        public PlayerLosesBattleArgs(BattleHero[] battleHeroes) : base(MatchResult.Lost, battleHeroes)
        {
        }
    }
}
