using System;
using Newtonsoft.Json;

namespace Gram.Rpg.Client.Infrastructure.LocalStorage
{
    public interface ILocalNameValueStore
    {
        void      DeleteEverything_YesEverthingInEveryScope();
        void      Delete(string              name);
        bool      GetBool(string             name, bool      or = false);
        DateTime  GetDateTime(string         name, DateTime? or = null);
        decimal   GetDecimal(string          name, decimal   or = 0);
        float     GetFloat(string            name, float     or = 0);
        int       GetInt(string              name, int       or = 0);
        DateTime? GetNullableDateTime(string name, DateTime? or = null);
        int?      GetNullableInt(string      name, int?      or = null);
        T         GetObject<T>(string        name, T         or = default(T), JsonConverter converter = null) where T : class;
        string    GetString(string           name, string    or = null);
        bool      Has(string                 name);
        void      Remove(string              name);
        void      SetBool(string             name, bool     value);
        void      SetDateTime(string         name, DateTime value);
        void      SetDecimal(string          name, decimal  value);
        void      SetFlag(string             name);
        void      SetFloat(string            name, float  value);
        void      SetInt(string              name, int    value);
        void      SetObject(string           name, object value, JsonConverter converter = null);
        void      SetString(string           name, string value);
    }
}
