using System;
using System.Linq;
using Gram.Rpg.Client.Application.Repos;
using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Application.Factories
{
    public interface INewPlayerFactory
    {
        IPlayer1 Create();

    }

    public class NewPlayerFactory : INewPlayerFactory
    {
        private readonly IHeroRepo heroRepo;

        public NewPlayerFactory(IHeroRepo heroRepo)
        {
            this.heroRepo = heroRepo;
        }

        public IPlayer1 Create()
        {
            var startingHeroes = heroRepo.GetRandomHeroes(3);

            var uid = Guid.NewGuid().ToString();
            return new Player1(uid,
                startingHeroes.Select(s => new OwnedHero(s)));
        }
    }
}
