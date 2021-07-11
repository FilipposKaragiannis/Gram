using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Core.Threading
{
    public class YTaskCompletionSource
    {
        private readonly Func<bool>                   disposerStillExists;
        private readonly TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
        private readonly bool                         withoutDisposer;

        public static implicit operator Task(YTaskCompletionSource tcs)
        {
            return tcs.Task;
        }

        public YTaskCompletionSource()
        {
            withoutDisposer     = true;
            disposerStillExists = () => false;
        }

        public YTaskCompletionSource(IDisposer disposer)
        {
            withoutDisposer     = disposer == null;
            disposerStillExists = () => disposer != null && !disposer.IsDisposed;
        }
        
        public YTaskCompletionSource(params IDisposer[] disposer)
        {
            withoutDisposer     = disposer == null || disposer.IsEmpty();
            disposerStillExists = () => disposer != null && !disposer.Any(d => d.IsDisposed);
        }

        public Task Task => tcs.Task;

        public TaskAwaiter<object> GetAwaiter()
        {
            return tcs.Task.GetAwaiter();
        }

        public void SetCanceled()
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetCanceled();
        }

        public void SetException(Exception exception)
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetException(exception);
        }

        public void SetException(IEnumerable<Exception> exceptions)
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetException(exceptions);
        }

        public void SetResult()
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetResult(null);
        }

        public void SetResult(Task task)
        {
            if ((withoutDisposer || disposerStillExists()) && task.IsCompleted)
                tcs.SetResult(null);
        }

        public bool TrySetCanceled()
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetCanceled();

            return false;
        }

        public bool TrySetCanceled(CancellationToken cancellationToken)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetCanceled(cancellationToken);

            return false;
        }

        public bool TrySetException(Exception exception)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetException(exception);

            return false;
        }

        public bool TrySetException(IEnumerable<Exception> exceptions)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetException(exceptions);

            return false;
        }

        public bool TrySetResult()
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetResult(null);

            return false;
        }
    }


    public class YTaskCompletionSource<T>
    {
        private readonly Func<bool>              disposerStillExists;
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
        private readonly bool                    withoutDisposer;

        public static implicit operator Task<T>(YTaskCompletionSource<T> tcs)
        {
            return tcs.Task;
        }
        
        public YTaskCompletionSource(params IDisposer[] disposer)
        {
            withoutDisposer     = disposer == null || disposer.IsEmpty();
            disposerStillExists = () => disposer != null && !disposer.Any(d => d.IsDisposed);
        }

        public Task<T> Task => tcs.Task;

        public TaskAwaiter<T> GetAwaiter()
        {
            return tcs.Task.GetAwaiter();
        }
        
        public void SetCanceled()
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetCanceled();
        }

        public void SetException(Exception exception)
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetException(exception);
        }

        public void SetException(IEnumerable<Exception> exceptions)
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetException(exceptions);
        }

        public void SetResult(T result)
        {
            if (withoutDisposer || disposerStillExists())
                tcs.SetResult(result);
        }

        public void SetResult(Task<T> task)
        {
            if ((withoutDisposer || disposerStillExists()) && task.IsCompleted)
                tcs.SetResult(task.Result);
        }

        public bool TrySetCanceled()
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetCanceled();

            return false;
        }

        public bool TrySetCanceled(CancellationToken cancellationToken)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetCanceled(cancellationToken);

            return false;
        }

        public bool TrySetException(Exception exception)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetException(exception);

            return false;
        }

        public bool TrySetException(IEnumerable<Exception> exceptions)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetException(exceptions);

            return false;
        }

        public bool TrySetResult(T result)
        {
            if (withoutDisposer || disposerStillExists())
                return tcs.TrySetResult(result);

            return false;
        }
    }
}
