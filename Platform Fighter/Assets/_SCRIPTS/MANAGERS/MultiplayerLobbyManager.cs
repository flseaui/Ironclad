using UnityEngine;
using UnityEngine.Networking;

namespace MANAGERS
{
    public class MultiplayerLobbyManager : NetworkLobbyManager
    {
        private void Start()
        {
            StartMatchMaker();
        }

        public override void OnLobbyClientEnter()
        {
            base.OnLobbyClientEnter();
        }

        public override void OnLobbyClientExit()
        {
            base.OnLobbyClientExit();
        }
    }
}