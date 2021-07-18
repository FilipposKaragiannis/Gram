using System.Linq;
using Gram.Rpg.Client.Application.Exceptions;
using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Domain.Entities.Summaries;
using Gram.Rpg.Client.Infrastructure.Player;

namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerLosesBattleUseCase
{
    public interface IPlayerLossesBattle
    {
        PlayerLosesBattleResult Execute(PlayerLosesBattleArgs args);
    }

    public class PlayerLossesBattle : IPlayerLossesBattle
    {
        [Injected] public IHeroAllocator   HeroAllocator;
        [Injected] public IPlayer1Gateway  Player1Gateway;
        [Injected] public IPlayer1Provider Player1Provider;

        public PlayerLosesBattleResult Execute(PlayerLosesBattleArgs args)
        {
            var p1 = Player1Provider.Get();

            var usedHeroes = args.BattleHeroes;

            if (usedHeroes.Any(s => s.RemainingHealth > 0))
                throw new AliveHeroException();
            
            var statsSummary = p1.PlayerStats.PlayerLost(args.BattleHeroes.Select(s => s.Id));

            var heroReward = p1.TryAwardHero(HeroAllocator);

            Player1Gateway.SavePlayer(p1);

            return new PlayerLosesBattleResult
            {
                BattleLostSummary = new BattleLostSummary
                {
                    PlayerStatsSummary = statsSummary,
                    HeroRewardSummary  = heroReward
                }
            };
        }
    }
}
