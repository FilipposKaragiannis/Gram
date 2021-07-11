using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;

namespace Gram.Rpg.Client.Presentation.Initialisation
{
    public class Ioc : InitialisableBase
    {
        private readonly Container   container;
        private readonly ITimeSource globalTimeSource;

        public Ioc(ITimeSource timeSource, out Container container)
        {
            globalTimeSource = timeSource;

            container = new Container();

            this.container = container;
        }

        protected override void Initialise()
        {
            container.RegisterSingleton(globalTimeSource);

            container.Register(new Application.ContainerRegistrations(Disposer),
                new Client.Infrastructure.ContainerRegistrations(Disposer),
                new Game.ContainerRegistrations(Disposer),
                new ContainerRegistrations(Disposer));

            SL.Initialise(container.Resolver);
        }
    }
}
