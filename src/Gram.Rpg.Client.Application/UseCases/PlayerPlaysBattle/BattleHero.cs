namespace Gram.Rpg.Client.Application.UseCases.PlayerPlaysBattle
{
    public class BattleHero
    {
        public BattleHero(string id, int remainingHealth)
        {
            Id              = id;
            RemainingHealth = remainingHealth;
        }

        public string Id              { get; }
        public int    RemainingHealth { get; }
    }
}
