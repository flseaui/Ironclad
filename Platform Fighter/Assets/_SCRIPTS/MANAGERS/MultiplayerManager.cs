using PLAYER;
using UnityEngine;
using UnityEngine.Networking;

namespace MANAGERS
{
    public class MultiplayerManager : NetworkManager
    {
        public override void OnClientConnect(NetworkConnection conn)
        {
            ClientScene.AddPlayer(conn, (short) conn.connectionId);
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            Debug.Log(playerControllerId);
            var player = Instantiate
            (
                playerPrefab, 
                GameManager.Instance.spawnPoints[playerControllerId].position, 
                GameManager.Instance.spawnPoints[playerControllerId].rotation
            );
            //player.GetComponent<PlayerInfo>().color = Color.red;

            AssetManager.PopulateActions(new[] { DATA.Types.Character.TEST_CHARACTER });

            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}