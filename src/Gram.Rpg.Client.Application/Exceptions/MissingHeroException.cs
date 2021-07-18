namespace Gram.Rpg.Client.Application.Exceptions
{
    public class MissingHeroException : GApplicationException
    {
        public MissingHeroException(string heroId) : base($"Hero with id {heroId} was missing from player inventory")
        {
        }
    }
}
