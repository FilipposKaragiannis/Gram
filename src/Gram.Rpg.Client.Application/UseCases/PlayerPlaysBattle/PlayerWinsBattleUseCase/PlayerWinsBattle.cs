using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Application.Exceptions;
using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Domain.Entities.Summaries;
using Gram.Rpg.Client.Infrastructure.Player;

namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerWinsBattleUseCase
{
    public interface IPlayerWinsBattle
    {
        PlayerWinsBattleResult Execute(PlayerWinsBattleArgs args);
    }

    public class PlayerWinsBattle : IPlayerWinsBattle
    {
        [Injected] public IPlayer1Gateway  Player1Gateway;
        [Injected] public IPlayer1Provider Player1Provider;
        [Injected] public IHeroAllocator   HeroAllocator;


        public PlayerWinsBattleResult Execute(PlayerWinsBattleArgs args)
        {
            var p1 = Player1Provider.Get();

            var heroesToUpgrade = args.BattleHeroes.Where(s => s.RemainingHealth > 0).ToArray();

            if (heroesToUpgrade == null || heroesToUpgrade.Length == 0)
                throw new GApplicationException("THere should be alive heroes on a win result");

            var inventory = p1.HeroInventory;

            var heroUpgradeSummaries = new List<HeroUpgradeSummary>();

            foreach (var hero in heroesToUpgrade)
            {
                var heroId = hero.Id;
                if (!inventory.Has(heroId))
                    throw new GApplicationException("Alive hero Id was not present on Player inventory");

                var playerHero         = p1.HeroInventory[heroId];
                var heroUpgradeSummary = new HeroUpgradeSummary(heroId);

                if (playerHero.ExperiencePoints < 4)
                    heroUpgradeSummary.ExperienceSummary = playerHero.AddExperiencePoints(1);
                else
                    heroUpgradeSummary.HeroLevelUpSummary = playerHero.LevelUp();

                heroUpgradeSummaries.Add(heroUpgradeSummary);
            }

            var statsSummary = p1.PlayerStats.PlayerWon(args.BattleHeroes.Select(s => s.Id));

            var heroReward = p1.TryAwardHero(HeroAllocator);

            Player1Gateway.SavePlayer(p1);

            return new PlayerWinsBattleResult
            {
                BattleWonSummary = new BattleWonSummary
                {
                    HeroUpgradeSummaries = heroUpgradeSummaries.ToArray(),
                    PlayerStatsSummary   = statsSummary,
                    HeroRewardSummary    = heroReward
                }
            };
        }
    }
}
