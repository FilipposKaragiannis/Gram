using System.Collections.Generic;
using Gram.Rpg.Client.Domain.Values;

namespace Gram.Rpg.Client.Domain.Entities
{
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
        public MatchResult         MatchResult { get; }
        public IEnumerable<string> HeroesUsed  { get; }
    }
}
