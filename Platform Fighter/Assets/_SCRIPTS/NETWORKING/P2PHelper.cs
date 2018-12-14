using System;
using Facepunch.Steamworks;
using MISC;

namespace NETWORKING
{
    public class P2PHelper : Singleton<P2PHelper>
    {
        public int NetworkLatency { get; set; }
        
        private int _sentFrame;
        private int _hostSentFrame;
        
        // should only be called while in a lobby
        public void TestLobbyNetworkLatency()
        {
            if (Client.Instance.SteamId != Client.Instance.Lobby.Owner)
            {
                Events.OnPingSent
                (
                    Client.Instance.SteamId,
                    Client.Instance.Lobby.Owner,
                    P2PHandler.Instance.FramesLapsed
                );
                _sentFrame = P2PHandler.Instance.FramesLapsed;
            }
        }

        // DO NOT CALL, INTERNAL ONLY
        public void ProgressLatencyTest(P2PPing ping, ulong senderId, bool isFinal = false)
        {
            if (!isFinal)
            {
                if (Client.Instance.Lobby.Owner == Client.Instance.SteamId)
                {
                    _hostSentFrame = P2PHandler.Instance.FramesLapsed;
                    Events.OnPingSent(Client.Instance.SteamId, senderId, P2PHandler.Instance.FramesLapsed);
                }
                else
                {   
                    NetworkLatency = ping.LocalFrame - _sentFrame;
                    Events.OnFirstNetworkLatencyCalculated(Client.Instance.SteamId, senderId, P2PHandler.Instance.FramesLapsed);
                }
            }
            // host just recieved final packet
            else
            {
                if (ping.LocalFrame - _hostSentFrame > NetworkLatency)
                {
                    NetworkLatency = ping.LocalFrame - _hostSentFrame;
                }
                Events.OnFinalNetworkLatencyCalculated(NetworkLatency);
                
            }
            
            
        }
    }
}