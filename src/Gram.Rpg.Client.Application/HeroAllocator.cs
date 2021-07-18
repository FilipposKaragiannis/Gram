using Gram.Rpg.Client.Application.Repos;
using Gram.Rpg.Client.Core.Domain.Values;
using Gram.Rpg.Client.Core.Extensions;
using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Application
{
    public interface IHeroAllocator
    {
        StringArraySummary AllocateNewHero(IPlayer1 player1);
    }

    public class HeroAllocator : IHeroAllocator
    {
        private readonly IHeroRepo _heroRepo;

        public HeroAllocator(IHeroRepo heroRepo)
        {
            _heroRepo = heroRepo;
        }

        public StringArraySummary AllocateNewHero(IPlayer1 player1)
        {
            var heroes  = player1.HeroInventory.ToArrayOf(s => s.Id);
            var newHero = _heroRepo.GetRandomHero();

            player1.HeroInventory.Add(newHero);

            return new StringArraySummary(heroes)
            {
                New = player1.HeroInventory.ToArrayOf(s => s.Id)
            };
        }
    }
}
