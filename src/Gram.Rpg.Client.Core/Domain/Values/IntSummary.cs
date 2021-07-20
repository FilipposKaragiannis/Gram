namespace Gram.Rpg.Client.Core.Domain.Values
{
    public interface IIntSummary : ISummary
    {
        int Delta { get; }
        int New   { get; }
        int Old   { get; }
    }


    public class IntSummary : IIntSummary
    {
        public IntSummary(int old, int? @new = null)
        {
            Old = old;

            if (@new.HasValue)
                New = @new.Value;
        }

        public bool HasChange => Delta != 0;

        public int Delta => New - Old;
        public int New   { get; }
        public int Old   { get; }
    }
}
