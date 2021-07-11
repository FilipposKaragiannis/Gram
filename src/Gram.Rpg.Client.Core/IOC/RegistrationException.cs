using System;

namespace Gram.Rpg.Client.Core.IOC
{
    public class RegistrationException : ApplicationException
    {
        public RegistrationException(string message) : base(message)
        {
        }

        public RegistrationException(string message, Exception e) : base(message, e)
        {
        }
    }
}
