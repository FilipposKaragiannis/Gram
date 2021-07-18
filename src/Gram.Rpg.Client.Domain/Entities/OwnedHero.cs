using Gram.Rpg.Client.Core.Domain.Values;
using Gram.Rpg.Client.Core.Extensions;
using Gram.Rpg.Client.Domain.Entities.Summaries;

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

        IntSummary         AddExperiencePoints(int points);
        HeroLevelUpSummary LevelUp();
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
        public int    MaxHealth        { get; private set; }
        public int    AttackPower      { get; private set; }
        public int    ExperiencePoints { get; private set; }
        public int    Level            { get; private set;}

        public IntSummary AddExperiencePoints(int points)
        {
            var oldXp = ExperiencePoints;
            ExperiencePoints += points;

            return new IntSummary(oldXp, ExperiencePoints);
        }

        public HeroLevelUpSummary LevelUp()
        {
            var oldLevel = Level;
            var oldXp    = ExperiencePoints;
            var oldAttack           = AttackPower;
            var oldHealth           = MaxHealth;
            
            Level++;
            ExperiencePoints =  0;
            AttackPower      += (AttackPower * 0.1f).RoundToInt();
            MaxHealth      += (MaxHealth * 0.1f).RoundToInt();

            return new HeroLevelUpSummary
            {
                LevelSummary       = new IntSummary(oldLevel,  Level),
                AttackPowerSummary = new IntSummary(oldAttack, AttackPower),
                MaxHealthSummary   = new IntSummary(oldHealth, MaxHealth),
                ExperienceSummary = new IntSummary(oldXp, ExperiencePoints)
            };
        }
    }
}
