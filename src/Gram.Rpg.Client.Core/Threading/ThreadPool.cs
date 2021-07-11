using System.Collections.Generic;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.Threading
{
    public interface IThreadPool
    {
        void Execute(IThreadPoolTask task);
    }


    public class ThreadPool : GObject, IThreadPool
    {
        private readonly Queue<IThreadPoolTask>  completedTasks;
        private readonly Stack<ThreadPoolThread> freeThreads;
        private readonly Queue<IThreadPoolTask>  pendingTasks;
        private readonly object                  syncLock = new object();
        // Holding a reference to all threads to ensure that they aren't garbage collected.
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ThreadPoolThread[] threads;

        public ThreadPool(IWillDisposeYou disposer, IThreadMarshaller marshaller, int numThreads, string name = null) : base(disposer)
        {
            threads        = new ThreadPoolThread[numThreads];
            pendingTasks   = new Queue<IThreadPoolTask>();
            freeThreads    = new Stack<ThreadPoolThread>(numThreads);
            completedTasks = new Queue<IThreadPoolTask>();

            for (var i = 0; i < numThreads; i++)
            {
                var thread = new ThreadPoolThread(this, "TP{0}.{1:00}".Fill(name.IsNullOrWhitespace() ? "" : "_" + name, i), OnThreadComplete);

                threads[i] = thread;

                freeThreads.Push(thread);
            }

            marshaller.PollOnSourceThread(this, OnMarshall);
        }

        public void Execute(IThreadPoolTask task)
        {
            ThreadPoolThread thread;

            lock (syncLock)
            {
                if (freeThreads.Count == 0)
                {
                    pendingTasks.Enqueue(task);
                    return;
                }

                thread = freeThreads.Pop();
            }

            thread.Execute(task);
        }


        private void OnMarshall()
        {
            // (completedTasks.Count == 0) is deliberately not thread safe.
            // We're checking completedTasks.Count on the Unity thread when it could be changed from a worker thread.
            // This is to avoid unnecessariily taking a lock on every Unity frame. A semaphore could also be used but
            // this approach is very cheap - remember it runs every frame regardless of whether there is work to be done.
            // If Count returns an incorrect zero then we'll get the correct value on the next frame. If it returns an
            // incorrect non-zero then we'll take an unnecessary lock on this frame but the returned array will be empty
            // and so no work will be done. 

            if (completedTasks.Count == 0)
                return;


            IThreadPoolTask[] tasks;

            lock (syncLock)
            {
                tasks = completedTasks.ToArray();
                completedTasks.Clear();
            }

            for (var i = 0; i < tasks.Length; i++)
                tasks[i].Complete();
        }

        private void OnThreadComplete(ThreadPoolThread thread, IThreadPoolTask completedTask)
        {
            // REMEMBER: we're on the worker thread.

            IThreadPoolTask pendingTask;

            lock (syncLock)
            {
                completedTasks.Enqueue(completedTask);

                if (pendingTasks.Count == 0)
                {
                    freeThreads.Push(thread);
                    return;
                }

                pendingTask = pendingTasks.Dequeue();
            }

            thread.Execute(pendingTask);
        }
    }
}
