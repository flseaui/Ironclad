using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : MonoBehaviour
    {
        public Vector2 Acceleration;
        public Vector2 TerminalVelocity;
        public bool Grounded;
        public Types.ActionType CurrentAction;
        public Types.Direction Direction;

    }
}
