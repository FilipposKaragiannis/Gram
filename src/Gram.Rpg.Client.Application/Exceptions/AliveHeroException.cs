namespace Gram.Rpg.Client.Application.Exceptions
{
    public class AliveHeroException : GApplicationException
    {
        public AliveHeroException() : base($"No hero should be alive on a loss")
        {
        }
    }
}
