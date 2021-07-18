using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.Data
{
    public class Repo<TEntry> : IReadonlyRepo<TEntry>, IStore<TEntry>
    {
        // The intent of this repo is to be generic - no knowledge of the stored data type is necessary.
        // As such, it doesn't have methods like TryGetById, since this requires the data to have an ID
        // property. At the time of writing, that behaviour is provided by the EntityRepo, since every 
        // entity has an ID property.

        protected TEntry[] data;

        [PublicAPI]
        public Repo(TEntry[] data)
        {
            this.data = data;
        }


        public TEntry[] GetAll()
        {
            return data.ToArray();
        }

        public IEnumerable<TEntry> SearchFor(Func<TEntry, bool> predicate)
        {
            return data.Where(predicate);
        }

        public void Set(IEnumerable<TEntry> source)
        {
            data = source.ToArray();
        }

        public void Set(params IEnumerable<TEntry>[] sources)
        {
            var list = new List<TEntry>();

            list.AddRanges(sources);

            Set(list);
        }

        public IEnumerator<TEntry> GetEnumerator() => data.ToList().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
