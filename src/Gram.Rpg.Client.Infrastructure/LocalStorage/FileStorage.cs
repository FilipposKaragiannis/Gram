using System;
using System.IO;
using System.Text;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Infrastructure.LocalStorage
{
    public class FileStorage
    {
        private readonly string  path;

        public FileStorage(string path)
        {
            this.path = path;

            Directory.CreateDirectory(path);
        }

        public void DeleteEverything()
        {
            var files = Directory.GetFiles(path);

            foreach (var f in files)
                File.Delete(f);
        }

        public void DeleteKey(string key)
        {
            Log("Del", key);

            File.Delete(Path.Combine(path, key));
        }

        public string GetString(string key)
        {
            var value = File.ReadAllText(Path.Combine(path, key), Encoding.UTF8);

            Log("Get", key, value);

            return value;
        }

        public bool HasKey(string key)
        {
            var hasKey = File.Exists(Path.Combine(path, key));

            Log("Has", key, hasKey);

            return hasKey;
        }

        public void SetString(string key, string value)
        {
            Log("Set", key, value);

            File.WriteAllText(Path.Combine(path, key), value, Encoding.UTF8);
        }


        private void Log(string action, string key, object value = null)
        {
            if (key == null)
                return;
            
            if (!key.StartsWith("_"))
                return;

            try
            {
                key = key.Replace(".txt", "");

                if (value == null)
                    G.Log("{0}: {1,-55}".Fill(action, key));
                else
                    G.Log("{0}: {1,-55} {2}".Fill(action, key, value.ToString().Replace("\n", "\\n")));
            }
            catch (Exception e)
            {
                G.LogException("Error logging FileStorage action.", e);
            }
        }
    }
}
