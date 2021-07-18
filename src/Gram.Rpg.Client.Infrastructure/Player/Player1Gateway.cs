using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Domain.Entities;
using Gram.Rpg.Client.Infrastructure.Dtos;
using Gram.Rpg.Client.Infrastructure.LocalStorage;
using Gram.Rpg.Client.Infrastructure.Mappers;

namespace Gram.Rpg.Client.Infrastructure.Player
{
    public class Player1Gateway : GObject, IPlayer1Gateway
    {
        private readonly ILocalNameValueStore localStore;
        private readonly IPlayer1Mapper       _player1Mapper;

        private const string playerKey = "Player";

        public Player1Gateway(IWillDisposeYou disposer, ILocalNameValueStoreFactory localNameValueStoreFactory, IPlayer1Mapper player1Mapper)
            : base(disposer)
        {
            _player1Mapper = player1Mapper;
            localStore     = localNameValueStoreFactory.Create("Player1");
        }

        public IPlayer1 LoadPlayer()
        {
            var playerDto = localStore.GetObject<PlayerDto>(playerKey);

            if (playerDto == null)
            {
                var player = new Player1();
                localStore.SetObject(playerKey,
                    _player1Mapper.ToDto(player));
                return player;
            }

            return _player1Mapper.FromDto(playerDto);
        }
    }
}
