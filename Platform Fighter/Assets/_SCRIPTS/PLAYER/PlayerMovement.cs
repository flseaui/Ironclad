using System.Security.Cryptography.X509Certificates;
using NETWORKING;
using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerData)), RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _addedForce;
        private PlayerDataPacket Data { get; set; }

        private Rigidbody2D Rigidbody { get; set; }

        private void Awake()
        {
            Data = GetComponent<PlayerData>().DataPacket;
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            CalculateVelocity();
        }

        private void FixedUpdate()
        {
            MovePlayer(_addedForce, true);
            
            Data.Position = Rigidbody.position;
        }

        private void CalculateVelocity()
        {
            Vector2 KnockbackDecay;
            KnockbackDecay.x = .01f;

            int gravity;
            
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
            
            if (Data.KnockbackVelocity.x != 0 || Data.KnockbackVelocity.y != 0)
            {
                Data.KnockbackVelocity.x -= Data.KnockbackVelocity.x - KnockbackDecay.x < 0 ? Data.KnockbackVelocity.x : KnockbackDecay.x;

                _addedForce.x = Data.KnockbackVelocity.x - Data.CurrentVelocity.x;

            }else if (Data.CurrentVelocity.x != Data.TargetVelocity.x)
            {
                if (Data.CurrentVelocity.x > Data.TargetVelocity.x)
                    _addedForce.x = Data.CurrentVelocity.x - Data.TargetVelocity.x >= Data.Acceleration.x
                        ? -Data.Acceleration.x
                        : -(Data.CurrentVelocity.x - Data.TargetVelocity.x);
                else
                    _addedForce.x = Data.TargetVelocity.x - Data.CurrentVelocity.x >= Data.Acceleration.x
                        ? Data.Acceleration.x
                        : Data.TargetVelocity.x - Data.CurrentVelocity.x;
            }
            else
                _addedForce.x = 0;

            if (Data.KnockbackVelocity.x != 0 || Data.KnockbackVelocity.y != 0)
                _addedForce.y = Data.KnockbackVelocity.y - Data.CurrentVelocity.y;
            else if (Data.TargetVelocity.y != 0)
                _addedForce.y = Data.TargetVelocity.y - Data.CurrentVelocity.y;
            else
                _addedForce.y = 0;

        }


        public void MovePlayer(Vector2 addedForce, bool sendNetworkAction)
        {
            if (addedForce.x != 0)
            {
                Rigidbody.AddForce(addedForce, ForceMode2D.Impulse);
                Data.CurrentVelocity += addedForce;
                //Events.OnEntityMoved(GetComponent<NetworkIdentity>(), addedForce, sendNetworkAction);
            }
        }
    }
}