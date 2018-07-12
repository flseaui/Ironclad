using UnityEngine;
using UnityEngine.Networking;

namespace PLAYER
{
    class PlayerInfo : NetworkBehaviour
    {
        [SyncVar]
        public Color color;
    }
}