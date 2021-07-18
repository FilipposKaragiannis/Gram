using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Domain.Entities
{
    public interface IPlayerStats : IEnumerable<IBattleHistoryEntry>
    {
        int TotalWins   { get; }
        int TotalLosses { get; }

        void Record(MatchResult result, IEnumerable<string> heroesUsed);
    }

    public class PlayerStats : IPlayerStats
    {
        private readonly IList<IBattleHistoryEntry> _historicEntries;

        public PlayerStats(IEnumerable<IBattleHistoryEntry> historicEntries = null)
        {
            _historicEntries = historicEntries?.ToList() ?? new List<IBattleHistoryEntry>();
        }

        public int TotalWins   => _historicEntries.Count(s => s.IsWin);
        public int TotalLosses => _historicEntries.Count(s => s.IsWin);

        public void Record(MatchResult result, IEnumerable<string> heroesUsed)
        {
            _historicEntries.Add(new BattleHistoryEntry(result, heroesUsed));
        }

        public IEnumerator<IBattleHistoryEntry> GetEnumerator() => _historicEntries.GetEnumerator();
        IEnumerator IEnumerable.                GetEnumerator() => GetEnumerator();
    }

    public interface IBattleHistoryEntry
    {
        bool                IsWin       { get; }
        bool                IsLoss      { get; }
        MatchResult         MatchResult { get; }
        IEnumerable<string> HeroesUsed  { get; }
    }

    public class BattleHistoryEntry : IBattleHistoryEntry
    {
        public BattleHistoryEntry(MatchResult matchResult, IEnumerable<string> heroesUsed)
        {
            MatchResult = matchResult;
            HeroesUsed  = heroesUsed;
        }

        public bool                IsWin       => MatchResult == MatchResult.Won;
        public bool                IsLoss      => MatchResult == MatchResult.Lost;
        public              MatchResult         MatchResult { get; }
        public              IEnumerable<string> HeroesUsed  { get; }
    }
}
