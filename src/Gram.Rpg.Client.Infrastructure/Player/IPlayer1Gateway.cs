using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Infrastructure.Player
{
    public interface IPlayer1Gateway
    {
        IPlayer1 LoadPlayer();
        void     SavePlayer(IPlayer1 player1);
    }
}
