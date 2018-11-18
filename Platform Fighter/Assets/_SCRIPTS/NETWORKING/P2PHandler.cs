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
        private int _playersJoined = 1;

        public int FramesLapsed;
        
        private void Start()
        {
            Events.OnInputsChanged += SendP2PInputSet;
            Events.OnMatchJoined += SendP2PMatchJoined;
            Events.OnPingSent += SendP2PPing;
            SubscribeToP2PEvents();          
        }

        private void FixedUpdate()
        {
            if (MatchStateManager.Instance.ReadyToFight)
                ++FramesLapsed;           
        }
        
        private void Update()
        {
            if (_playersJoined >= Client.Instance.Lobby.NumMembers)
                MatchStateManager.Instance.ReadyToFight = true;
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

        private void SendP2PPing(NetworkIdentity networkIdentity, int localFrame)
        {
            var body = new P2PPing(localFrame);
            var message = new P2PMessage(networkIdentity.Id, P2PMessageKey.Ping, body.Serialize());
            
            SendP2PMessage(message);
        }
        
        private void SendP2PMatchJoined(NetworkIdentity networkIdentity)
        {
            var body = new P2PJoin();
            var message = new P2PMessage(networkIdentity.Id, P2PMessageKey.Join, body.Serialize());
            
            SendP2PMessage(message);
        }

        private void SendP2PInputSet(NetworkIdentity networkIdentity, P2PInputSet.InputChange[] inputs,
            bool sendNetworkAction)
        {
            if (!sendNetworkAction) return;
            
            var body = new P2PInputSet(inputs);
            var message = new P2PMessage(networkIdentity.Id, P2PMessageKey.InputSet, body.Serialize());
           
            //Debug.Log(message.Body);
            
            SendP2PMessage(message);
        }  
        
        public void SendP2PMessage(P2PMessage message)
        {
            if (Client.Instance.Lobby.NumMembers == 1) return;
            
            var serializedMessage = JsonUtility.ToJson(message);

            var data = Encoding.UTF8.GetBytes(serializedMessage);

            var numClients = 0;
            
            foreach (var id in Client.Instance.Lobby.GetMemberIDs().Where(id => id != Client.Instance.SteamId))
            {
                ++numClients;
                Client.Instance.Networking.SendP2PPacket(id, data, data.Length, Networking.SendType.Reliable, 0);
            }
            //Debug.Log($"Sent {message.Body} on frame { FramesLapsed }");
        }

        public void ParseP2PMessage(ulong senderID, P2PMessage msg)
        {
            var player = MatchStateManager.Instance.GetPlayer(msg.PlayerId);

            //Debug.Log($"Recieved {msg.Body} on frame { FramesLapsed }");
            
            switch (msg.Key)
            {
                case P2PMessageKey.InputSet:
                    var inputSet = JsonUtility.FromJson<P2PInputSet>(msg.Body);
    
                    player.GetComponent<NetworkInput>().GiveInputs(inputSet.Inputs);
                    break;
                case P2PMessageKey.Join:
                    var joinMessage = JsonUtility.FromJson<P2PJoin>(msg.Body);
                    
                    ++_playersJoined;                  
                    break;
                case P2PMessageKey.Ping:
                    var pingMessage = JsonUtility.FromJson<P2PPing>(msg.Body);

                    //Debug.Log($"Sent on frame {pingMessage.LocalFrame}, recieved on frame {FramesLapsed}");
                    
                    Events.OnPingSent(MatchStateManager.Instance.GetPlayers().FirstOrDefault(p => p != player)?.GetComponent<NetworkIdentity>(),
                        FramesLapsed);
                    break;

            }
        }
    }
}