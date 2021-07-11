using System;

namespace Gram.Rpg.Client.Core.IOC
{
    public class ServiceResolutionException : ApplicationException
    {
        public ServiceResolutionException(string message) : base(message)
        {
        }
    }
}
