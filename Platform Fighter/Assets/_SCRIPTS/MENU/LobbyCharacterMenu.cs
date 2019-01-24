using System;
using System.Collections.Generic;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using MISC;
using NETWORKING;
using TMPro;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LobbyCharacterMenu)]
    public class LobbyCharacterMenu : Menu
    {
        [SerializeField] private PlayerProfilePanel _playerProfilerPanel;
        
        [SerializeField] private int _playerReady;

        protected override void SwitchToThis(params string[] args)
        {
            Client.Instance.Lobby.OnLobbyCreated = OnCreated;
            Client.Instance.Lobby.OnLobbyJoined = OnJoined;
            Client.Instance.Lobby.OnLobbyDataUpdated = OnDataUpdated;
            Client.Instance.Lobby.OnLobbyMemberDataUpdated = OnMemberDataUpdated;
            Client.Instance.Lobby.OnLobbyStateChanged = OnStateChange;
            Client.Instance.Lobby.OnChatMessageRecieved = OnChatMessage;

            if (args.Length > 0)
            {
                if (args[0] == "create")
                    Client.Instance.Lobby.Create(Lobby.Type.Public, 2);
                else if (args[0] == "join")
                    Client.Instance.Lobby.Join(ulong.Parse(args[1]));
            }

            GameManager.Instance.MatchType = Types.MatchType.OnlineMultiplayer;
            Events.OnPingCalculated += _playerProfilerPanel.SetPlayerProfilePing;
        }
        
        private void OnCreated(bool success)
        {
            if (!success) return;

            SetupMemberData();

            _playerProfilerPanel.ClearPlayerProfiles();
            _playerProfilerPanel.AddPlayerProfile(Client.Instance.SteamId);

            LatencyTester.Instance.BeginTesting();
            
            /*Debug.Log("lobby created: " + Client.Instance.Lobby.CurrentLobby);
            Debug.Log($"Owner: {Client.Instance.Lobby.Owner}");
            Debug.Log($"Max Members: {Client.Instance.Lobby.MaxMembers}");
            Debug.Log($"Num Members: {Client.Instance.Lobby.NumMembers}");*/
        }

        private void OnJoined(bool success)
        {
            if (!success) return;

            SetupMemberData();

            foreach (var member in Client.Instance.Lobby.GetMemberIDs())
            {
                _playerProfilerPanel.AddPlayerProfile(member);

                if (member != Client.Instance.SteamId) CheckMemberData(member, false);
            }
            
            LatencyTester.Instance.BeginTesting();
        }

        private void SetupMemberData()
        {
            Client.Instance.Lobby.SetMemberData("character", "testCharacter");
            Client.Instance.Lobby.SetMemberData("lobbySpot", (Client.Instance.Lobby.NumMembers - 1).ToString());
            Client.Instance.Lobby.SetMemberData("ready", "false");
        }

        private void OnDataUpdated()
        {
            //_playerProfilerPanel.ClearPlayerProfiles();
            //foreach (var member in Client.Instance.Lobby.GetMemberIDs()) _playerProfilerPanel.AddPlayerProfile(member);
        }

        private void OnMemberDataUpdated(ulong steamId)
        {
            CheckMemberData(steamId, true);
        }

        public void CheckMemberData(ulong steamId, bool update)
        {
            _playerReady = 0;
            foreach (var member in Client.Instance.Lobby.GetMemberIDs())
                if (Client.Instance.Lobby.GetMemberData(member, "ready") == "true")
                    _playerReady++;

            switch (Client.Instance.Lobby.GetMemberData(steamId, "ready"))
            {
                case "true":
                    _playerProfilerPanel.ReadyPlayerProfile(steamId);
                    if (_playerReady >= Client.Instance.Lobby.NumMembers && Client.Instance.Lobby.NumMembers > 1 &&
                        update)
                    {
                        var tempCharacterArray = new List<Types.Character>();

                        GameManager.Instance.SteamIds = Client.Instance.Lobby.GetMemberIDs();

                        foreach (var id in GameManager.Instance.SteamIds)
                            tempCharacterArray.Add(
                                CharacterStringToId(Client.Instance.Lobby.GetMemberData(id, "character")));

                        GameManager.Instance.Characters = tempCharacterArray.ToArray();

                        MenuManager.Instance.MenuState = Types.Menu.GameStartMenu;
                    }

                    break;
                case "false":
                    if (!update) return;

                    _playerProfilerPanel.UnreadyPlayerProfile(steamId);

                    break;
            }
        }

        private void OnChatMessage(ulong d, byte[] c, int s)
        {
            Debug.Log("OnChatMessage");
        }

        private void OnStateChange(Lobby.MemberStateChange change, ulong initiator, ulong affectee)
        {
            switch (change)
            {
                case Lobby.MemberStateChange.Entered:
                    _playerProfilerPanel.AddPlayerProfile(initiator);
                    break;
                case Lobby.MemberStateChange.Disconnected:
                    _playerProfilerPanel.RemovePlayerProfile(initiator);
                    break;
                case Lobby.MemberStateChange.Left:
                    _playerProfilerPanel.RemovePlayerProfile(initiator);
                    break;
                case Lobby.MemberStateChange.Kicked:
                    break;
                case Lobby.MemberStateChange.Banned:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(change), change, null);
            }
        }

        public void ReadyToPlay()
        {
            if (Client.Instance.Lobby.GetMemberData(Client.Instance.SteamId, "ready") != "true")
                Client.Instance.Lobby.SetMemberData("ready", "true");
            else
                Client.Instance.Lobby.SetMemberData("ready", "false");

            _playerReady = 0;
            foreach (var member in Client.Instance.Lobby.GetMemberIDs())
                if (Client.Instance.Lobby.GetMemberData(member, "ready") == "true")
                    _playerReady++;
        }

        public Types.Character CharacterStringToId(string character)
        {
            switch (character)
            {
                case "testCharacter":
                    return Types.Character.TestCharacter;
                default:
                    return Types.Character.None;
            }
        }

        public void OnCharacterChanged(int character)
        {
            Client.Instance.Lobby.SetMemberData
            (
                "character",
                character
                    .Map(0, "testCharacter")
            );
        }

        public void GoBack()
        {
            _playerProfilerPanel.ClearPlayerProfiles();
            Client.Instance.Lobby.Leave();
            MenuManager.Instance.SwitchToPreviousMenu();
        }
    }
}