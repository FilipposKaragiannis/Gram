using System;
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
        public Player1()
        {
            Id            = Guid.NewGuid().ToString();
            HeroInventory = new HeroInventory(new Dictionary<string, OwnedHero>()
            {
                {"hero1", new OwnedHero("HRO01", "Lion", 100, 20, 0,0)},
                {"hero2", new OwnedHero("HRO02", "Lina", 30, 110, 0,0)},
                {"hero3", new OwnedHero("HRO03", "Tide", 40, 110, 21,10)},
                
            });
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
