using System;
using System.Threading;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.Threading
{
    /// <summary>
    /// Thread for use with our ThreadPool. It guarantees that the onComplete event callback will 
    /// be called on the Unity main thread.
    /// </summary>
    internal class ThreadPoolThread : GObject
    {
        private readonly Action<ThreadPoolThread, IThreadPoolTask> onComplete;
        private readonly AutoResetEvent                            resetEvent;
        private volatile bool                                      running;
        private volatile IThreadPoolTask                           task;
        private          Thread                                    thread;


        public ThreadPoolThread(IDisposer disposer, string name, Action<ThreadPoolThread, IThreadPoolTask> onComplete) : base(disposer)
        {
            this.onComplete = onComplete;

            resetEvent = new AutoResetEvent(false);
            running    = true;

            thread = new Thread(DoWork)
                     {
                         Name     = name,
                         Priority = ThreadPriority.AboveNormal
                     };
            thread.Start();
        }

        public void Execute(IThreadPoolTask task)
        {
            if (!running)
                throw new InvalidOperationException("Attempting to execute on a WorkerThread that is not running.");


            this.task = task;

            resetEvent.Set();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            running = false;

            task = null;

            resetEvent.Set();
            resetEvent.Close();

            thread = null;
        }

        private void DoWork()
        {
            while (running)
                try
                {
                    resetEvent.WaitOne();

                    if (task != null)
                    {
                        task.Execute();
                        var temp = task;
                        task = null;
                        onComplete(this, temp);
                    }
                }
                catch (Exception e)
                {
                    if (task == null)
                        G.LogException("Error executing ThreadPoolTask<?>", e);
                    else
                        G.LogException("Error executing ThreadPoolTask<{0}>".Fill(task.GetType()), e);
                }
        }
    }
}
