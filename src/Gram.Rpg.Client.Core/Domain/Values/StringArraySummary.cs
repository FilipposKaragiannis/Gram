using System.Linq;

namespace Gram.Rpg.Client.Core.Domain.Values
{
    public interface IStringArraySummary : ISummary
    {
        string[] Delta     { get; }
        bool     HasChange { get; }
        string[] New       { get; }
        string[] Old       { get; }
    }


    public class StringArraySummary : IStringArraySummary
    {
        public StringArraySummary(string[] old)
        {
            Old = old;
        }

        public bool HasChange => Delta != null && Delta.Any();

        public string[] Delta
        {
            get
            {
                if (New == null)
                    return new string[0];

                if (Old == null)
                    return New;

                return New.Except(Old).ToArray();
            }
        }

        public string[] New { get; set; }
        public string[] Old { get; }
    }
}
