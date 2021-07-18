namespace Gram.Rpg.Client.Application.Exceptions
{
    public class NoAliveHeroesException : GApplicationException
    {
        public NoAliveHeroesException() : base($"No alive heroes was found in the win battle result")
        {
        }
    }
}
