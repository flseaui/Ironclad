using System;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{
    public class InputSender : MonoBehaviour
    {
        public bool[] Inputs;

        private void Awake()
        {
            Inputs = new bool[Enum.GetNames(typeof(Types.Input)).Length];
        }
    }
}