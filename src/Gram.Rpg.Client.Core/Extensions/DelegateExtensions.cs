using System;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core.Extensions
{
    [PublicAPI]
    public static class DelegateExtensions
    {
        [StringFormatMethod("format")]
        public static bool AlreadyHasSubscriber(this Delegate del, Delegate subscriber, string msg, params object[] args)
        {
            var list = del.GetInvocationList();

            for (var i = 0; i < list.Length; i++)
                if (list[i] == subscriber)
                    throw new Exception(msg.Fill(args));
            
            return false;
        }

        public static void InvokeSuppressingErrors(this Action action)
        {
            if (action == null)
                return;

            try
            {
                action();
            }
            catch (Exception e)
            {
                G.LogException("Error whilst invoking Action. Suppressing.", e);
            }
        }

        public static void InvokeSuppressingErrors<T>(this Action<T> action, T arg1)
        {
            if (action == null)
                return;

            try
            {
                action(arg1);
            }
            catch (Exception e)
            {
                G.LogException("Error whilst invoking Action<{0}>. Suppressing.".Fill(typeof(T).Name), e);
            }
        }

        public static void InvokeSuppressingErrors<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action == null)
                return;

            try
            {
                action(arg1, arg2);
            }
            catch (Exception e)
            {
                G.LogException("Error whilst invoking Action<{0}, {1}>. Suppressing."
                    .Fill(typeof(T1).Name, typeof(T2).Name), e);
            }
        }
    }
}
