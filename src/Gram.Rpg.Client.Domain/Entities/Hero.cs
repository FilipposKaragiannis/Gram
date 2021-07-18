using Gram.Rpg.Client.Core.Domain;

namespace Gram.Rpg.Client.Domain.Entities
{
    public interface IHero : IEntity<string>
    {
        string Name        { get; }
        int    AttackPower { get; }
        int    MaxHealth   { get; }
    }

    public class Hero : Entity<string>, IHero
    {
        public Hero(string id, string name, int attackPower, int maxHealth) : base(id)
        {
            Name        = name;
            AttackPower = attackPower;
            MaxHealth   = maxHealth;
        }

        public string Name        { get; }
        public int    AttackPower { get; }
        public int    MaxHealth   { get; }
    }
}
