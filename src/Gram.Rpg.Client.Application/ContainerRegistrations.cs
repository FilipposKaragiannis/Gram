using Gram.Rpg.Client.Application.Factories;
using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Application.Repos;
using Gram.Rpg.Client.Application.UseCases;
using Gram.Rpg.Client.Application.UseCases.AppStarts;
using Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerLosesBattleUseCase;
using Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle.PlayerWinsBattleUseCase;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;

namespace Gram.Rpg.Client.Application
{
    public class ContainerRegistrations : Module
    {
        public ContainerRegistrations(IWillDisposeYou disposer) : base(disposer)
        {
        }

        [Singleton] private IHeroRepo HeroRepo() => new HeroRepo(Repos.HeroRepo.GetData());

        [Instance] private IAppStarts IAppStarts() => new AppStarts(Disposer);

        [Instance] private IAppRequestsStateOfTheWorld IAppRequestsStateOfTheWorld() => Instantiate<AppRequestsStateOfTheWorld>();

        [Singleton] private IPlayer1Provider IPlayer1Provider() => Resolver.Get<Player1Provider>();

        [Singleton] private INewPlayerFactory INewPlayerFactory() => Instantiate<NewPlayerFactory>();

        [Singleton] private IPlayer1Store IPlayer1Store() => Resolver.Get<Player1Provider>();

        [Singleton] private Player1Provider Player1Provider() => Instantiate<Player1Provider>();

        [Singleton] private IPlayerWinsBattle IPlayerWinsBattle() => Instantiate<PlayerWinsBattle>();

        [Singleton] private PlayerLossesBattle PlayerLossesBattle() => Instantiate<PlayerLossesBattle>();

        [Singleton] private IHeroAllocator IHeroAllocator() => Instantiate<HeroAllocator>();
    }
}
