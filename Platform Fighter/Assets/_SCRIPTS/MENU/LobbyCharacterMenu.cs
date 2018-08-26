using System;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using TOOLS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.LobbyCharacterMenu)]
    public class LobbyCharacterMenu : Menu
    {
        [SerializeField] private PlayerProfilePanel _playerProfilerPanel;

        private int _playerReady;

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
                else if (args[0] == "join") Client.Instance.Lobby.Join(ulong.Parse(args[1]));
            }
        }

        private void OnCreated(bool success)
        {
            if (!success) return;

            _playerProfilerPanel.ClearPlayerProfiles();
            _playerProfilerPanel.AddPlayerProfile(Client.Instance.SteamId);

            NLog.Log(NLog.LogType.Message, "lobby created: " + Client.Instance.Lobby.CurrentLobby);
            NLog.Log(NLog.LogType.Message, $"Owner: {Client.Instance.Lobby.Owner}");
            NLog.Log(NLog.LogType.Message, $"Max Members: {Client.Instance.Lobby.MaxMembers}");
            NLog.Log(NLog.LogType.Message, $"Num Members: {Client.Instance.Lobby.NumMembers}");
        }

        private void OnJoined(bool success)
        {
            NLog.Log(NLog.LogType.Message, "OnLobbyJoined");
            if (!success) return;
        }

        private void OnDataUpdated()
        {
            NLog.Log(NLog.LogType.Message, "OnLobbyDataUpdated");
            _playerProfilerPanel.ClearPlayerProfiles();
            foreach (var member in Client.Instance.Lobby.GetMemberIDs()) _playerProfilerPanel.AddPlayerProfile(member);
        }

        private void OnMemberDataUpdated(ulong member)
        {
            NLog.Log(NLog.LogType.Message, "OnLobbyMemberDataUpdated");
            if (Client.Instance.Lobby.GetMemberData(member, "ready").Equals("true"))
            {
                _playerProfilerPanel.ReadyPlayerProfile(member);
                ++_playerReady;
                if (_playerReady >= Client.Instance.Lobby.NumMembers)
                    MenuManager.Instance.MenuState = Types.Menu.GameStartMenu;
            }
        }

        private void OnChatMessage(ulong d, byte[] c, int s)
        {
            NLog.Log(NLog.LogType.Message, "OnChatMessage");
        }

        private void OnStateChange(Lobby.MemberStateChange change, ulong initiator, ulong affectee)
        {
            NLog.Log(NLog.LogType.Message, "OnLobbyStateChanged");
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
            Client.Instance.Lobby.SetMemberData("ready", "true");
            //Client.Instance.Lobby.OnLobbyMemberDataUpdated(Client.Instance.SteamId);
            NLog.Log(NLog.LogType.Message, "wedy 2 pway");
        }

        public void GoBack()
        {
            _playerProfilerPanel.ClearPlayerProfiles();
            Client.Instance.Lobby.Leave();
            MenuManager.Instance.SwitchToPreviousMenu();
        }
    }
}