using System.Linq;
using ExpectedObjects;
using Gram.Rpg.Client.Application;
using Gram.Rpg.Client.Application.Exceptions;
using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Application.Repos;
using Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle;
using Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerLosesBattleUseCase;
using Gram.Rpg.Client.Domain.Entities;
using Gram.Rpg.Client.Infrastructure.Player;
using Gram.Rpg.Client.Tests.Builders;
using Moq;
using NUnit.Framework;

namespace Gram.Rpg.Client.Tests.Unit.Application.UseCases
{
    public class Tests_PlayerLossesBattle
    {
        private static readonly PlayerLosesBattleArgs args = new PlayerLosesBattleArgs(new[]
        {
            new BattleHero("hero1", 0)
        });

        private readonly HeroRepo _heroRepo = new HeroRepo(new[]
        {
            new Hero("hero5", "hero5", 44, 100)
        });

        private readonly OwnedHero[] _ownedHeroes =
        {
            new OwnedHero("hero1", "hero1", 10, 1, 1, 50),
            new OwnedHero("hero2", "hero2", 20, 2, 4, 67),
            new OwnedHero("hero3", "hero3", 15, 4, 2, 100),
        };

        private HeroAllocator         heroAllocator;
        private Mock<IPlayer1Gateway> p1Gateway;
        private IPlayer1              player;

        private          Mock<IPlayer1Provider> playerProvider;
        private readonly int                    startingLosses = 1;

        private readonly int                startingWins = 2;
        private          PlayerLossesBattle sut;


        [SetUp]
        public void Setup()
        {
            p1Gateway = new Mock<IPlayer1Gateway>();

            player = new PlayerBuilder
            {
                Wins   = startingWins,
                Losses = startingLosses,
                Heroes = _ownedHeroes
            }.Build();

            playerProvider = new Mock<IPlayer1Provider>();
            playerProvider.Setup(p => p.Get())
                .Returns(player);

            heroAllocator = new HeroAllocator(_heroRepo);

            sut = new PlayerLossesBattle
            {
                HeroAllocator   = heroAllocator,
                Player1Provider = playerProvider.Object,
                Player1Gateway  = p1Gateway.Object
            };
        }

        [Test]
        public void ThenNoHeroIsUpgraded()
        {
            var res = sut.Execute(args);

            var expected = _ownedHeroes.Cast<IOwnedHero>().ToArray();

            expected.ToExpectedObject().ShouldEqual(player.HeroInventory.ToArray());
        }

        [Test]
        public void ThenTheWinsAreNotIncreased()
        {
            var res = sut.Execute(args);

            Assert.AreEqual(startingWins, player.PlayerStats.TotalWins);
        }

        [Test]
        public void ThenTheLossesAreIncreased()
        {
            var res = sut.Execute(args);

            Assert.AreEqual(startingLosses + 1, player.PlayerStats.TotalLosses);
        }

        [Test]
        public void ThenTheTotalMatchesAreCorrect()
        {
            var res = sut.Execute(args);

            var expected = startingLosses + startingWins + 1;
            Assert.AreEqual(expected, player.PlayerStats.TotalBattles);
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

            var su = new PlayerLossesBattle()
            {
                HeroAllocator   = heroAllocator,
                Player1Provider = playerProvider.Object,
                Player1Gateway  = p1Gateway.Object
            };

            su.Execute(new PlayerLosesBattleArgs(new[]
            {
                new BattleHero("hero3", 0)
            }));

            Assert.AreEqual(5, player.PlayerStats.TotalBattles);
            Assert.AreEqual(4, player.HeroInventory.Count());
            Assert.IsTrue(player.HeroInventory.Has("hero5"));
        }

        [Test]
        public void WhenAliveHeroesExist_AnExceptionIsThrown()
        {
            var newArgs = new PlayerLosesBattleArgs(new[]
            {
                new BattleHero("hero1", 1)
            });

            Assert.Throws<AliveHeroException>(() => sut.Execute(newArgs));
        }
    }
}
