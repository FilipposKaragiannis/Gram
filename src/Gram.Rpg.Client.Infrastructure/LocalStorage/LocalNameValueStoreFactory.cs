namespace Gram.Rpg.Client.Infrastructure.LocalStorage
{
    public interface ILocalNameValueStoreFactory
    {
        ILocalNameValueStore Create(string scope);
        void                 Clear();

    }

    public class LocalNameValueStoreFactory : ILocalNameValueStoreFactory
    {
        private readonly FileStorage _fileStorage;

        public LocalNameValueStoreFactory(FileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public ILocalNameValueStore Create(string scope)
        {
            return new FileNameValueStore(scope, _fileStorage);
        }

        public void Clear()
        {
            _fileStorage.DeleteEverything();
        }
    }
}
