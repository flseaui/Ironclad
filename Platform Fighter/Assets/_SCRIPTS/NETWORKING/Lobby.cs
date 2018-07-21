using Facepunch.Steamworks;
using UnityEngine;

namespace NETWORKING
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private GameObject _serverList, _serverButtonPrefab;

        public void JoinLobby(ulong id)
        {
            Client.Instance.Lobby.Join(id);
            Client.Instance.Lobby.OnLobbyJoined = delegate
            {
                Debug.Log("lobby joined: " + Client.Instance.Lobby.CurrentLobbyData);
            };
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
    }
}