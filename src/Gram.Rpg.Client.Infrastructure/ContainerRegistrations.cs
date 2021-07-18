using System.IO;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Infrastructure.LocalStorage;
using Gram.Rpg.Client.Infrastructure.Mappers;
using Gram.Rpg.Client.Infrastructure.Player;

namespace Gram.Rpg.Client.Infrastructure
{
    public class ContainerRegistrations : Module
    {
        public ContainerRegistrations(IWillDisposeYou disposer) : base(disposer)
        {
        }

        [Singleton]
        private ILocalNameValueStoreFactory ILocalNameValueStoreFactory()
        {
            var data = "data";

            var path = UnityEngine.Application.isEditor
                ? Path.Combine(Directory.GetCurrentDirectory(),            "localStorage")
                : Path.Combine(UnityEngine.Application.persistentDataPath, data);

            var fileStorage = new FileStorage(path);

            return new LocalNameValueStoreFactory(fileStorage);
        }
        
        [Singleton]
        private IPlayer1Gateway IPlayer1Gateway()
        {
            var localNameValueStoreFactory = Resolver.Get<ILocalNameValueStoreFactory>();
            var player1Mapper              = Resolver.Get<IPlayer1Mapper>();
                
            return new Player1Gateway(Disposer, localNameValueStoreFactory, player1Mapper);
        }
        
        [Singleton]
        private IPlayer1Mapper IPlayer1Mapper()
        {
            return new Player1Mapper();
        }
    }
}
