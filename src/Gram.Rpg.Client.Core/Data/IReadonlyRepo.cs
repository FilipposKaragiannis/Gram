using System;
using System.Collections.Generic;

namespace Gram.Rpg.Client.Core.Data
{
    public interface IReadonlyRepo<TEntry> : IEnumerable<TEntry>
    {
        TEntry[]            GetAll();
        IEnumerable<TEntry> SearchFor(Func<TEntry, bool> predicate);
    }
}
