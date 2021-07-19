using Gram.Rpg.Client.Core.IOC;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static T Inject<T>(this T instance)
        {
            return SL.Inject(instance);
        }
    }
}
