using System;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;
using Newtonsoft.Json;

namespace Gram.Rpg.Client.Infrastructure.LocalStorage
{
    public class FileNameValueStore : ILocalNameValueStore
    {
        private readonly string      scope;
        private readonly FileStorage storage;

        public FileNameValueStore(string scope, FileStorage storage)
        {
            this.scope   = scope;
            this.storage = storage;
        }

        public T GetObject<T>(string name, T or = default, JsonConverter converter = null) where T : class
        {
            string json;

            try
            {
                json = DecryptAsString(name);
            }
            catch (Exception e)
            {
                G.LogException("Could not GetObject<{0}>: [{1}]. Defaulting to: [{2}]".Fill(typeof(T).Name, name, or), e);
                TryDelete(name);
                return or;
            }


            if (json == null)
                return or;

            try
            {
                if (converter == null)
                    return JsonConvert.DeserializeObject<T>(json);

                return JsonConvert.DeserializeObject<T>(json, converter);
            }
            catch (Exception e)
            {
                G.LogException(
                    "Error deserialising JSON for LocalNameValueStore with key [{0}]. Falling back on the default value.".Fill(GetScopedKey(name)),
                    e);
                TryDelete(name);
                return or;
            }
        }

        public void SetObject(string name, object value, JsonConverter converter = null)
        {
            try
            {
                if (value == null)
                {
                    Delete(name);
                    return;
                }

                string json;

                if (converter == null)
                    json = JsonConvert.SerializeObject(value);
                else
                    json = JsonConvert.SerializeObject(value, converter);

                Encrypt(name, json);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetObject: [{0}].".Fill(name), e);
            }
        }

        private void Delete(string name)
        {
            var scopedKey  = GetScopedKey(name);
            var storageKey = GetDebugKey(scopedKey);

            if (!storage.HasKey(storageKey))
                return;


            storage.DeleteKey(storageKey);
        }

        private static string GetDebugKey(string key)
        {
            return "_{0}.txt".Fill(key);
        }

        private string Decrypt(string name)
        {
            var scopedKey  = GetScopedKey(name);
            var storageKey = GetDebugKey(scopedKey);

            if (!storage.HasKey(storageKey))
                return null;

            var debugValue = storage.GetString(storageKey);

            return debugValue;
        }

        private string DecryptAsString(string name)
        {
            return Decrypt(name);
        }

        private void Encrypt(string name, string value)
        {
            var scopedKey = GetScopedKey(name);
            storage.SetString(GetDebugKey(scopedKey), value);
        }

        private string GetScopedKey(string name)
        {
            return scope + "." + name;
        }

        private void TryDelete(string name)
        {
            try
            {
                Delete(name);
            }
            catch (Exception e)
            {
                G.LogException("Could not Delete: [{0}].", e);
            }
        }
    }
}
