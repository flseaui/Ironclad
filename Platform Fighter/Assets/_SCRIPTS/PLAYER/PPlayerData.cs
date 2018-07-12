using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    public class PPlayerData : MonoBehaviour
    {
        private void Start()
        {
            Acceleration = 0;
            TerminalVelocity = 0;
            Grounded = true;
            CurrentAction = 0;
            Direction = 0;
        }

        public double Acceleration;
        public double TerminalVelocity;
        public bool Grounded;
        public ActionType CurrentAction;
        public Direction Direction;
    }
}