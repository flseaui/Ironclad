using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    public class MultiplayerMenu : Menu
    {
        public override Types.Menu MenuType => Types.Menu.MULTIPLAYER_MENU;

        protected override void SwitchToMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}