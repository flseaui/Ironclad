using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.MAIN_MENU)]
    public class MainMenu : Menu
    {
        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }

        public void SwitchToSingleplayerMenu() => MenuManager.Instance.MenuState = Types.Menu.SINGLEPLAYER_MENU;

        public void SwitchToMultiplayerMenu() => MenuManager.Instance.MenuState = Types.Menu.MULTIPLAYER_MENU;
    }
}