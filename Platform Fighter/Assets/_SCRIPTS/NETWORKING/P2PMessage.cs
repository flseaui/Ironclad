using System;

namespace NETWORKING
{
    [Serializable]
    public struct P2PMessage
    {
        public int PlayerId;
        public string Body;

        public P2PMessageKey Key;

        public P2PMessage(int playerId, P2PMessageKey key, string body)
        {
            PlayerId = playerId;
            Key = key;
            Body = body;
        }
    }

    public enum P2PMessageKey
    {
        InputSet,
        Move,
        Spawn
    }
}