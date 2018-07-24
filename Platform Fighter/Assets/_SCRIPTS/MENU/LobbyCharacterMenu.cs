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
            Client.Instance.Lobby.OnLobbyMemberDataUpdated = delegate(ulong memberId)
            {
                Debug.Log(Client.Instance.Lobby.GetMemberData(memberId, "name"));
            };
        }
        
        
        
        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();

    }
}