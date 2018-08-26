using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : MonoBehaviour
    {
        public Vector2 Acceleration;
        public Types.ActionType CurrentAction;

        public Types.Direction Direction;

        public bool Grounded;
        public Vector2 TerminalVelocity;
    }
}