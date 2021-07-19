using System;
using System.Collections.Generic;

namespace Gram.Rpg.Client.Core.IOC
{
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

        private void Register(IModule module)
        {
            if (modules.Contains(module))
                throw new InvalidOperationException("Module already registered in container.");
            modules.Add(module);

            resolver.RegisterModule(module);
        }
    }
}
