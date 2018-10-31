using System;
using System.Linq;
using System.Text;
using Facepunch.Steamworks;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEngine;

namespace NETWORKING
{
    public class P2PHandler : Singleton<P2PHandler>
    {
        private void Start()
        {
            Events.OnEntityMoved += SendP2PMove;
            Events.OnEntitySpawned += SendP2PSpawn;
            Events.OnInputsChanged += SendP2PInputSet;
            SubscribeToP2PEvents();
        }

        private void SubscribeToP2PEvents()
        {
            Client.Instance.Networking.SetListenChannel(0, true);
            Client.Instance.Networking.OnP2PData += OnP2PData;
            Client.Instance.Networking.OnIncomingConnection += OnIncomingConnection;
            Client.Instance.Networking.OnConnectionFailed += OnConnectionFailed;
        }

        private void OnConnectionFailed(ulong arg1, Networking.SessionError arg2)
        {
            throw new NotImplementedException();
        }

        private bool OnIncomingConnection(ulong arg) => throw new NotImplementedException();

        private void OnP2PData(ulong sender, byte[] bytes, int length, int channel)
        {
            var str = Encoding.UTF8.GetString(bytes, 0, length);
            // deserialize the message
            var serializedMessage = JsonUtility.FromJson<P2PMessage>(str);

            ParseP2PMessage(sender, serializedMessage);
        }

        private void SendP2PMove(NetworkIdentity networkIdentity, Vector2 addedForce, bool sendNetworkAction)
        {
            if (!sendNetworkAction) return;

            var moveBody = new P2PMove(networkIdentity.Id, addedForce);
            var movementMessage = new P2PMessage(networkIdentity.Id, P2PMessageKey.Move, moveBody.Serialize());

            SendP2PMessage(movementMessage);
        }

        private void SendP2PInputSet(NetworkIdentity networkIdentity, P2PInputSet.InputChange[] inputs,
            bool sendNetworkAction)
        {
            if (!sendNetworkAction) return;
            
            var body = new P2PInputSet(networkIdentity.Id, inputs);
            var message = new P2PMessage(networkIdentity.Id, P2PMessageKey.InputSet, body.Serialize());
           
            Debug.Log(message.Body);
            
            SendP2PMessage(message);
        }
        
        private void SendP2PSpawn(NetworkIdentity networkIdentity, bool sendNetworkAction)
        {
            if (!sendNetworkAction) return;

            var spawnBody = new P2PSpawn(networkIdentity.Id);
            var spawningMessage = new P2PMessage(networkIdentity.Id, P2PMessageKey.Spawn, spawnBody.Serialize());

            SendP2PMessage(spawningMessage);
        }
        
        public static void SendP2PMessage(P2PMessage message)
        {
            var serializedMessage = JsonUtility.ToJson(message);

            var data = Encoding.UTF8.GetBytes(serializedMessage);

            foreach (var id in Client.Instance.Lobby.GetMemberIDs().Where(id => id != Client.Instance.SteamId))
                Client.Instance.Networking.SendP2PPacket(id, data, data.Length, Networking.SendType.Unreliable, 0);
        }

        public void ParseP2PMessage(ulong senderID, P2PMessage msg)
        {
            var player = MatchStateManager.Instance.GetPlayer(msg.PlayerId);
            switch (msg.Key)
            {
                case P2PMessageKey.InputSet:
                    var body = JsonUtility.FromJson<P2PInputSet>(msg.Body);

                    var networkInput = player.GetComponent<NetworkInput>();

                    networkInput.ShouldPredictInputs = false;
                    
                    networkInput.ParseInputs(body.Inputs);
                    break;
                case P2PMessageKey.Move:
                    //deserialize the message body
                    var moveBody = JsonUtility.FromJson<P2PMove>(msg.Body);

                    //note the false flag, which instructs the program to NOT also try sending the event again.
                    player.GetComponent<PlayerMovement>().MovePlayer(moveBody.AddedForce, false);
                    break;
                case P2PMessageKey.Spawn:
                    var spawnBody = JsonUtility.FromJson<P2PSpawn>(msg.Body);
                    
                    
                    break;
            }
        }
    }
}