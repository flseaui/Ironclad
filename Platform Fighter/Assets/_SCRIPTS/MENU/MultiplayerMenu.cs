using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.MultiplayerMenu)]
    public class MultiplayerMenu : Menu
    {
        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }
        
        public void SwitchToLobbyMenu() => MenuManager.Instance.MenuState = Types.Menu.LobbySelectMenu;
        
        public void SwitchToMainMenu() => MenuManager.Instance.MenuState = Types.Menu.MainMenu;
        
    }
}