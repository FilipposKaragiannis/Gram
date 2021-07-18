using Gram.Rpg.Client.Application.Factories;
using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Application.Repos;
using Gram.Rpg.Client.Application.UseCases;
using Gram.Rpg.Client.Application.UseCases.AppStarts;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;

namespace Gram.Rpg.Client.Application
{
    public class ContainerRegistrations : Module
    {
        public ContainerRegistrations(IWillDisposeYou disposer) : base(disposer)
        {
        }


        [Singleton] private IHeroRepo HeroRepo()
        {
            return new HeroRepo(Repos.HeroRepo.GetData());
        }

        [Instance] private IAppStarts IAppStarts()
        {
            return new AppStarts(Disposer);
        }

        [Instance] private IAppRequestsStateOfTheWorld IAppRequestsStateOfTheWorld()
        {
            return Instantiate<AppRequestsStateOfTheWorld>();
        }

        [Singleton] private IPlayer1Provider IPlayer1Provider()
        {
            return Resolver.Get<Player1Provider>();
        }

        [Singleton] private INewPlayerFactory INewPlayerFactory()
        {
            return Instantiate<NewPlayerFactory>();
        }

        [Singleton] private IPlayer1Store IPlayer1Store()
        {
            return Resolver.Get<Player1Provider>();
        }

        [Singleton] private Player1Provider Player1Provider()
        {
            return Instantiate<Player1Provider>();
        }
    }
}
