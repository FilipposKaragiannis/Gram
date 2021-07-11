using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Core.Messaging;
using Gram.Rpg.Client.Presentation.Initialisation;
using JetBrains.Annotations;
using GUI = Gram.Rpg.Client.Presentation.Initialisation.Gui;
using UNITY = Gram.Rpg.Client.Presentation.Initialisation.Unity;

namespace Gram.Rpg.Client.Presentation
{
    [UsedImplicitly]
    public abstract partial class EntryPoint
    {
        private Container  container;
        private Disposer   disposer;
        private IGameMode  gameMode;
        private TimeSource globalTimeSource;

        [Injected] public IMessageBus MessageBus;


        private void OnDisposed()
        {
            container?.Dispose();
        }

        private void Initialise()
        {
            void InitialiseSystems(params IInitialisable[] systems)
            {
                foreach (var s in systems)
                {
                    if (SL.Initialised)
                        s.Inject();

                    s.Initialise(disposer);
                }
            }

            InitialiseSystems(new Logging(),
                new Ioc(globalTimeSource, out container),
                new UNITY(),
                new GeneralTypes(),
                new GUI());

            this.Inject();
        }

        private void OnInitialised()
        {
            gameMode = GameModeFactory.Create(disposer);

            gameMode.Initialise();

            gameMode.Start();
        }
    }
}
