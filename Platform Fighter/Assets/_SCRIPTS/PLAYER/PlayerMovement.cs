using ATTRIBUTES;
using DATA;
using MISC;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [StepOrder(3), RequireComponent(typeof(PlayerData)), RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : Steppable
    {
        [SerializeField] private Vector2 _addedForce;

        private PlayerDataPacket Data { get; set; }
        private PlayerController PlayerController { get; set; }
        
        protected sealed override void Step()
        {
            CalculateVelocity();
         
            transform.Translate(Data.CurrentVelocity);
            Data.Position = transform.position;
        }

        protected override void LateAwake()
        {
            Data = GetComponent<PlayerData>().DataPacket;
            PlayerController = GetComponent<PlayerController>();
        }

        private void CalculateVelocity()
        {
            //Temp Variables

            float deceleration;
            float gravity;
            float terminalVelocity;

            deceleration = .01f;
            gravity = -0.0f;
            if (Data.RelativeLocation == PlayerDataPacket.PlayerLocation.Airborne)
                terminalVelocity = 0.00f;
            else
                terminalVelocity = 0.00f;
            if (Data.KnockbackVelocity != Vector2.zero)
            {
                //Apply DI

                Data.CurrentVelocity = Data.KnockbackVelocity;
                if (Data.KnockbackVelocity.x != 0)
                {
                    if (Mathf.Abs(Data.KnockbackVelocity.x) > deceleration)
                        Data.KnockbackVelocity.x -= Data.KnockbackVelocity.x > 0 ? deceleration : -deceleration;
                    else
                        Data.KnockbackVelocity.x = 0;
                }
                else
                {
                    if (Data.KnockbackVelocity.y + gravity > terminalVelocity)
                        Data.KnockbackVelocity.y += gravity;
                    else
                        Data.KnockbackVelocity.y = terminalVelocity;
                }
            }
            else if (Data.VelocityModifier != ActionInfo.VelocityModifier.ModificationType.IgnoreBoth)
            {
                if (Data.VelocityModifier != ActionInfo.VelocityModifier.ModificationType.IgnoreX)
                {
                    if (Data.TargetVelocity.x == 0)
                    {
                        if (Mathf.Abs(Data.CurrentVelocity.x) > deceleration)
                            Data.CurrentVelocity.x -= Data.CurrentVelocity.x > 0 ? deceleration : -deceleration;
                        else
                            Data.CurrentVelocity.x = 0;
                    }
                    else
                    {
                        Data.CurrentVelocity.x = Data.TargetVelocity.x;
                    }
                }

                //Perhaps decay X velocity if ignored, for now unknown
                if (Data.VelocityModifier != ActionInfo.VelocityModifier.ModificationType.IgnoreY)
                {
                    Data.CurrentVelocity.y = Data.TargetVelocity.y;
                }
                else
                {
                    if (Data.CurrentVelocity.y + gravity > terminalVelocity)
                        Data.CurrentVelocity.y += gravity;
                    else
                        Data.CurrentVelocity.y = terminalVelocity;
                }
            }
            else
            {
                if (Data.CurrentVelocity.y + gravity > terminalVelocity)
                    Data.CurrentVelocity.y += gravity;
                else
                    Data.CurrentVelocity.y = terminalVelocity;
            }
        }
    }
}