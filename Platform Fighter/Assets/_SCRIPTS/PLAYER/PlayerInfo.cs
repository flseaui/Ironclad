using UnityEngine;
using UnityEngine.Networking;

namespace PLAYER
{
    internal class PlayerInfo : NetworkBehaviour
    {
        [SyncVar] public Color color;
    }
}