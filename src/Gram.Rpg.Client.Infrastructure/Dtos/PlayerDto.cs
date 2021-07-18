namespace Gram.Rpg.Client.Infrastructure.Dtos
{
    public class PlayerDto
    {
        public string         Id          { get; set; }
        public HeroDto[]      OwnedHeroes { get; set; }
        public PlayerStatsDto PlayerStats { get; set; }
    }

    public class HeroDto
    {
        public string Id               { get; set; }
        public string Name             { get; set; }
        public int    MaxHealth        { get; set; }
        public int    AttackPower      { get; set; }
        public int    ExperiencePoints { get; set; }
        public int    Level            { get; set; }
    }

    public class PlayerStatsDto
    {
        public BattleHistoryDto[] BattleHistoryEntries { get; set; }
    }

    public class BattleHistoryDto
    {
        public int      MatchResult { get; set; }
        public string[] HeroesUsed  { get; set; }
    }
}
