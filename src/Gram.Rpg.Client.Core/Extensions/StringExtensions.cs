using System;
using System.Linq;
using System.Text.RegularExpressions;
using Gram.Rpg.Client.Core.Design;

namespace Gram.Rpg.Client.Core.Extensions
{
    [PublicAPI]
    public static class StringExtensions
    {
        public static string CamelSplitter(this string s)
        {
            var step1 = Regex.Replace(s, @"([A-Z]|[0-9]+)", " $1");
            var step2 = step1.Replace("_", " ");
            var step3 = Regex.Replace(step2, @"(\s{2,})", " ");

            return step3.TrimStart(' ');
        }

        public static bool EqualsAny(this string s, params string[] others)
        {
            return s == null ? others.Any(o => o == null) : others.Any(s.Equals);
        }

        [StringFormatMethod("format")]
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

        [ContractAnnotation("s:null => true")]
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        [ContractAnnotation("s:null => true")]
        public static bool IsNullOrWhitespace(this string s)
        {
            return s == null || s.All(char.IsWhiteSpace);
        }
    }
}
