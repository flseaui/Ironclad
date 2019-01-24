using Facepunch.Steamworks;
using MANAGERS;
using MISC;
using UnityEngine;

namespace NETWORKING
{
    public class LatencyTester : Singleton<LatencyTester>
    {
        private int _hostSentFrame;

        private int _sentFrame;
        public int NetworkLatency { get; set; }

        public void BeginTesting()
        {
            InvokeRepeating(nameof(SendPing), 0,2);
        }
        
        public void SendPing()
        {
            Events.OnPingSent(Time.realtimeSinceStartup);
        }

        public void ReceivePing(P2PPing msg, ulong senderID)
        {
            var ping = Time.realtimeSinceStartup - msg.SentTime;
            Events.OnPingCalculated(ping, senderID);
        }
    }
}