using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    public class LobbyMenu : Menu
    {
        public override Types.Menu MenuType => Types.Menu.LOBBY_MENU;

        protected override void SwitchToMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}