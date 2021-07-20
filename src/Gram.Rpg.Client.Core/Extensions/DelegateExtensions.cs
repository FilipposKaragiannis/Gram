using System;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class DelegateExtensions
    {
        public static bool AlreadyHasSubscriber(this Delegate del, Delegate subscriber, string msg, params object[] args)
        {
            var list = del.GetInvocationList();

            for (var i = 0; i < list.Length; i++)
                if (list[i] == subscriber)
                    throw new Exception(msg.Fill(args));
            
            return false;
        }
    }
}
