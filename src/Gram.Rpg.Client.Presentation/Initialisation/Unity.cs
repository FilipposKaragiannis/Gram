using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Messaging;
using UnityEngine;
using UApplication = UnityEngine.Application;

namespace Gram.Rpg.Client.Presentation.Initialisation
{
    public class Unity : InitialisableBase
    {
        protected override void Initialise()
        {
            Time.fixedDeltaTime               = 0.01f;
            FixedUpdateMessage.FixedDeltaTime = 0.01f;

            QualitySettings.vSyncCount = 1;
            UApplication.targetFrameRate = 60;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}
