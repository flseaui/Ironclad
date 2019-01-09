using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.MainMenu)]
    public class MainMenu : Menu
    {
        protected override void SwitchToThis(params string[] args)
        {
            throw new NotImplementedException();
        }

        public void SwitchToSingleplayerMenu()
        {
            MenuManager.Instance.MenuState = Types.Menu.SingleplayerMenu;
        }

        public void SwitchToMultiplayerMenu()
        {
            MenuManager.Instance.MenuState = Types.Menu.MultiplayerMenu;
        }
    }
}