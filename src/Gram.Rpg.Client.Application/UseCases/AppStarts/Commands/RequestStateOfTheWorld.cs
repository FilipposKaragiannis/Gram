using System;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Core.Threading;

namespace Gram.Rpg.Client.Application.UseCases.AppStarts.Commands
{
    public interface IAppStartsCallbacks
    {
        void AppStartsUp_UnexpectedError(Action retry);
    }

    public class RequestStateOfTheWorld
    {
        [Injected] public IAppRequestsStateOfTheWorld AppRequestsSotw;

        public void Execute(IAppStartsCallbacks callbacks)
        {
            var tcs = new YTaskCompletionSource();

             void TryRequestSotw()
            {
                try
                {
                    AppRequestsSotw.Execute();
                    tcs.SetResult();
                }
                catch (Exception e)
                {
                    G.LogException("Unexpected error whilst requesting SOTW.", e);
                    callbacks?.AppStartsUp_UnexpectedError(TryRequestSotw);
                }
            }

            TryRequestSotw();
        }
    }
}
