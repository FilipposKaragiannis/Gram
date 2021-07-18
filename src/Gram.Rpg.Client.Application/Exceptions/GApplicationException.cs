using System;

namespace Gram.Rpg.Client.Application.Exceptions
{
    public class GApplicationException : ApplicationException
    {
        public GApplicationException(string message) : base(message)
        {
        }
    }
}
