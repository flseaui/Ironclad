using Facepunch.Steamworks;
using UnityEngine;

namespace NETWORKING
{
    public class Lobby : MonoBehaviour
    {
        public void CreateLobby()
        {
            using (var client = new Client(appId: 408))
            {
                client.Lobby.Create(Facepunch.Steamworks.Lobby.Type.Public, 2);

                client.Lobby.OnLobbyCreated = (success) =>
                {
                    Debug.Log("lobby created: " + client.Lobby.CurrentLobby);
                    Debug.Log($"Owner: {client.Lobby.Owner}");
                    Debug.Log($"Max Members: {client.Lobby.MaxMembers}");
                    Debug.Log($"Num Members: {client.Lobby.NumMembers}");

                    client.Lobby.Leave();

                };
            }
        }
    }
}