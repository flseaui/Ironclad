using System;
using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.SingleplayerMenu)]
    public class SingleplayerMenu : Menu
    {
        protected override void SwitchToThis(params string[] args)
        {
            throw new NotImplementedException();
        }

        public void ReadyButton()
        {
            GameManager.Instance.Characters = new []
                { 
                  Types.Character.TestCharacter, 
                  Types.Character.TestCharacter 
                };
            MenuManager.Instance.MenuState = Types.Menu.GameStartMenu;   
        }
        
        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();
        
    }
}