using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.MULTIPLAYER_MENU)]
    public class MultiplayerMenu : Menu
    {
        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }
        
        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();
        
    }
}