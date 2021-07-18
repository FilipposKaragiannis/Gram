using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Core.Domain.Values;
using Gram.Rpg.Client.Domain.Entities.Summaries;
using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Domain.Entities
{
    public interface IPlayerStats : IEnumerable<IBattleHistoryEntry>
    {
        int TotalWins    { get; }
        int TotalLosses  { get; }
        int TotalBattles { get; }

        PlayerStatsSummary PlayerWon(IEnumerable<string>  heroIds);
        PlayerStatsSummary PlayerLost(IEnumerable<string> heroIds);
    }

    public class PlayerStats : IPlayerStats
    {
        private readonly IList<IBattleHistoryEntry> _historicEntries;

        public PlayerStats(IEnumerable<IBattleHistoryEntry> historicEntries = null)
        {
            _historicEntries = historicEntries?.ToList() ?? new List<IBattleHistoryEntry>();
        }

        public int TotalWins    => _historicEntries.Count(s => s.IsWin);
        public int TotalLosses  => _historicEntries.Count(s => s.IsWin);
        public int TotalBattles => TotalWins + TotalLosses;

        public PlayerStatsSummary PlayerWon(IEnumerable<string> heroIds)
        {
            var oldWins   = TotalWins;
            var oldLosses = TotalLosses;

            var entry = new BattleHistoryEntry(MatchResult.Won, heroIds);

            _historicEntries.Add(entry);

            return new PlayerStatsSummary
            {
                WinsSummary   = new IntSummary(oldWins,   TotalWins),
                LossesSummary = new IntSummary(oldLosses, TotalLosses),
            };
        }

        public PlayerStatsSummary PlayerLost(IEnumerable<string> heroIds)
        {
            var oldWins   = TotalWins;
            var oldLosses = TotalLosses;

            var entry = new BattleHistoryEntry(MatchResult.Lost, heroIds);

            _historicEntries.Add(entry);

            return new PlayerStatsSummary
            {
                WinsSummary   = new IntSummary(oldWins,   TotalWins),
                LossesSummary = new IntSummary(oldLosses, TotalLosses),
            };
        }

        public IEnumerator<IBattleHistoryEntry> GetEnumerator()
        {
            return _historicEntries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
