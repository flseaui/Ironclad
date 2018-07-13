using UnityEngine;
using DATA;
using UnityEngine.Networking;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : NetworkBehaviour
    {
        [SyncVar]
        public Vector2 Acceleration;
        [SyncVar]
        public Vector2 TerminalVelocity;
        [SyncVar]
        public bool Grounded;
        [SyncVar]
        public Types.ActionType CurrentAction;
        [SyncVar]
        public Types.Direction Direction;

    }
}
