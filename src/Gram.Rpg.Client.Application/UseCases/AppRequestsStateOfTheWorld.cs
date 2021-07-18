using Gram.Rpg.Client.Application.Providers;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Domain.Entities;
using Gram.Rpg.Client.Infrastructure.Player;

namespace Gram.Rpg.Client.Application.UseCases
{
    public interface IAppRequestsStateOfTheWorld
    {
        void Execute();
    }

    public class AppRequestsStateOfTheWorld : IAppRequestsStateOfTheWorld
    {
        [Injected] public IPlayer1Gateway player1Gateway; 
        [Injected] public IPlayer1Store   Player1Store;

        public void Execute()
        {
            var p1 = player1Gateway.LoadPlayer();
            Player1Store.Set((Player1) p1);
        }
    }
}
