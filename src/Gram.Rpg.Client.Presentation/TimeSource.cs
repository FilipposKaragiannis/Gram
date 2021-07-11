using System;
using System.Collections;
using System.Threading.Tasks;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;
using Gram.Rpg.Client.Core.Threading;
using Gram.Rpg.Client.Presentation.Instance.Components;
using UnityEngine;

namespace Gram.Rpg.Client.Presentation
{
    public interface ITimeSource
    {
    }

    public class TimeSource : GMonoBehaviour, ITimeSource
    {
        private static IEnumerator DoRun<T>(T coroutine, Action<T> oc) where T : IEnumerator
        {
            yield return coroutine;

            oc?.Invoke(coroutine);
        }

        public Task<T> Run<T>(T coroutine) where T : IEnumerator
        {
            var ytcs = new YTaskCompletionSource<T>();

            StartCoroutine(DoRun(coroutine, ytcs.SetResult));

            return ytcs.Task;
        }

        public void WaitForEndOfFrame(Action oc)
        {
            StartCoroutine(DoWaitForEndOfFrame(oc));
        }

        public Task WaitForEndOfFrameAsync(IDisposer disposer)
        {
            var ytcs = new YTaskCompletionSource(disposer);

            WaitForEndOfFrame(ytcs.SetResult);

            return ytcs.Task;
        }

        public void WaitForFrames(int frames, Action oc)
        {
            void Wait()
            {
                if (frames <= 0)
                {
                    oc?.Invoke();
                    return;
                }

                frames--;
                WaitForNextFrame(Wait);
            }

            Wait();
        }

        public Task WaitForFramesAsync(IDisposer disposer, int frames)
        {
            var ytcs = new YTaskCompletionSource(disposer);

            WaitForFrames(frames, ytcs.SetResult);

            return ytcs.Task;
        }

        public void WaitForMilliseconds(int time, Action oc)
        {
            var seconds = time > 0 ? time * 0.001f : 0;
            WaitForSeconds(seconds, oc);
        }

        public Task WaitForMillisecondsAsync(IDisposer disposer, int time)
        {
            var ytcs = new YTaskCompletionSource(disposer);

            WaitForMilliseconds(time, ytcs.SetResult);

            return ytcs.Task;
        }

        public void WaitForNextFrame(Action oc)
        {
            if (IsDisposed)
            {
                oc.Invoke();
                return;
            }

            StartCoroutine(DoWaitForNextFrame(oc));
        }

        public Task WaitForNextFrameAsync(IDisposer disposer)
        {
            var ytcs = new YTaskCompletionSource(disposer);

            WaitForNextFrame(ytcs.SetResult);

            return ytcs.Task;
        }

        public void WaitForSeconds(float time, Action oc)
        {
            if (time.RoughlyEquals(0f))
                oc?.Invoke();
            else
                StartCoroutine(DoWaitForSeconds(time, oc));
        }

        public void WaitForSeconds(IDisposer disposer, float time, Action oc)
        {
            if (time.RoughlyEquals(0f) && !disposer.IsDisposed)
            {
                oc?.Invoke();
                return;
            }


            var enumerator = DoWaitForSeconds(time, oc);

            StartCoroutine(enumerator);

            disposer.Add(() => StopCoroutine(enumerator));
        }

        public Task WaitForSecondsAsync(IDisposer disposer, float time)
        {
            var ytcs = new YTaskCompletionSource(disposer);

            WaitForSeconds(time, ytcs.SetResult);

            return ytcs.Task;
        }

        protected override void OnDispose()
        {
            StopAllCoroutines();

            base.OnDispose();
        }

        private IEnumerator DoWaitForEndOfFrame(Action oc)
        {
            yield return new WaitForEndOfFrame();

            while (instance && instance.Paused)
                yield return new WaitForEndOfFrame();

            oc?.Invoke();
        }

        private IEnumerator DoWaitForNextFrame(Action oc)
        {
            // WaitWhile always waits until the next frame even if the predicate returns true immediately.
            // This approach will wait whilst we are paused AND also wait for the next update in one
            // statement.

            yield return new WaitWhile(() => instance && instance.Paused);

            oc?.Invoke();
        }

        private IEnumerator DoWaitForSeconds(float time, Action oc)
        {
            yield return new YWaitForSeconds(time, () => instance && instance.Paused);

            oc?.Invoke();
        }


        private class YWaitForSeconds : IEnumerator
        {
            private readonly float      duration;
            private readonly Func<bool> isPaused;
            private          float      elapsed;

            public YWaitForSeconds(float time, Func<bool> isPaused = null)
            {
                duration      = time;
                this.isPaused = isPaused ?? (() => false);
            }

            public object Current => null;

            public bool MoveNext()
            {
                if (isPaused())
                    return true;

                var keepGoing = elapsed < duration;

                elapsed += Time.deltaTime;

                return keepGoing;
            }

            public void Reset()
            {
                elapsed = 0;
            }
        }
    }
}
