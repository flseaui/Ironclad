using System;
using ATTRIBUTES;
using NETWORKING;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{    
    public class PlayerData : MonoBehaviour, ISettable
    {
        public PlayerDataPacket DataPacket;
        
        public void SetData(object data)
        {
            var newData = (PlayerDataPacket) data;
            DataPacket = newData;
        }
    }
    
    [Serializable]
    public class PlayerDataPacket
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

        public Vector2 CurrentVelocity;

        public Vector2 TargetVelocity;

        public Vector2  Acceleration;

        public Vector2 KnockbackVelocity;

        public Types.Direction Direction;

        public float Gravity;

        public double Percent;

        public Vector2 TerminalVelocity;
    }
}