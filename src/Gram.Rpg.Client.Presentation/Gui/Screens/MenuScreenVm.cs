using Gram.Rpg.Client.Domain.Entities;

namespace Gram.Rpg.Client.Presentation.Gui.Screens
{
    public class MenuScreenVm
    {
        public IPlayer1 Player1 { get; }

        public MenuScreenVm(IPlayer1 player1)
        {
            Player1 = player1;
        }
    }
}
