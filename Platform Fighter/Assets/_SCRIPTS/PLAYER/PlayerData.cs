using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : MonoBehaviour
    {
        public Vector2 Acceleration;
        public Types.ActionType CurrentAction;

        public Types.Direction Direction;

        public enum PlayerLocation
        {
            Grounded,
            Airborne,
        }

        public enum PlayerState
        {
            KnockedDown,
            OnLedge,
            Stunned,
            FreeFall,
        }

        public Vector2 TerminalVelocity;

        public double Percent;

        public Vector2 
            PlayerVelocity,
            MovementVelocity,
            KnockbackVelocity;

        public float gravity;
    }
}