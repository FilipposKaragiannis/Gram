using System;
using System.Collections.Generic;
using System.Linq;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.IOC
{
    public partial class Container
    {
        private class Scope : GObject
        {
            private readonly IReadOnlyDictionary<Type, Func<object>> creators;
            private readonly Dictionary<Type, object>                instances;


            public Scope(IReadOnlyDictionary<Type, Func<object>> creators, string name = null)
            {
                this.creators = creators;

                instances = new Dictionary<Type, object>();

                Name = name ?? "no_name";
            }

            public string Name { get; }

            public void Register(Type type, object value)
            {
                instances[type] = value;
            }

            public override string ToString()
            {
                return $"{Name} Scope";
            }

            public bool TryResolve(Type type, out object instance)
            {
                if (instances.TryGetValue(type, out instance))
                    return true;

                if (!creators.TryGetValue(type, out var creator))
                    return false;
                
                instance = creator();
                instances.Add(type, instance);

                instance.Inject();
                return true;

            }

            protected override void OnDispose()
            {
                G.Log($"Disposing of scope: {Name}");
                
                foreach (var disposable in instances.Select(s => s.Value as IDisposable))
                    disposable?.Dispose();

                instances.Clear();
            }
        }
    }
}
