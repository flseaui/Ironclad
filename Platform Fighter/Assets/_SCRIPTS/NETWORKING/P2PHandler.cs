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

        public void Initialize()
        {
            FrameCounter = 0;
        }
    }

    public class P2PHandler : SettableSingleton<P2PHandler>
    {
        public P2PHandlerPacket DataPacket;

        [NonSerialized] public int InputPacketsReceived;
        [NonSerialized] public int InputPacketsSent;
        [NonSerialized] public int Ping;
        
        [NonSerialized] public bool ReceivedFirstInput;
        [NonSerialized] public bool GameStarted;
        [NonSerialized] public bool AllPlayersReady;

        private int _playersJoined = 1;
        private int _previousDelay;
        private int _lastPingTime = -1;     
        private int _remoteFrameLag;
        private int _localFrameLag;
        private int _framesToStall;
        private int _frameStallTimer;
        
        private bool _started;
        private bool _initialSave;

        public int Delay;
        
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
            if (!AllPlayersReady || !GameStarted)
                return;

            if (_framesToStall > 0)
            {
                --_frameStallTimer;
                if (_frameStallTimer == 0)
                {
                    StallFrame();
                }
            }

            // calculate averaged frame advantage 
            if (TimeManager.Instance.FramesLapsed % 100 == 0)
            {
                var inputFrameAdvantage = _localFrameLag / 100f - _remoteFrameLag / 100f;
                DispersedInputAdvantagePause(inputFrameAdvantage);
                _localFrameLag = 0;
                _remoteFrameLag = 0;
            }
            
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
            InvokeRepeating(nameof(SendPing), 0, 1);
        }
        
        private void DispersedInputAdvantagePause(float inputFrameAdvantage)
        {
            var simulationFrameAdvantage = .5f * inputFrameAdvantage;

            _framesToStall = 0;
            if (simulationFrameAdvantage >= .75f)
            {
                _framesToStall += Math.Max(1, (int)(simulationFrameAdvantage + .5f));
            }

            _frameStallTimer = CalculateStallCadence(_framesToStall);
        }

        private void StallFrame()
        {
            Debug.Log("STALLLLLLLLLLLLL!!!!!!!!!!!!! on " + DataPacket.FrameCounter);
            TimeManager.Instance.PauseForFrames(1, Types.PauseType.FixedUpdate);

            _frameStallTimer = CalculateStallCadence(_framesToStall);
        }

        private int CalculateStallCadence(int framesToStall)
        {
            switch (framesToStall)
            {
                case 1: return 10;
                case 2: return 9;
                case 3: return 8;
                case 4: return 7;
                case 5: return 6;
                case 6: return 5;
                case 7: return 4;
                case 8: return 3;
                case 9: return 2;
                default: return 1;
            }
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

            var body = new P2PInputSet(inputs, angle, InputPacketsSent);
            var message = new P2PMessage(networkIdentity.SteamId, P2PMessageKey.InputSet, body.Serialize());

            SendP2PMessage(message);

            _localFrameLag = InputPacketsReceived - InputPacketsSent + Delay;

            InputPacketsSent = ++InputPacketsSent;
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

                    InputPacketsReceived = ++InputPacketsReceived;

                    ReceivedFirstInput = true;
                    
                    _remoteFrameLag = InputPacketsSent - InputPacketsReceived + Delay;
                    
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

                    var ping = pingMessage.SentTime - _lastPingTime - 1000;
                    
                    _lastPingTime = pingMessage.SentTime;
                    
                    Ping = ping;
                    Delay = CalcInputLagFrames(Delay, ping / 100);
                    Debug.Log("delay: " + Delay);
                    Events.OnPingCalculated?.Invoke(ping, senderID);
                    break;
            }
        }

        private static int CalcInputLagFrames(int curInputLagFrames, int worstPeerFrameLag)
        {
            var newInputLagFrames = curInputLagFrames;

            // increase lag
            if (curInputLagFrames <= 0 && worstPeerFrameLag >= 2.0f) newInputLagFrames = 1;
            if (curInputLagFrames <= 1 && worstPeerFrameLag >= 3.0f) newInputLagFrames = 2;
            if (curInputLagFrames <= 2 && worstPeerFrameLag >= 6.0f) newInputLagFrames = 3;
            if (curInputLagFrames <= 3 && worstPeerFrameLag >= 8.0f) newInputLagFrames = 4;

            // reduce lag
            if (curInputLagFrames >= 4 && worstPeerFrameLag <= 7.0f) newInputLagFrames = 3;
            if (curInputLagFrames >= 3 && worstPeerFrameLag <= 5.0f) newInputLagFrames = 2;
            if (curInputLagFrames >= 2 && worstPeerFrameLag <= 2.0f) newInputLagFrames = 1;
            if (curInputLagFrames >= 1 && worstPeerFrameLag <= 1.0f) newInputLagFrames = 0;

            return newInputLagFrames;
        }
        
        public void IncrementFrameCounter()
        {
            if (!_initialSave)
            {
                RollbackManager.Instance.SaveGameState(0, true);
                _initialSave = true;
            }
            
            DataPacket.FrameCounter += 1 + (_previousDelay - Delay);
        }
        
        public override void SetData(object newData)
        {
            var data = (P2PHandlerPacket) newData;

            DataPacket.FrameCounter = data.FrameCounter;
        }
    }
}