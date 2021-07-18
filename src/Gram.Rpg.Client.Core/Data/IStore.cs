using System.Collections.Generic;

namespace Gram.Rpg.Client.Core.Data
{
    public interface IStore<TEntry>
    {
        void Set(IEnumerable<TEntry>          source);
        void Set(params IEnumerable<TEntry>[] sources);
    }
}
