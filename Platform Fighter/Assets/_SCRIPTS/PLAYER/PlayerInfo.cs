using UnityEngine;
using UnityEngine.Networking;

namespace PlatFighter.PLAYER
{
    class PlayerInfo : NetworkBehaviour
    {
        [SyncVar]
        public Color color;
    }
}