using System;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core
{
    public static class G
    {
        private static Action<string>            logError     = delegate { };
        private static Action<string, Exception> logException = delegate { };
        private static Action<string>            logInfo      = delegate { };
        private static Action<string>            logSafe      = delegate { };
        private static Action<string>            logWarning   = delegate { };


        public static void Configure(Action<string> logInfo,
            Action<string>                          logSafe,
            Action<string>                          logWarning   = null,
            Action<string>                          logError     = null,
            Action<string, Exception>               logException = null)
        {
            G.logInfo      = logInfo;
            G.logSafe      = logSafe;
            G.logWarning   = logWarning   ?? logInfo;
            G.logError     = logError     ?? logInfo;
            G.logException = logException ?? ((msg, e) => G.logError("{0}\n{1}".Fill(msg, e)));
        }

        public static void LogError(object msg)
        {
            try
            {
                if (msg == null)
                {
                    logWarning("null");
                    return;
                }

                logError(msg + "\n");
            }
            catch (Exception e)
            {
                LogErrorWhilstLogging(e);
            }
        }

        public static void LogWarning(object msg)
        {
            try
            {
                if (msg == null)
                {
                    logWarning("null");
                    return;
                }

                logWarning(msg + "\n");
            }
            catch (Exception e)
            {
                LogErrorWhilstLogging(e);
            }
        }

        public static void LogException(string message, Exception e)
        {
            try
            {
                logException(message, e);
            }
            catch (Exception exception)
            {
                LogErrorWhilstLogging(exception);
            }
        }


        public static void Log(object msg)
        {
            if (msg == null)
            {
                logWarning("null");
                return;
            }

            try
            {
                logInfo(msg + "\n");
            }
            catch (Exception e)
            {
                LogErrorWhilstLogging(e);
            }
        }

        private static void LogErrorWhilstLogging(Exception e)
        {
            logSafe("Error whilst logging. " + e);
        }
    }
}
