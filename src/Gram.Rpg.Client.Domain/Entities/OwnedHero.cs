namespace Gram.Rpg.Client.Domain.Entities
{
    public interface IOwnedHero
    {
        string Id               { get; }
        string Name             { get; }
        int    MaxHealth        { get; }
        int    AttackPower      { get; }
        int    ExperiencePoints { get; }
        int    Level            { get; }
    }

    public class OwnedHero : IOwnedHero
    {
        public OwnedHero(IHero hero)
        {
            Id               = hero.Id;
            Name             = hero.Name;
            AttackPower      = hero.AttackPower;
            MaxHealth        = hero.MaxHealth;
            ExperiencePoints = 0;
            Level            = 0;
        }

        public OwnedHero(string id, string name, int attackPower, int experiencePoints, int level, int maxHealth)
        {
            Id               = id;
            Name             = name;
            AttackPower      = attackPower;
            ExperiencePoints = experiencePoints;
            Level            = level;
            MaxHealth        = maxHealth;
        }

        public string Id               { get; }
        public string Name             { get; }
        public int    MaxHealth        { get; }
        public int    AttackPower      { get; }
        public int    ExperiencePoints { get; }
        public int    Level            { get; }
    }
}
