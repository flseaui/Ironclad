using System;
using ATTRIBUTES;
using Facepunch.Steamworks;
using JetBrains.Annotations;
using MANAGERS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LobbySelectMenu)]
    public class LobbySelectMenu : Menu
    {
        [SerializeField] private GameObject _serverList, _serverButtonPrefab;

        protected override void SwitchToThis(params string[] args)
        {
            throw new NotImplementedException();
        }

        public void CreateLobby()
        {
            MenuManager.Instance.SwitchToMenuWithArgs(Types.Menu.LobbyCharacterMenu, "create");
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
                    button.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        
                        Debug.Log($"Joined Lobby: {lobby.Name}");
                        MenuManager.Instance.SwitchToMenuWithArgs(Types.Menu.LobbyCharacterMenu, "join", lobby.LobbyID.ToString());
                    });
                    Debug.Log($"Found Lobby: {lobby.Name}");
                }
           };
          
        }
        
        public void SwitchToMultiplayerMenu() => MenuManager.Instance.MenuState = Types.Menu.MultiplayerMenu;
    }
}