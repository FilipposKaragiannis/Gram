using System;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.Threading
{
    public class ThreadPoolTask<T> : IThreadPoolTask where T : class
    {
        // note: the type T is constrained to a class so that it can be made volatile.
        // If we require the ThreadPoolTask to have non-class references then we will
        // want to review this constraint, approach and/or whether to have other 
        // implementations of IThreadPoolTask.

        private readonly Func<T>   action;
        private readonly Action<T> onComplete;
        private volatile bool      isComplete;
        private volatile T         result;

        public ThreadPoolTask(Func<T> action, Action<T> onComplete)
        {
            this.action     = action;
            this.onComplete = onComplete;
        }


        // private Stopwatch sw;
        // private long exec;

        public bool IsComplete
        {
            get { return isComplete; }
            private set { isComplete = value; }
        }

        public void Complete()
        {
//            var comp = sw.ElapsedMilliseconds - exec;

//            Y.Log("Task:  {0:N0}   {1:N0}   {2:N0}", exec, comp, sw.ElapsedMilliseconds);

            onComplete(result);
        }

        public void Execute()
        {
            // sw = Stopwatch.StartNew();

            try
            {
                result = action();
                // exec = sw.ElapsedMilliseconds;
            }
            catch (Exception e)
            {
                G.LogException("Error invoking ThreadPoolTask<{0}> action.".Fill(typeof(T)), e);
            }

            IsComplete = true;
        }
    }
}
