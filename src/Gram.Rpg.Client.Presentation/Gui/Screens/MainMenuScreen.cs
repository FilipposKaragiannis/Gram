using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UIElements;

namespace Gram.Rpg.Client.Presentation.Gui.Screens
{
    public interface IMainMenuScreen : IScreen
    {
    }


    public class MainMenuScreen : View<MenuScreenPresenter>, IMainMenuScreen, IMainMenuView
    {
        private MenuScreenVm _vm;
        private Label        label;

        public void Open(Action oc)
        {
            oc?.Invoke();
        }

        public static IMainMenuScreen Create(Func<IMainMenuView, MenuScreenPresenter> getPresenter,
            MenuScreenVm                                                              vm)
        {
            var screen = new GameObject("MainMenu").AddComponent<MainMenuScreen>();

            screen.Presenter = getPresenter(screen);
            screen._vm       = vm;

            screen.Build();
            return screen;
        }

        private void Build()
        {
            var playerOb = JsonConvert.SerializeObject(_vm.Player1);
            label = new Label(playerOb);
        }
    }
}
