using System;
using System.Collections.Generic;

namespace Gram.Rpg.Client.Core.IOC
{
    // Anything registered in the Container must not use the Service Locator - it must take constructor dependencies.
    // This prevents any hidden registration ordering problems or even circular references that could exist if the SL
    // is used inside the service.


    public partial class Container : IDisposable
    {
        private readonly IList<IModule> modules;
        private readonly ResolverImpl   resolver;

        public Container()
        {
            resolver = new ResolverImpl();

            modules = new List<IModule>();
        }

        public IResolver Resolver => resolver;

        public void Dispose()
        {
            resolver.Dispose();
        }

        public void Register(params IModule[] modules)
        {
            foreach (var m in modules)
                Register(m);
        }

        public Container RegisterSingleton<T>(T instance)
        {
            resolver.RegisterSingleton(instance);

            return this;
        }

        
        private void Register(IModule module)
        {
            if (modules.Contains(module))
#if DEBUG
                throw new InvalidOperationException("Module already registered in container.");
#else
                return;
#endif
            modules.Add(module);

            resolver.RegisterModule(module);
        }
    }
}
