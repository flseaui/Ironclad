using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.SINGLEPLAYER_MENU)]
    public class SingleplayerMenu : Menu
    {
        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }

        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();

    }
}