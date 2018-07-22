using System;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LobbySelectMenu)]
    public class LobbySelectMenu : Menu
    {
        [SerializeField] private GameObject _serverList, _serverButtonPrefab;

        protected override void SwitchToThis()
        {
            throw new NotImplementedException();
        }

        public void CreateLobby()
        {
            Client.Instance.Lobby.Create(Lobby.Type.Public, 2);

            Client.Instance.Lobby.OnLobbyCreated = delegate
            {
                Debug.Log("lobby created: " + Client.Instance.Lobby.CurrentLobby);
                Debug.Log($"Owner: {Client.Instance.Lobby.Owner}");
                Debug.Log($"Max Members: {Client.Instance.Lobby.MaxMembers}");
                Debug.Log($"Num Members: {Client.Instance.Lobby.NumMembers}");
            };

            MenuManager.Instance.MenuState = Types.Menu.LobbyCharacterMenu;
        }

        public void FindLobbies()
        {
            Client.Instance.LobbyList.Refresh();

            //wait for the callback
            foreach (Transform child in _serverList.transform)
                Destroy(child.gameObject);

            Client.Instance.LobbyList.OnLobbiesUpdated = delegate
            {
                if (!Client.Instance.LobbyList.Finished) return;

                foreach (var lobby in Client.Instance.LobbyList.Lobbies)
                {
                    Instantiate(_serverButtonPrefab, _serverList.transform);
                    Debug.Log($"Found Lobby: {lobby.Name}");
                }
            };
        }

        public void SwitchToMultiplayerMenu() => MenuManager.Instance.MenuState = Types.Menu.MultiplayerMenu;
    }
}