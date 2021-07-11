using System;
using System.Collections.Generic;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.IOC
{
    public partial class Container
    {
        private class ResolverImpl : IResolver, IDisposable
        {
            private readonly Dictionary<Type, Func<object>> instanceCreators;
            private readonly Dictionary<Type, Func<object>> scopedCreators;
            private readonly Stack<Scope>                   scopes;
            private readonly Dictionary<Type, Func<object>> singletonCreators;
            private readonly Scope                          singletonScope;

            public ResolverImpl()
            {
                instanceCreators  = new Dictionary<Type, Func<object>>();
                scopedCreators    = new Dictionary<Type, Func<object>>();
                singletonCreators = new Dictionary<Type, Func<object>>();

                scopes = new Stack<Scope>();

                singletonScope = BeginScope(singletonCreators, "Singleton");
            }

            public void BeginScope(IDisposer owner, string name = null)
            {
                var result = BeginScope(name);

                owner.Add(result.Dispose);
            }

            public ContainerScope BeginScope(string name = null)
            {
                var scope = BeginScope(scopedCreators, name);

                return new ContainerScope(() =>
                                          {
                                              if (scopes.Count == 0)
                                                  return;
                                              
                                              var peeked = scopes.Peek();

                                              if (peeked != scope)
                                              {
                                                  G.LogWarning($"The peeked scope '{peeked.Name}' is not the expected scope '{scope.Name}'.");
                                                  return;
                                              }

                                              scopes.Pop()
                                                    .Dispose();
                                          });
            }

            public void Dispose()
            {
                var array = scopes.ToArray();

                for (var i = array.Length - 1; i >= 0; i--)
                    array[i].Dispose();

                instanceCreators.Clear();
                scopedCreators.Clear();
                scopes.Clear();
            }

            public T Get<T>()
            {
                var type = typeof(T);

                return (T)Get(type);
            }

            public object Get(Type serviceType, Type receivingType = null, string memberName = null)
            {
                if (TryGet(serviceType, out var service))
                    return service;


                var msg = $"Unable to resolve service: {serviceType.FullName}";

                if (receivingType != null)
                    msg += $" for: {receivingType}";
                if (memberName != null)
                    msg += $" > {memberName}";

                throw new ServiceResolutionException(msg);
            }

            public void RegisterInstance<T>(T creator)
            {
                instanceCreators[typeof(T)] = () => creator;
            }

            public void RegisterModule(IModule module)
            {
                module.Resolver = this;

                // We allow modules registered later to override services registered in earlier modules.

                foreach (var r in module._Instances())
                    try
                    {
                        instanceCreators[r.Type] = r.LazyCreator;
                    }
                    catch (Exception e)
                    {
                        throw new RegistrationException($"Error registering instance: {e}");
                    }


                foreach (var r in module._Singletons())
                    try
                    {
                        singletonCreators[r.Type] = r.LazyCreator;
                    }
                    catch (Exception e)
                    {
                        throw new RegistrationException($"Error registering singleton instance: {e}");
                    }


                foreach (var r in module._Scopeds())
                    try
                    {
                        scopedCreators[r.Type] = r.LazyCreator;
                    }
                    catch (Exception e)
                    {
                        throw new RegistrationException($"Error registering scoped instance: {e}");
                    }
            }

            public void RegisterScoped<T>(T instance)
            {
                CurrentScope.Register(typeof(T), instance);
            }

            public void RegisterSingleton<T>(T instance)
            {
                singletonScope.Register(typeof(T), instance);
            }

            public bool TryGet(Type serviceType, out object service)
            {
                if (instanceCreators.TryGetValue(serviceType, out var creator))
                {
                    service = creator().Inject();
                    return true;
                }

                if (CurrentScope.TryResolve(serviceType, out var scopedInstance))
                {
                    service = scopedInstance;
                    return true;
                }


                if (singletonScope != CurrentScope)
                    if (singletonScope.TryResolve(serviceType, out var singletonInstance))
                    {
                        service = singletonInstance;
                        return true;
                    }

                service = null;
                return false;
            }

            public bool TryGet<T>(out T service)
            {
                var type = typeof(T);

                var success = TryGet(type, out var svc);

                service = (T)svc;
                return success;
            }


            private Scope CurrentScope => scopes.Peek();

            private Scope BeginScope(IReadOnlyDictionary<Type, Func<object>> creators, string name)
            {
                var scope = new Scope(creators, name);

                scopes.Push(scope);

                return scope;
            }
        }
    }
}
