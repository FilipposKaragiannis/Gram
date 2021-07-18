using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerWinsBattleUseCase
{
    public class PlayerWinsBattleArgs : PlayerPlaysBattleArgs
    {
        public PlayerWinsBattleArgs(BattleHero[] battleHeroes) : base(MatchResult.Won, battleHeroes)
        {
        }
    }
}
