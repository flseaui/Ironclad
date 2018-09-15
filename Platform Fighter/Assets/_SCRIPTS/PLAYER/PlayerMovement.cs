using System;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData)), RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerData Data { get; set; }

        private Rigidbody2D Rigidbody { get; set; }

        private Vector2 AddedForce;

        private void Awake()
        {
            Data = GetComponent<PlayerData>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            CalculateVelocity();
            MovePlayer();
        }

        private void CalculateVelocity()
        {
            /*
            if (Data.MovementVelocity.y > 0)
            {
                Data.PlayerVelocity.y = Data.MovementVelocity.y;

                Data.MovementVelocity.y -= Data.gravity;

                if (Data.MovementVelocity.y > 0)
                    Data.MovementVelocity.y = 0;
            }
            else if (Data.MovementVelocity.y < 0)
            {
                Data.PlayerVelocity.y = Data.MovementVelocity.y;

                if (Data.PlayerVelocity.y > -Data.TerminalVelocity.y)
                {
                    Data.MovementVelocity.y -= Data.gravity;

                    if (Data.MovementVelocity.y < -Data.TerminalVelocity.y)
                        Data.MovementVelocity.y = -Data.TerminalVelocity.y;
                }
            }
            else
                Data.PlayerVelocity.y = 0;

            Data.MovementVelocity.x *= (Data.Direction == Types.Direction.Left ? -1 : 1);

            Data.PlayerVelocity.x = Data.MovementVelocity.x;

            Data.PlayerVelocity += Data.KnockbackVelocity;

            Data.KnockbackVelocity /= 2;

            if (Data.KnockbackVelocity.x < 1)
                Data.KnockbackVelocity.x = 0;
            if (Data.KnockbackVelocity.y < 1)
                Data.KnockbackVelocity.y = 0;
                
                */

            Debug.Log("CurrentVelocity: " + Data.CurrentVelocity);
            if (Data.CurrentVelocity.x != Data.TargetVelocity.x)
            {
                if (Data.CurrentVelocity.x > Data.TargetVelocity.x)
                    AddedForce.x = (Data.CurrentVelocity.x - Data.TargetVelocity.x >= Data.Acceleration.x
                        ? -Data.Acceleration.x
                        : -(Data.CurrentVelocity.x - Data.TargetVelocity.x));
                else
                    AddedForce.x = (Data.TargetVelocity.x - Data.CurrentVelocity.x >= Data.Acceleration.x
                        ? Data.Acceleration.x
                        : Data.TargetVelocity.x - Data.CurrentVelocity.x);
            }
            else
                AddedForce.x = 0;
            
            Rigidbody.AddForce(AddedForce, ForceMode2D.Impulse);
            Data.CurrentVelocity += AddedForce;


        }

        private void MovePlayer()
        {                   

        }
    }
}