namespace Gram.Rpg.Client.Core.Threading
{
    public interface IThreadPoolTask
    {
        bool IsComplete { get; }
        void Complete();
        void Execute();
    }
}
