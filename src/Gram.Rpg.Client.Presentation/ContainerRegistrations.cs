using System.Diagnostics.CodeAnalysis;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Core.Messaging;
using Gram.Rpg.Client.Presentation.Gui;
using Gram.Rpg.Client.Presentation.Infrastructure;
using Gram.Rpg.Client.Presentation.Input;

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

        [Singleton]
        private IGuiCamera IGuiCamera()
        {
            return GuiCamera.Create();
        }
        
        [Singleton]
        private ICanvas ICanvas()
        {
            return Canvas.Create();
        }
        
        [Singleton]
        IInput IInput()
        {
            if (UnityEngine.Application.isEditor)
                return new MouseInput();

            return new TouchInput();
        }
        
        [Singleton]
        private IInputDispatcher IInputDispatcher()
        {
            return Instantiate<InputDispatcher>();
        }
    }
}
