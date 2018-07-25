using System;
using System.Linq;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LobbyCharacterMenu)]
    public class LobbyCharacterMenu : Menu
    {
        [SerializeField] private PlayerProfilePanel _playerProfilerPanel;
        
        protected override void SwitchToThis()
        {
            Client.Instance.Lobby.OnLobbyMemberDataUpdated = delegate(ulong memberId)
            {
                Debug.Log(Client.Instance.Lobby.GetMemberData(memberId, "name"));
            };
        }

        public void AddPlayer() => _playerProfilerPanel.AddPlayer();
        
        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();

    }
}