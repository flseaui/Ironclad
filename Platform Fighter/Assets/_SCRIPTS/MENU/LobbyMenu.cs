using System;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LOBBY_MENU)]
    public class LobbyMenu : Menu
    {
        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }
        
        public void CreateLobby()
        {
            Client.Instance.Lobby.Create(Facepunch.Steamworks.Lobby.Type.Public, 2);

            Client.Instance.Lobby.OnLobbyCreated = delegate
            {
                Debug.Log("lobby created: " + Client.Instance.Lobby.CurrentLobby);
                Debug.Log($"Owner: {Client.Instance.Lobby.Owner}");
                Debug.Log($"Max Members: {Client.Instance.Lobby.MaxMembers}");
                Debug.Log($"Num Members: {Client.Instance.Lobby.NumMembers}");
            };
            
            MenuManager.Instance.MenuState = Types.Menu.MULTIPLAYER_MENU;
        }

        public void SwitchToGameStartMenu() => MenuManager.Instance.MenuState = Types.Menu.GAME_START;

    }
}