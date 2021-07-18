using Gram.Rpg.Client.Application.UseCases.AppStarts.Commands;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Application.UseCases.AppStarts
{
    public interface IAppStarts
    {
        void Execute(IAppStartsCallbacks callbacks);
    }

    public class AppStarts : GObject, IAppStarts
    {
        private readonly RequestStateOfTheWorld requestSotw;

        public AppStarts(IWillDisposeYou owner) : base(owner)
        {
            requestSotw = new RequestStateOfTheWorld().Inject();
        }
        
        public void Execute(IAppStartsCallbacks callbacks)
        {
            requestSotw.Execute(callbacks);
        }
    }
}
