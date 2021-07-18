using System.Collections.Generic;
using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Tests.Builders
{
    public class PlayerBuilder
    {
        public int                    Wins   { get; set; }
        public int                    Losses { get; set; } 
        public IEnumerable<OwnedHero> Heroes { get; set; }
        
        public PlayerBuilder()
        {
            Heroes = new List<OwnedHero>();

        }

        public IPlayer1 Build()
        {
            var stats = new PlayerStats();
            
            for (var i = 0; i < Wins; i++)
                stats.PlayerWon(new string[0]);
            
            for (var i = 0; i < Losses; i++)
                stats.PlayerLost(new string[0]);

            return new Player1("PlayerId", new HeroInventory(Heroes), stats);
        }
    }
}
