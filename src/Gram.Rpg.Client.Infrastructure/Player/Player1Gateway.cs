using System;
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
                G.LogWarning($"No player record found in local history");
                return null;
            }

            return _player1Mapper.FromDto(playerDto);
        }

        public void SavePlayer(IPlayer1 player1)
        {
            try
            {
                localStore.SetObject(playerKey,
                    _player1Mapper.ToDto(player1));
            }
            catch (Exception e)
            {
                G.LogError(e);
                throw new ApplicationException($"Failed to save player to local storage.{e}");
            }
        }
    }
}
