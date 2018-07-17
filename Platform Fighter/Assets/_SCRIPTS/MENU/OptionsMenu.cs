using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    public class OptionsMenu : Menu
    {
        public override Types.Menu MenuType => Types.Menu.OPTIONS_MENU;

        protected override void SwitchToMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}