using System;
using System.Threading.Tasks;

namespace Gram.Rpg.Client.Core.Threading
{
    public static class TaskExtensions
    {
        public static void SurfaceErrors(this Task task)
        {
            task.ContinueWith(t =>
                              {
                                  if (t.Exception != null)
                                  {
                                      G.LogException("Unhandled error from Task.", t.Exception);
                                      throw t.Exception;
                                  }
                              }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        
        public static ICatchContinuation Cancelled(this Task task, Action action)
        {
            var continuation = new TaskExtensionContinuation();

            void AlreadyCompleted(Task t)
            {
                if (t.IsFaulted)
                    continuation.TaskErrored(t.Exception);
                else
                    continuation.TaskSuccessful();

                continuation.Cancelled(action);
            }

            void Continuation(Task t)
            {
                continuation.Cancelled(action);
                
                if (t.IsFaulted)
                    continuation.InvokeCatch(t.Exception);
                else if (t.IsCanceled)
                    continuation.InvokeCancelled();

                continuation.InvokeFinally();
            }

            if (task.IsCompleted)
                AlreadyCompleted(task);
            else
                task.ContinueWith(Continuation,
                                  TaskScheduler.FromCurrentSynchronizationContext());

            continuation.LogUncaughtErrors();
            
            return continuation;
        }
        
        public static IFinallyContinuation Catch(this Task task, Action<Exception> action)
        {
            var continuation = new TaskExtensionContinuation();

            void AlreadyCompleted(Task t)
            {
                if (t.IsFaulted)
                    continuation.TaskErrored(t.Exception);
                else
                    continuation.TaskSuccessful();

                continuation.Catch(action);
            }

            void Continuation(Task t)
            {
                continuation.Catch(action);

                if (t.IsFaulted)
                    continuation.InvokeCatch(t.Exception);

                continuation.InvokeFinally();
            }

            if (task.IsCompleted)
                AlreadyCompleted(task);
            else
                task.ContinueWith(Continuation,
                                  TaskScheduler.FromCurrentSynchronizationContext());

            return continuation;
        }

        public static void Finally(this Task task, Action action)
        {
            var continuation = new TaskExtensionContinuation();

            continuation.Finally(action);

            if (task.IsCompleted)
                continuation.InvokeFinally();
            else
                task.ContinueWith(t => continuation.InvokeFinally(),
                                  TaskScheduler.FromCurrentSynchronizationContext());
        }

        public static void Initialise(Action<Action> waitForNextTick)
        {
            TaskExtensionContinuation.WaitForNextTick = waitForNextTick;
        }

        public static ICatchContinuation Then(this Task task, Action action)
        {
            var continuation = new TaskExtensionContinuation();

            void AlreadyCompleted(Task t)
            {
                if (t.IsFaulted)
                    continuation.TaskErrored(t.Exception);
                else if (t.IsCanceled)
                    continuation.TaskCancelled();
                else
                    continuation.TaskSuccessful(action);
            }

            void Continuation(Task t)
            {
                if (t.IsFaulted)
                    continuation.InvokeCatch(t.Exception);
                else if (t.IsCanceled)
                    continuation.InvokeCancelled();
                else
                    continuation.InvokeThen(action);

                continuation.InvokeFinally();
            }

            if (task.IsCompleted)
                AlreadyCompleted(task);
            else
                task.ContinueWith(Continuation,
                                  TaskScheduler.FromCurrentSynchronizationContext());

            continuation.LogUncaughtErrors();

            return continuation;
        }

        public static ICatchContinuation Then<T>(this Task<T> task, Action<T> action)
        {
            var continuation = new TaskExtensionContinuation();

            void AlreadyCompleted(Task<T> t)
            {
                if (t.IsFaulted)
                    continuation.TaskErrored(t.Exception);
                else if (t.IsCanceled)
                    continuation.TaskCancelled();
                else
                    continuation.TaskSuccessful(() => action?.Invoke(t.Result));
            }

            void Continuation(Task<T> t)
            {
                if (t.IsFaulted)
                    continuation.InvokeCatch(t.Exception);
                else if (t.IsCanceled)
                    continuation.InvokeCancelled();
                else
                    continuation.InvokeThen(() => action?.Invoke(t.Result));

                continuation.InvokeFinally();
            }

            if (task.IsCompleted)
                AlreadyCompleted(task);
            else
                task.ContinueWith(Continuation,
                                  TaskScheduler.FromCurrentSynchronizationContext());

            return continuation;
        }


        private class TaskExtensionContinuation : ICancelledContinuation, ICatchContinuation, IFinallyContinuation
        {
            internal static Action<Action>    WaitForNextTick;
            private         bool              alreadyCompleted;
            private         Action            cancelledAction;
            private         Action<Exception> catchAction;
            private         bool              catchingErrors;
            private         Action            finallyAction;
            private         Exception         rethrowError;
            private         bool              taskCancelled;
            private         Exception         taskError;

            public ICancelledContinuation Cancelled(Action action)
            {
                cancelledAction = action;
                
                if (taskCancelled)
                    InvokeCancelled();

                return this;
            }

            public IFinallyContinuation Catch(Action<Exception> action)
            {
                catchingErrors = true;

                catchAction = action;

                if (taskError != null)
                    InvokeCatch(taskError);

                else if (rethrowError != null)
                    InvokeCatch(rethrowError);

                return this;
            }

            public void Finally(Action action)
            {
                finallyAction = action;

                if (alreadyCompleted)
                    InvokeFinally();
            }
            
            public void InvokeCancelled()
            {
                alreadyCompleted = true;

                try
                {
                    cancelledAction?.Invoke();
                }
                catch (Exception e)
                {
                    rethrowError = e;
                }
            }

            public void InvokeCatch(Exception exception)
            {
                alreadyCompleted = true;
                
                if (catchAction == null)
                {
                    rethrowError = taskError = exception;
                    return;
                }

                try
                {
                    catchAction(exception);
                    rethrowError = taskError = null;
                }
                catch (Exception e)
                {
                    rethrowError = e;
                }
            }

            public void InvokeFinally()
            {
                alreadyCompleted = true;

                try
                {
                    finallyAction?.Invoke();
                }
                catch (Exception e)
                {
                    rethrowError = rethrowError ?? e;
                }

                if (rethrowError == null)
                    return;

                G.LogWarning($"Unhandled exception!: {rethrowError}");
                rethrowError = null;
            }

            public void InvokeThen(Action action)
            {
                alreadyCompleted = true;

                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    InvokeCatch(e);
                }
            }

            public void LogUncaughtErrors()
            {
                WaitForNextTick?.Invoke(TryLoggingErrors);

                void TryLoggingErrors()
                {
                    if (!catchingErrors && (taskError != null || rethrowError != null))
                        G.LogException("Unhandled error from Task!", taskError ?? rethrowError);
                }
            }

            public void TaskCancelled()
            {
                alreadyCompleted = taskCancelled = true;
            }
            
            public void TaskErrored(Exception exception)
            {
                alreadyCompleted = true;

                taskError = exception;
            }

            public void TaskSuccessful(Action action = null)
            {
                alreadyCompleted = true;

                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    rethrowError = e;
                }
            }
        }
    }


    public interface ICancelledContinuation
    {
        IFinallyContinuation Catch(Action<Exception> action);
        void                 Finally(Action          action);
    }


    public interface ICatchContinuation
    {
        ICancelledContinuation Cancelled(Action action);
        IFinallyContinuation Catch(Action<Exception> action);
        void                 Finally(Action          action);
    }


    public interface IFinallyContinuation
    {
        void Finally(Action action);
    }
}
