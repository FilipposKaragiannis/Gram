using System;

namespace Gram.Rpg.Client.Core.Extensions
{
    public static class StringExtensions
    {
        public static string Fill(this string format, params object[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                    return format;

                return string.Format(format, args);
            }
            catch (Exception e)
            {
                G.LogWarning($"Error formatting string: [{format}]\n{e}");
                return format;
            }
        }
    }
}
