using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.IOC;
using Gram.Rpg.Client.Core.Messaging;
using Gram.Rpg.Client.Presentation.Gui;
using Gram.Rpg.Client.Presentation.Input;

namespace Gram.Rpg.Client.Presentation.Initialisation
{
    public class Gui : InitialisableBase
    {
        [Injected] public IGuiCamera       GuiCamera;
        [Injected] public ICanvas          Canvas;
        [Injected] public IInputDispatcher InputDispatcher;
        [Injected] public IMessageBus      MessageBus;

        protected override void Initialise()
        {
            Canvas.Initialise(GuiCamera);
            
            var strategy    = new GuiRayCreationStrategy();
            var guiDetector = new StandardInputDetector(strategy, GuiCamera.Zdepth, GuiCamera.CullingMask);

            InputDispatcher.AddDetector(Disposer, "GUI", guiDetector);
            
            
            MessageBus.SubscribeTo<UpdateMessage>(m => Instance.Instance.GUpdate(m.DeltaTime));
        }
    }
}
