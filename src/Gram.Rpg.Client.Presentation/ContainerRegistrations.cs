using System.Diagnostics.CodeAnalysis;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Core.Messaging;
using Gram.Rpg.Client.Presentation.Infrastructure;

namespace Gram.Rpg.Client.Presentation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ContainerRegistrations : Module
    {
        public ContainerRegistrations(IWillDisposeYou disposer) : base(disposer)
        {
        }

        [Singleton]
        private IMessageBus IMessageBus() => new MessageBus(new DedicatedEventHandlers());
    }
}
