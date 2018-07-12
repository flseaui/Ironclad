using UnityEngine;
using DATA;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerData : MonoBehaviour
    {

        public double Acceleration;
        public double TerminalVelocity;
        public bool Grounded;
        public Types.ActionType CurrentAction;
        public Types.Direction Direction;

    }
}
