using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;

namespace Gram.Rpg.Client.Presentation.Initialisation
{
    public class Ioc : InitialisableBase
    {
        private readonly Container   container;

        public Ioc(out Container container)
        {

            container = new Container();

            this.container = container;
        }

        protected override void Initialise()
        {
            container.Register(new Application.ContainerRegistrations(Disposer),
                new Client.Infrastructure.ContainerRegistrations(Disposer),
                new Game.ContainerRegistrations(Disposer),
                new ContainerRegistrations(Disposer));

            SL.Initialise(container.Resolver);
        }
    }
}
