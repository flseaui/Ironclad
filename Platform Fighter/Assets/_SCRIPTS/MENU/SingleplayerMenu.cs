﻿using ATTRIBUTES;
using DATA;
using MANAGERS;

namespace MENU
{
    [MenuType(Types.Menu.SingleplayerMenu)]
    public class SingleplayerMenu : Menu
    {
        protected override void SwitchToThis(params string[] args)
        {
            GameManager.Instance.MatchType = Types.MatchType.OfflineSingleplayer;
        }

        public void ReadyButton()
        {
            GameManager.Instance.Characters = new[]
            {
                Types.Character.TestCharacter,
                Types.Character.TestCharacter
            };
            GameManager.Instance.Stage = Types.Stage.TestStage;
            MenuManager.Instance.MenuState = Types.Menu.GameStartMenu;
        }

        public void GoBack()
        {
            MenuManager.Instance.SwitchToPreviousMenu();
        }
    }
}