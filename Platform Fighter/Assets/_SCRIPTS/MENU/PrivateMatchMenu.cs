using System;
using ATTRIBUTES;
using Facepunch.Steamworks;
using MANAGERS;
using UnityEngine;
using Types = DATA.Types;

namespace MENU
{
    [MenuType(Types.Menu.PrivateMatchMenu)]
    public class PrivateMatchMenu : Menu
    {
        protected override void SwitchToThis(params string[] args)
        {
            throw new NotImplementedException();
        }

        public void InviteFriend()
        {
            Client.Instance.Lobby.OpenFriendInviteOverlay();

            Client.Instance.Lobby.OnLobbyJoined = delegate(bool successfullyJoined)
            {
                if (successfullyJoined)
                    Debug.Log("Player Joined");
            };

            /*foreach ( var friend in Client.Instance.Friends.All.Where( x => x.IsOnline ) )
            {
               NLog.Log(LogType.Message,  $"{friend.Id}: {friend.Name}" );
            }*/
        }

        public void GoBack() => MenuManager.Instance.SwitchToPreviousMenu();
    }
}