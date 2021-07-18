using System.Collections.Generic;

namespace Gram.Rpg.Client.Domain.Entities
{
    public interface IPlayer1
    {
        string         Id            { get; }
        IHeroInventory HeroInventory { get; }
        IPlayerStats   PlayerStats   { get; }
    }

    public class Player1 : IPlayer1
    {
        public Player1(string id, IEnumerable<OwnedHero> initialHeroes)
        {
            Id            = id;
            HeroInventory = new HeroInventory(initialHeroes);
            PlayerStats   = new PlayerStats();
        }

        public Player1(string id, IHeroInventory heroInventory, IPlayerStats playerStats)
        {
            HeroInventory = heroInventory;
            PlayerStats   = playerStats;
            Id            = id;
        }

        public string         Id            { get; }
        public IHeroInventory HeroInventory { get; }
        public IPlayerStats   PlayerStats   { get; }
    }
}
