using System;

namespace Gram.Rpg.Client.Core.IOC
{
    public interface IResolver
    {
        void           BeginScope(IDisposer owner, string name = null);
        ContainerScope BeginScope(string    name);
        T              Get<T>();
        object         Get(Type               serviceType, Type receivingType = null, string memberName = null);
        void           RegisterInstance<T>(T  creator);
        void           RegisterScoped<T>(T    instance);
        void           RegisterSingleton<T>(T instance);
        bool           TryGet<T>(out T        service);
    }
}
