using Gram.Rpg.Client.Application.Exceptions;
using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Application.Providers
{
    public interface IPlayer1Provider
    {
        IPlayer1 Get();
    }


    public interface IPlayer1Store
    {
        void Set(Player1 p1);
    }


    public class Player1Provider : IPlayer1Provider, IPlayer1Store
    {
        private Player1 _p1;

        IPlayer1 IPlayer1Provider.Get()
        {
            if (_p1 == null)
                throw new GApplicationException("Player1 is not yet stored.");

            return _p1;
        }

        void IPlayer1Store.Set(Player1 p1) => _p1 = p1;
    }
}
