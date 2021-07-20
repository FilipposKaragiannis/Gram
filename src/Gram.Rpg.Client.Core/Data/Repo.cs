using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gram.Rpg.Client.Core.Data
{
    public class Repo<TEntry> : IEnumerable<TEntry>
    {
        protected TEntry[] data;

        protected Repo(TEntry[] data)
        {
            this.data = data;
        }

        public IEnumerator<TEntry> GetEnumerator() => data.ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
