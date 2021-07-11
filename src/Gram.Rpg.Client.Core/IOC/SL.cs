using System;
using System.Linq;
using System.Reflection;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.IOC
{
    public static class SL
    {
        private static IResolver resolver;

        public static void BeginScope(IDisposer owner, string name)
        {
            resolver.BeginScope(owner, name);
        }

        public static ContainerScope BeginScope(string name)
        {
            return resolver.BeginScope(name);
        }

        // [Obsolete("Not really obsolete, but flagging whilst we remove unnecessary uses of it. SL.Get should be used in rare cases, prefer DI instead.")]
        public static T Get<T>()
        {
            if (resolver == null)
                throw new InvalidOperationException("Can not Get service [{0}]; Resolver is null. This can happen if the service you are getting makes a call to SL (or similar). You should review your service retrieval.".Fill(typeof(T).FullName));

            return resolver.Get<T>();
        }

        // [Obsolete("Not really obsolete, but flagging whilst we remove unnecessary uses of it. SL.Get should be used in rare cases, prefer DI instead.")]
        public static bool TryGet<T>(out T service)
        {
            service = default;

            if (resolver == null)
                return false;
            
            return resolver.TryGet(out service);
        }

        // [Obsolete("Not really obsolete, but flagging whilst we remove unnecessary uses of it. SL.Get should be used in rare cases, prefer DI instead.")]
        public static object Get(Type type)
        {
            if (resolver == null)
                throw new InvalidOperationException("Can not Get service [{0}]; Resolver is null. This can happen if the service you are getting makes a call to SL (or similar). You should review your service retrieval.".Fill(type.FullName));

            return resolver.Get(type);
        }

        public static void Initialise(IResolver resolver)
        {
            SL.resolver = resolver;
        }

        public static T Inject<T>(T instance)
        {
            if (resolver == null)
                throw new InvalidOperationException("Can not configure object of type [{0}]; Resolver is null. This can happen if the service you are getting makes a call to SL (or similar). You should review your service retrieval.".Fill(typeof(T).FullName));

            var type = instance.GetType();

            try
            {
                type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(m => m.GetCustomAttributes(typeof(InjectedAttribute), false).Any())
                    .ForEach(m =>
                             {
                                 var f = m as FieldInfo;
                                 if (f != null)
                                 {
                                     var svc = resolver.Get(f.FieldType, type, f.Name);
                                     f.SetValue(instance, svc);
                                     return;
                                 }

                                 var p = m as PropertyInfo;
                                 if (p != null)
                                     throw new InvalidOperationException("Properties are not supported for injection. This is because IL2CPP does not recognise them (Unity 5.3.4p5) and errors. Use fields instead.");

                                 throw new InvalidOperationException("Unsupported member type for injecting dependencies.");
                             });
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("An error occurred whilst configuring an instance of type [{0}]".Fill(type), e);
            }

            return instance;
        }

        public static void RegisterInstance<T>(T instance)
        {
            resolver.RegisterInstance(instance);
        }

        public static void RegisterScoped<T>(T instance)
        {
            resolver.RegisterScoped(instance);
        }

        public static void RegisterSingleton<T>(T instance)
        {
            resolver.RegisterSingleton(instance);
        }

        public static bool Initialised => resolver != null;
    }
}
