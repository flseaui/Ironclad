using System;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{
    public class InputSender : MonoBehaviour
    {
        public bool[] Inputs;

        protected PlayerData PlayerData { get; set; }
        
        protected virtual void Awake()
        {
            Inputs = new bool[Enum.GetNames(typeof(Types.Input)).Length];
            PlayerData = GetComponent<PlayerData>();
        }
    }
}