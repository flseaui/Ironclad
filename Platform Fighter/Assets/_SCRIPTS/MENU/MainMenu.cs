using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    public class MainMenu : Menu
    {
        public override Types.Menu MenuType => Types.Menu.MAIN_MENU;

        protected override void SwitchToMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}