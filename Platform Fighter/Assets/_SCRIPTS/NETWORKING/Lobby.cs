using Facepunch.Steamworks;
using UnityEngine;

namespace NETWORKING
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private GameObject _serverList, _serverButtonPrefab;

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
        }

        public void JoinLobby(ulong id)
        {
            
            Client.Instance.Lobby.Join(Client.Instance.Lobby.CurrentLobby);
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