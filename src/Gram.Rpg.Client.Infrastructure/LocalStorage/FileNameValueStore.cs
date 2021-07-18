using System;
using System.Diagnostics;
using System.Globalization;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;
using Newtonsoft.Json;

namespace Gram.Rpg.Client.Infrastructure.LocalStorage
{
    public class FileNameValueStore : ILocalNameValueStore
    {
        private readonly string       scope;
        private readonly FileStorage  storage;

        public FileNameValueStore( string scope, FileStorage storage)
        {
            this.scope       = scope;
            this.storage     = storage;
        }


        public void DeleteEverything_YesEverthingInEveryScope()
        {
            storage.DeleteEverything();
        }

        public bool GetBool(string name, bool or = false)
        {
            try
            {
                var decrypt = DecryptAsInt(name);

                if (decrypt == null)
                    return or;

                return decrypt == 1;
            }
            catch (Exception e)
            {
                G.LogException("Could not GetBool: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }
        }

        public DateTime GetDateTime(string name, DateTime? or = null)
        {
            return GetNullableDateTime(name) ?? or ?? DateTime.UtcNow;
        }

        public decimal GetDecimal(string name, decimal or = 0)
        {
            try
            {
                return DecryptAsDecimal(name) ?? or;
            }
            catch (Exception e)
            {
                G.LogException("Could not GetDecimal: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }
        }

        public float GetFloat(string name, float or = 0)
        {
            try
            {
                return DecryptAsFloat(name) ?? or;
            }
            catch (Exception e)
            {
                G.LogException("Could not GetFloat: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }
        }

        public int GetInt(string name, int or = 0)
        {
            try
            {
                return DecryptAsInt(name) ?? or;
            }
            catch (Exception e)
            {
                G.LogException("Could not GetInt: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }
        }

        public DateTime? GetNullableDateTime(string name, DateTime? or = null)
        {
            string s;

            try
            {
                s = DecryptAsString(name);
            }
            catch (Exception e)
            {
                G.LogException("Could not GetNullableDateTime: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }

            if (s == null)
                return or;


            try
            {
                var l = long.Parse(s);

                return DateTime.FromBinary(l);
            }
            catch (Exception e)
            {
                G.LogException("Error retrieving DateTime from FileNameValueStore.", e);
                TryDelete(name);
                return or ?? DateTime.UtcNow;
            }
        }

        public int? GetNullableInt(string name, int? or = null)
        {
            try
            {
                return DecryptAsInt(name) ?? or;
            }
            catch (Exception e)
            {
                G.LogException("Could not GetNullableInt: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }
        }

        public T GetObject<T>(string name, T or = default(T), JsonConverter converter = null) where T : class
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
                    "Error deserialising JSON for LocalNameValueStore with key [{0}]. Falling back on the default value.".Fill(GetScopedKey(name)), e);
                TryDelete(name);
                return or;
            }
        }

        public string GetString(string name, string or = null)
        {
            try
            {
                return DecryptAsString(name) ?? or;
            }
            catch (Exception e)
            {
                G.LogException("Could not GetString: [{0}]. Defaulting to: [{1}]".Fill(name, or), e);
                TryDelete(name);
                return or;
            }
        }

        public bool Has(string name)
        {
            try
            {
                var scopedKey  = GetScopedKey(name);
                var storagekey = GetDebugKey(scopedKey);

                return storage.HasKey(storagekey);
            }
            catch (Exception e)
            {
                G.LogException("Could not do Has for: [{0}]. Defaulting to: [false]".Fill(name), e);
                return false;
            }
        }

        public void Remove(string name)
        {
            try
            {
                Delete(name);
            }
            catch (Exception e)
            {
                G.LogException("Could not Remove: [{0}].".Fill(name), e);
            }
        }

        public void SetBool(string name, bool value)
        {
            try
            {
                Encrypt(name, value ? 1 : 0);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetBool: [{0}].".Fill(name), e);
            }
        }

        public void SetDateTime(string name, DateTime value)
        {
            try
            {
                var s = value.ToBinary().ToString();

                Encrypt(name, s);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetDateTime: [{0}].".Fill(name), e);
            }
        }

        public void SetDecimal(string name, decimal value)
        {
            try
            {
                Encrypt(name, value);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetDecimal: [{0}].".Fill(name), e);
            }
        }

        public void SetFlag(string name)
        {
            try
            {
                // We just want to store a key, we don't really care what the value is - it's just a flag.
                SetBool(name, true);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetFlag: [{0}].".Fill(name), e);
            }
        }

        public void SetFloat(string name, float value)
        {
            try
            {
                Encrypt(name, value);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetFloat: [{0}].".Fill(name), e);
            }
        }

        public void SetInt(string name, int value)
        {
            try
            {
                Encrypt(name, value);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetInt: [{0}].".Fill(name), e);
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

        public void SetString(string name, string value)
        {
            try
            {
                Encrypt(name, value);
            }
            catch (Exception e)
            {
                G.LogException("Could not SetString: [{0}].".Fill(name), e);
            }
        }

        public void Delete(string name)
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

        private decimal? DecryptAsDecimal(string name)
        {
            var decrypt = Decrypt(name);

            if (decrypt == null)
                return null;

            return decimal.Parse(decrypt, CultureInfo.InvariantCulture);
        }

        private float? DecryptAsFloat(string name)
        {
            var decrypt = Decrypt(name);

            if (decrypt == null)
                return null;

            return float.Parse(decrypt, CultureInfo.InvariantCulture);
        }

        private int? DecryptAsInt(string name)
        {
            var decrypt = Decrypt(name);

            if (decrypt == null)
                return null;

            return int.Parse(decrypt, CultureInfo.InvariantCulture);
        }

        private string DecryptAsString(string name)
        {
            return Decrypt(name);
        }

        private void Encrypt(string name, decimal value)
        {
            var s = value.ToString(CultureInfo.InvariantCulture);

            Encrypt(name, s);
        }

        private void Encrypt(string name, float value)
        {
            var s = value.ToString(CultureInfo.InvariantCulture);

            Encrypt(name, s);
        }

        private void Encrypt(string name, int value)
        {
            var s = value.ToString(CultureInfo.InvariantCulture);

            Encrypt(name, s);
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
