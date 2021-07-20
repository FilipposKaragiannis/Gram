using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gram.Rpg.Client.Core.Design;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.IOC
{
    public interface IModule
    {
        IResolver Resolver { set; }
        IEnumerable<Registration> _Instances();
        IEnumerable<Registration> _Singletons();
        IEnumerable<Registration> _Scopeds();
    }


    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class InstanceAttribute : Attribute
    {
    }


    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class ScopedAttribute : Attribute
    {
    }
    
    
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse]
    public class SingletonAttribute : Attribute
    {
    }


    public abstract class Module : IModule
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected readonly IWillDisposeYou                Disposer;
        private            Dictionary<Type, Func<object>> instanceRegistrations;

        protected Module(IWillDisposeYou disposer)
        {
            Disposer = disposer;
        }

        public IResolver Resolver { get; set; }

        public virtual IEnumerable<Registration> _Instances()
        {
            return GetMethodsWithAttribute<InstanceAttribute>();
        }

        public virtual IEnumerable<Registration> _Scopeds()
        {
            return GetMethodsWithAttribute<ScopedAttribute>();
        }

        public virtual IEnumerable<Registration> _Singletons()
        {
            return GetMethodsWithAttribute<SingletonAttribute>();
        }

        public object CreateInstance(Type t)
        {
            if (instanceRegistrations == null)
            {
                instanceRegistrations = new Dictionary<Type, Func<object>>();

                foreach (var reg in _Instances())
                    instanceRegistrations.Add(reg.Type, reg.LazyCreator);
            }


            if (instanceRegistrations.TryGetValue(t, out var func))
                return func();

            return null;
        }

        protected T Instantiate<T>()
        {
            var type = typeof(T);
            
            var ci = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                              .FirstOrDefault();

            if (ci == null)
                throw new InvalidOperationException($"Could not find constructor for type: [{type}]");


            var args = ci.GetParameters()
                         .Select(pi => pi.ParameterType == typeof(IWillDisposeYou) 
                                           ? Disposer 
                                           : Resolver.Get(pi.ParameterType, type, pi.Name))
                         .ToArray();

            return (T)ci.Invoke(args);
        }


        private IEnumerable<Registration> GetMethodsWithAttribute<T>() where T : Attribute
        {
            var dic = new Dictionary<string, Registration>();

            var type = GetType();
            
            while (type != null)
            {
                type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(mi => Attribute.GetCustomAttributes(mi, typeof(T), true).Length == 1)
                    .ForEach(mi =>
                             {
                                 var key = $"{mi.Name}({string.Join(", ", mi.GetParameters().Select(p => p.ParameterType.FullName))})";

                                 if (!dic.ContainsKey(key))
                                     dic.Add(key, new Registration(mi.ReturnType, () => mi.Invoke(this, null)));
                             });

                type = type.BaseType;
            }

            return dic.Values;
        }
    }
}
