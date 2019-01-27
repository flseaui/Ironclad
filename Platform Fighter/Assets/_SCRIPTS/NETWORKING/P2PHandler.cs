using System;
using System.Linq;
using System.Text;
using Facepunch.Steamworks;
using MANAGERS;
using MISC;
using PLAYER;
using UnityEngine;
using Types = DATA.Types;

namespace NETWORKING
{
    [Serializable]
    public class P2PHandlerPacket
    {
        public int FrameCounter = 0;
        public int FrameCounterLoops = 0;

        public void Initialize()
        {
            FrameCounter = 0;
            FrameCounterLoops = 0;
        }
    }

    public class P2PHandler : SettableSingleton<P2PHandler>
    {
        private int _playersJoined = 1;

        private int _previousDelay;

        private bool _started;
        private bool _initialSave;
        
        public P2PHandlerPacket DataPacket;

        public int Delay => 2;//Ping / 100;
        
        [NonSerialized]
        public int InputPacketsProcessed;
        [NonSerialized]
        public int InputPacketsReceived;
        [NonSerialized]
        public int InputPacketsSent;
        [NonSerialized]
        public int InputPacketsSentLoops;

        [NonSerialized]
        public bool ReceivedFirstInput;
        
        [NonSerialized] 
        public bool GameStarted;
        
        [NonSerialized]
        public int Ping;

        [NonSerialized]
        public bool AllPlayersReady;

        private int _lastPingTime = -1;
        
        protected override void OnAwake()
        {
            DataPacket.Initialize();
            _previousDelay = Delay;
        }
        
        private void Start()
        {
            Events.OnPingSent += SendP2PPing;
            Events.OnInputsChanged += SendP2PInputSet;
            Events.OnGameStarted += SendP2PGameStart;
            SubscribeToP2PEvents();
        }
        
        public void StartGame()
        {
            GameStarted = true;
        }
        
        private void FixedUpdate()
        {
            if (TimeManager.Instance.FixedUpdatePaused)
                return;
            if (!AllPlayersReady)
                return;

            IncrementFrameCounter();

            _previousDelay = Delay;
        }
        
        private void SubscribeToP2PEvents()
        {
            Client.Instance.Networking.SetListenChannel(0, true);
            Client.Instance.Networking.OnP2PData += OnP2PData;
            Client.Instance.Networking.OnIncomingConnection += OnIncomingConnection;
            Client.Instance.Networking.OnConnectionFailed += OnConnectionFailed;
        }

        public void BeginTesting()
        {
            InvokeRepeating(nameof(SendPing), 0, 2);
        }
        
        public void SendPing()
        {
            Events.OnPingSent?.Invoke((int) (Time.unscaledTime * 1000));
        }
        
        private void OnConnectionFailed(ulong steamid, Networking.SessionError error)
        {
            Debug.Log( "Connection Error: " + steamid + " - " + error );
        }
  
        private bool OnIncomingConnection(ulong steamid)
        {
            Debug.Log( "Incoming P2P Connection: " + steamid );
            return true;
        }

        private void OnP2PData(ulong sender, byte[] bytes, int length, int channel)
        {
            var str = Encoding.UTF8.GetString(bytes, 0, length);
            // deserialize the message
            var serializedMessage = JsonUtility.FromJson<P2PMessage>(str);

            ParseP2PMessage(sender, serializedMessage);
        }

        private void SendP2PPing(int sentTime)
        {
            var body = new P2PPing(sentTime);
            var message = new P2PMessage(Client.Instance.SteamId, P2PMessageKey.Ping, body.Serialize());

            SendP2PMessage(message);
        }
        
        private void SendP2PGameStart(NetworkIdentity networkIdentity)
        {
            var body = new P2PGameStart();
            var message = new P2PMessage(networkIdentity.SteamId, P2PMessageKey.GameStart, body.Serialize());

            SendP2PMessage(message);
        }

        private void SendP2PInputSet(NetworkIdentity networkIdentity, InputChange[] inputs,
            Vector2 angle, bool sendNetworkAction)
        {
            if (!sendNetworkAction) return;

            var body = new P2PInputSet(inputs, angle, InputPacketsSent, InputPacketsSentLoops);
            var message = new P2PMessage(networkIdentity.SteamId, P2PMessageKey.InputSet, body.Serialize());

            SendP2PMessage(message);

            if (InputPacketsSent == 599)
                InputPacketsSentLoops = ++InputPacketsSentLoops % 600;
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
            switch (msg.Key)
            {
                case P2PMessageKey.InputSet:
                    if (!AllPlayersReady) return;
                    
                    var player = MatchStateManager.Instance.GetPlayerBySteamId(msg.SteamId);
                    var inputSet = JsonUtility.FromJson<P2PInputSet>(msg.Body);

                    InputPacketsReceived = ++InputPacketsReceived % 600;

                    ReceivedFirstInput = true;

                    Debug.Log("sender: " + senderID);
                    player.GetComponent<NetworkInput>().GiveInputs(inputSet);
                    break;
                case P2PMessageKey.GameStart:

                    ++_playersJoined;
                    if (_playersJoined >= Client.Instance.Lobby.NumMembers)
                        AllPlayersReady = true;
                    break;
                case P2PMessageKey.Ping:
                    var pingMessage = JsonUtility.FromJson<P2PPing>(msg.Body);

                    if (_lastPingTime == -1)
                    {
                        _lastPingTime = pingMessage.SentTime;
                        return;
                    }

                    var ping = pingMessage.SentTime - _lastPingTime - 2000;
                    
                    _lastPingTime = pingMessage.SentTime;
                    
                    Ping = ping;
                    Debug.Log("delay: " + Delay);
                    Events.OnPingCalculated?.Invoke(ping, senderID);
                    break;
            }
        }

        public void OnInputPacketsProcessed()
        {
            InputPacketsProcessed = ++InputPacketsProcessed % 600;
        }

        public void IncrementFrameCounter()
        {
            if (!_initialSave)
            {
                RollbackManager.Instance.SaveGameState(0);
            }
            
            var prevFrameCount = DataPacket.FrameCounter;
            DataPacket.FrameCounter += 1 + (_previousDelay - Delay);
            DataPacket.FrameCounter %= 600;
            if (DataPacket.FrameCounter < prevFrameCount)
                DataPacket.FrameCounterLoops = ++DataPacket.FrameCounterLoops % 600;
            
            if (!_initialSave)
            {
                _initialSave = true;
                DataPacket.FrameCounterLoops = 0;
            }
        }
        
        public override void SetData(object newData)
        {
            var data = (P2PHandlerPacket) newData;

            DataPacket.FrameCounter = data.FrameCounter;
            DataPacket.FrameCounterLoops = data.FrameCounterLoops;
        }
    }
}