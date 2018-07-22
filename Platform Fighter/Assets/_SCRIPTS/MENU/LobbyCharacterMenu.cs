using System;
using System.Linq;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LobbyCharacterMenu)]
    public class LobbyCharacterMenu : Menu
    {
        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }
        
        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();

    }
}