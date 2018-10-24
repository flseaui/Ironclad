using System;
using ATTRIBUTES;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

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
            GameManager.Instance.Characters = new[]
            {
                Types.Character.TestCharacter,
                Types.Character.TestCharacter
            };
            GameManager.Instance.FromSingleplayer = true;
            GameManager.Instance.Stage = Types.Stage.TestStage;
            MenuManager.Instance.MenuState = Types.Menu.GameStartMenu;
        }

        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();
    }
}