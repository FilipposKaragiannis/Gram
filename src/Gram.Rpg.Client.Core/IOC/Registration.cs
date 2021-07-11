using System;

namespace Gram.Rpg.Client.Core.IOC
{
    public struct Registration
    {
        public Registration(Type type, Func<object> lazyCreator) : this()
        {
            Type        = type;
            LazyCreator = lazyCreator;
        }

        public Func<object> LazyCreator { get; }
        public Type         Type        { get; }
    }
}
