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
                    P2PHandler.Instance.DataPacket.FramesLapsed
                );
                _sentFrame = P2PHandler.Instance.DataPacket.FramesLapsed;
            }
        }

        // DO NOT CALL, INTERNAL ONLY
        public void ProgressLatencyTest(P2PPing ping, ulong senderId, bool isFinal = false)
        {
            if (!isFinal)
            {
                // host just recieved first packet
                if (Client.Instance.Lobby.Owner == Client.Instance.SteamId)
                {
                    _hostSentFrame = P2PHandler.Instance.DataPacket.FramesLapsed;
                    Events.OnPingSent(Client.Instance.SteamId, senderId, P2PHandler.Instance.DataPacket.FramesLapsed);
                }
                // user just recieved first packet from host
                else
                {   
                    NetworkLatency = ping.LocalFrame - _sentFrame;
                    Events.OnFirstNetworkLatencyCalculated(Client.Instance.SteamId, senderId, P2PHandler.Instance.DataPacket.FramesLapsed);
                }
            }
            else
            {
                // host just recieved final packet
                if (Client.Instance.Lobby.Owner == Client.Instance.SteamId)
                {
                    if (ping.LocalFrame - _hostSentFrame > NetworkLatency)
                    {
                        NetworkLatency = ping.LocalFrame - _hostSentFrame;
                    }

                    Events.OnFinalNetworkLatencyCalculated(NetworkLatency);
                    Events.OnFirstNetworkLatencyCalculated(Client.Instance.SteamId, senderId, NetworkLatency);
                }
                // user just recieved final packet from host
                else
                {
                    NetworkLatency = ping.LocalFrame;
                    Events.OnFinalNetworkLatencyCalculated(NetworkLatency);
                }
            }
            
            
        }
    }
}