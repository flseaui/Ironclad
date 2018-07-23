using System;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
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

            Client.Instance.Lobby.OnLobbyCreated = success =>
            {
                if (!success) return;

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
                foreach (var lobby in Client.Instance.LobbyList.Lobbies)
                {
                    var button = Instantiate(_serverButtonPrefab, _serverList.transform);
                    button.GetComponentInChildren<Text>().text = lobby.Name;
                    button.name = lobby.Name;
                    Debug.Log($"Found Lobby: {lobby.Name}");
                }
           };
        }

        public void SwitchToMultiplayerMenu() => MenuManager.Instance.MenuState = Types.Menu.MultiplayerMenu;
    }
}