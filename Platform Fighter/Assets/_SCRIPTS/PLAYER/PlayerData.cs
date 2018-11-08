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
            DataPacket.CurrentAction = newData.CurrentAction;
            DataPacket.CurrentVelocity = newData.CurrentVelocity;
            DataPacket.TargetVelocity = newData.TargetVelocity;
            DataPacket.Acceleration = newData.Acceleration;
            DataPacket.KnockbackVelocity = newData.KnockbackVelocity;
            DataPacket.Direction = newData.Direction;
            DataPacket.Gravity = newData.Gravity;
            DataPacket.Percent = newData.Percent;
            DataPacket.TerminalVelocity = newData.TerminalVelocity;
            DataPacket.Position = newData.Position;
            GetComponent<Rigidbody2D>().MovePosition(newData.Position);
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
        
        //These have just become actions
        public enum PlayerState
        {
            KnockedDown,
            OnLedge,
            Stunned,
            FreeFall
        }

        public Vector2 MovementStickAngle;
        
        public int ArealActions;
        
        public int ArealActionsMax;

        public Types.ActionType CurrentAction;

        public Vector2 CurrentVelocity;

        public Vector2 TargetVelocity;

        public Vector2 Acceleration;

        public Vector2 KnockbackVelocity;

        public Types.Direction Direction;

        public float Gravity;

        public double Percent;

        public Vector2 TerminalVelocity;

        public Vector2 Position;

        public PlayerLocation RelativeLocation;
    }
}