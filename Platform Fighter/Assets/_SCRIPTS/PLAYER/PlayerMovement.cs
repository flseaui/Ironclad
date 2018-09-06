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

            if (Data.MovementVelocity.x > 0)
                Data.PlayerVelocity.x = Data.MovementVelocity.x * (Data.Direction == Types.Direction.Left ? -1 : 1);

            Data.PlayerVelocity += Data.KnockbackVelocity;

            Data.KnockbackVelocity /= 2;

            if (Data.KnockbackVelocity.x < 1)
                Data.KnockbackVelocity.x = 0;

            if (Data.KnockbackVelocity.y < 1)
                Data.KnockbackVelocity.y = 0;
        }

        private void MovePlayer()
        {
            Debug.Log(Data.PlayerVelocity);
            Rigidbody.AddForce(Data.PlayerVelocity, ForceMode2D.Impulse);
        }
    }
}