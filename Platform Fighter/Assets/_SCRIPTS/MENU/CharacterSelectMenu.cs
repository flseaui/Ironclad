using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.CharacterSelectMenu)]
    public class CharacterSelectMenu : Menu
    {
        protected override void SwitchToThis(params string[] args)
        {
            throw new NotImplementedException();
        }

        public void GoBack()
        {
            MenuManager.Instance.SwitchToPreviousMenu();
        }
    }
}