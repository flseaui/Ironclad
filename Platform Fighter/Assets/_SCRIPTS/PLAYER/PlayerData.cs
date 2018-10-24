using System;
using ATTRIBUTES;
using NETWORKING;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{
    [Serializable]
    public class PlayerData : MonoBehaviour, ISettable
    {
        public void SetData(object data)
        {
            var newData = (PlayerData) data;
            CurrentAction = newData.CurrentAction;
            CurrentVelocity = newData.CurrentVelocity;
            TargetVelocity = newData.TargetVelocity;
            Acceleration = newData.Acceleration;
            KnockbackVelocity = newData.KnockbackVelocity;
            Direction = newData.Direction;
            Gravity = newData.Gravity;
            Percent = newData.Percent;
            TerminalVelocity = newData.TerminalVelocity;
        }
        
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