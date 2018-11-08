using System;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{
    public class InputSender : MonoBehaviour
    {
        public bool[] Inputs;
        public Vector2 MovementStickAngle;

        private void Awake()
        {
            Inputs = new bool[Enum.GetNames(typeof(Types.Input)).Length];
            MovementStickAngle = Vector2.zero;
        }
    }
}