using System;
using System.Linq;
using System.Text;
using Facepunch.Steamworks;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEngine;
using UnityEngine.Serialization;

namespace NETWORKING
{
    [Serializable]
    public class P2PHandlerPacket
    {
        public int FramesLapsed;
    }
    
    public class P2PHandler : SettableSingleton<P2PHandler>
    {
        public P2PHandlerPacket DataPacket;
        
        public int InputPacketsSent;
        public int InputPacketsReceived;
        
        public int Threshold = 0;
        
        private int _playersJoined = 1;

        public bool LatencyCalculated;
        
        private void Start()
        {
            Events.OnInputsChanged += SendP2PInputSet;
            Events.OnMatchJoined += SendP2PMatchJoined;
            Events.OnPingSent += SendP2PPing;
            Events.OnFirstNetworkLatencyCalculated += SendP2PLatency;
            SubscribeToP2PEvents();
            
            Events.OnFinalNetworkLatencyCalculated += i =>
            {
                Debug.Log($"LATENCY: {i}");
                LatencyCalculated = true;
            };
            P2PHelper.Instance.TestLobbyNetworkLatency();
        }

        private void FixedUpdate()
        {
            DataPacket.FramesLapsed = ++DataPacket.FramesLapsed % 600;
            
            if (Threshold > 600)
                Application.Quit();
            else
                ++Threshold;
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

        private void SendP2PPing(ulong steamId, ulong recipientSteamId, int localFrame)
        {
            var body = new P2PPing(localFrame);
            var message = new P2PMessage(steamId, P2PMessageKey.Ping, body.Serialize());
            
            SendP2PMessageToUser(message, recipientSteamId);
        }
        
        private void SendP2PLatency(ulong steamId, ulong recipientSteamId, int localFrame)
        {
            var body = new P2PPing(localFrame);
            var message = new P2PMessage(steamId, P2PMessageKey.Latency, body.Serialize());
            
            SendP2PMessageToUser(message, recipientSteamId);
        }
        
        private void SendP2PMatchJoined(NetworkIdentity networkIdentity)
        {
            var body = new P2PJoin();
            var message = new P2PMessage(networkIdentity.SteamId, P2PMessageKey.Join, body.Serialize());
            
            SendP2PMessage(message);
        }

        private void SendP2PInputSet(NetworkIdentity networkIdentity, P2PInputSet.InputChange[] inputs,
            bool sendNetworkAction)
        {
            if (!sendNetworkAction) return;
            if (!LatencyCalculated) return;

            var body = new P2PInputSet(inputs, InputPacketsSent);
            var message = new P2PMessage(networkIdentity.SteamId, P2PMessageKey.InputSet, body.Serialize());
            
            SendP2PMessage(message);

            InputPacketsSent = ++InputPacketsSent % 600;
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
        }
        
        public void SendP2PMessageToUser(P2PMessage message, ulong steamId)
        {                       
            var serializedMessage = JsonUtility.ToJson(message);
            var data = Encoding.UTF8.GetBytes(serializedMessage);

            Client.Instance.Networking.SendP2PPacket(steamId, data, data.Length, Networking.SendType.Reliable, 0);
        }
        
        public void ParseP2PMessage(ulong senderID, P2PMessage msg)
        {
            var player = MatchStateManager.Instance.GetPlayerBySteamId(msg.SteamId);
            
            switch (msg.Key)
            {
                case P2PMessageKey.InputSet:
                    //if (Threshold > 0)
                    //{
                        var inputSet = JsonUtility.FromJson<P2PInputSet>(msg.Body);

                        //--Threshold;
                        
                        player.GetComponent<NetworkInput>().GiveInputs(inputSet);
                    //}
                    break;
                case P2PMessageKey.Join:
                    var joinMessage = JsonUtility.FromJson<P2PJoin>(msg.Body);
                    
                    ++_playersJoined;                  
                    break;
                case P2PMessageKey.Ping:
                    var pingMessage = JsonUtility.FromJson<P2PPing>(msg.Body);

                    P2PHelper.Instance.ProgressLatencyTest(pingMessage, senderID);                  
                    
                    break;
                case P2PMessageKey.Latency:
                    var latencyMessage = JsonUtility.FromJson<P2PPing>(msg.Body);
                    
                    P2PHelper.Instance.ProgressLatencyTest(latencyMessage, senderID, true);
                    
                    break;

            }
        }

        public void OnInputPacketsReceived()
        {
            InputPacketsReceived = ++InputPacketsReceived % 600;
        }

        public override void SetData(object newData)
        {
            var data = (P2PHandlerPacket) newData;

            DataPacket.FramesLapsed = data.FramesLapsed;
        }
    }
}