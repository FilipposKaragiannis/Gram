using System;
using Gram.Rpg.Client.Core;
using Gram.Rpg.Client.Core.Extensions;

namespace Gram.Rpg.Client.Presentation
{
    public interface IGameMode : IDisposable
    {
        void Initialise();
        void Start();
    }
    
    public static class GameModeFactory
    {
        public static IGameMode Create(IDisposer disposer)
        {
            return new MainGameMode(disposer).Inject();
        }
    }
}
