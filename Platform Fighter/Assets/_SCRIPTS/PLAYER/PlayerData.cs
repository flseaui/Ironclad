using UnityEngine;
using UnityEngine.Networking;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : NetworkBehaviour
    {
        public Vector2 Acceleration;
        public Types.ActionType CurrentAction;

        [SyncVar] public Types.Direction Direction;

        public bool Grounded;
        public Vector2 TerminalVelocity;
    }
}