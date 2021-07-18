using System.Linq;
using ExpectedObjects;
using Gram.Rpg.Client.Application;
using Gram.Rpg.Client.Application.Exceptions;
using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Application.Repos;
using Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle;
using Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerWinsBattleUseCase;
using Gram.Rpg.Client.Domain.Entities;
using Gram.Rpg.Client.Infrastructure.Player;
using Gram.Rpg.Client.Tests.Builders;
using Moq;
using NUnit.Framework;

namespace Gram.Rpg.Client.Tests.Unit.Application.UseCases
{
    public class Tests_PlayerWinsBattle
    {
        private PlayerWinsBattle     sut;
        private IPlayer1             player;

        private int startingWins   = 2;
        private int startingLosses = 1;

        private static readonly PlayerWinsBattleArgs args = new PlayerWinsBattleArgs(new[]
        {
            new BattleHero("hero1", 100)
        });

        private Mock<IPlayer1Provider> playerProvider;

        private readonly OwnedHero[] _ownedHeroes = {
            new OwnedHero("hero1", "hero1", 10, 1, 1, 50),
            new OwnedHero("hero2", "hero2", 20, 2, 4, 67),
            new OwnedHero("hero3", "hero3", 15, 4, 2, 100),
        };

        private readonly HeroRepo _heroRepo = new HeroRepo(new []
        {
            new Hero("hero5", "hero5", 44, 100)
        });

        private HeroAllocator         heroAllocator;
        private Mock<IPlayer1Gateway> p1Gateway;


        [SetUp]
        public void Setup()
        {
            p1Gateway = new Mock<IPlayer1Gateway>();

            player = new PlayerBuilder
            {
                Wins = startingWins,
                Losses = startingLosses,
                Heroes =_ownedHeroes
            }.Build();

            playerProvider  = new Mock<IPlayer1Provider>();
            playerProvider.Setup(p => p.Get())
                .Returns(player);

            heroAllocator = new HeroAllocator(_heroRepo);
            
            sut = new PlayerWinsBattle
            {
                HeroAllocator   = heroAllocator, 
                Player1Provider = playerProvider.Object,
                Player1Gateway  = p1Gateway.Object
            };
        }

        [Test]
        public void ThenTheWinsAreIncreased()
        {
            var res = sut.Execute(args);

            Assert.AreEqual(startingWins + 1, player.PlayerStats.TotalWins);
        }
        
        [Test]
        public void ThenTheLossesAreNotChanged()
        {
            var res = sut.Execute(args);

            Assert.AreEqual(startingLosses, player.PlayerStats.TotalLosses);
        }
        
        [Test]
        public void ThenTheTotalMatchesAreCorrect()
        {
            var res = sut.Execute(args);

            var expected = startingLosses + startingWins + 1;
            Assert.AreEqual(expected, player.PlayerStats.TotalBattles);
        }

        [Test]
        public void WhenTheExperiencePointsOfTheHeroAreLessThanThreshold_TheHeroGainsExperience()
        {
            var res = sut.Execute(new PlayerWinsBattleArgs(new[]
            {
                new BattleHero("hero2", 1)
            }));

            var expected = new OwnedHero("hero2", "hero2", 20, 3, 4, 67);

            expected.ToExpectedObject().ShouldEqual(player.HeroInventory["hero2"]);
        }

        [Test]
        public void WhenTheExperiencePointsOfTheHeroReachesThreshold_TheHeroLevelsUp()
        {
            var res = sut.Execute(new PlayerWinsBattleArgs(new[]
            {
                new BattleHero("hero3", 1)
            }));

            var expectedXp        = 0;
            var expectedLevel     = 3;
            var expectedMaxHealth = 110;

            var hero = player.HeroInventory["hero3"];

            Assert.AreEqual(expectedXp,        hero.ExperiencePoints);
            Assert.AreEqual(expectedLevel,     hero.Level);
            Assert.AreEqual(expectedMaxHealth, hero.MaxHealth);
        }

        [Test]
        public void WhenThePlayerHasEnoughMatches_ThenAHeroIsAwarded()
        {
            player = new PlayerBuilder
            {
                Wins   = 2,
                Losses = 2,
                Heroes = _ownedHeroes
            }.Build();

            playerProvider.Setup(s => s.Get())
                .Returns(player);

            var su = new PlayerWinsBattle
            {
                HeroAllocator   = heroAllocator,
                Player1Provider = playerProvider.Object,
                Player1Gateway  = p1Gateway.Object
            };

            su.Execute(new PlayerWinsBattleArgs(new[]
            {
                new BattleHero("hero3", 1)
            }));

            Assert.AreEqual(5, player.PlayerStats.TotalBattles);
            Assert.AreEqual(4, player.HeroInventory.Count());
            Assert.IsTrue(player.HeroInventory.Has("hero5"));
        }

        [Test]
        public void WhenNoAliveHeroesExist_AnExceptionIsThrown()
        {
            var newArgs = new PlayerWinsBattleArgs(new BattleHero[0]);

            Assert.Throws<NoAliveHeroesException>(() => sut.Execute(newArgs));
        }

        [Test]
        public void WhenHeroUsedIsMissingFromPlayerInventory_AnExceptionIsThrown()
        {
            var newArgs = new PlayerWinsBattleArgs(new[]
            {
                new BattleHero("NonHero1", 100)
            });

            Assert.Throws<MissingHeroException>(() => sut.Execute(newArgs));
        }
    }
}
