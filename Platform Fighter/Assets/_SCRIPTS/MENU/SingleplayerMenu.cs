using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    public class SingleplayerMenu : Menu
    {
        public override Types.Menu MenuType => Types.Menu.SINGLEPLAYER_MENU;

        protected override void SwitchToMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}