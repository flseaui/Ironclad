using System;

namespace NETWORKING
{
    [Serializable]
    public struct P2PMessage
    {
        public ulong SteamId;
        public string Body;

        public P2PMessageKey Key;

        public P2PMessage(ulong steamId, P2PMessageKey key, string body)
        {
            SteamId = steamId;
            Key = key;
            Body = body;
        }
    }

    public enum P2PMessageKey
    {
        InputSet,
        Ping,
        Pong,
        GameStart
    }
}