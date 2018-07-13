using UnityEngine;
using UnityEngine.Networking;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : NetworkBehaviour
    {

        public Vector2 Acceleration;
        public Vector2 TerminalVelocity;
        public bool Grounded;
        public Types.ActionType CurrentAction;
        [SyncVar]
        public Types.Direction Direction;

    }
}
