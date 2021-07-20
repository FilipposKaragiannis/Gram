using System;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;

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
             void TryRequestSotw()
            {
                try
                {
                    AppRequestsSotw.Execute();
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
