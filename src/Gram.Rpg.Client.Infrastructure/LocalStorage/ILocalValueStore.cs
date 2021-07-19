using Newtonsoft.Json;

namespace Gram.Rpg.Client.Infrastructure.LocalStorage
{
    public interface ILocalNameValueStore
    {
        T         GetObject<T>(string        name, T         or = default, JsonConverter converter = null) where T : class;
        void      SetObject(string           name, object value, JsonConverter converter = null);
    }
}
