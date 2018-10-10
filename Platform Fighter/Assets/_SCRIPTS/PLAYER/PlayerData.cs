using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : MonoBehaviour
    {
        public enum PlayerLocation
        {
            Grounded,
            Airborne
        }

        public enum PlayerState
        {
            KnockedDown,
            OnLedge,
            Stunned,
            FreeFall
        }

        public Types.ActionType CurrentAction;

        public Vector2
            CurrentVelocity,
            TargetVelocity,
            Acceleration,
            KnockbackVelocity;

        public Types.Direction Direction;

        public float gravity;

        public double Percent;

        public Vector2 TerminalVelocity;
    }
}