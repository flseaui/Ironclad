using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using DATA;
using NETWORKING;
using UnityEngine;
using Types = DATA.Types;
namespace PLAYER
{
    [RequireComponent(typeof(PlayerData)), RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _addedForce;
        private PlayerDataPacket Data { get; set; }
        private PlayerController PlayerController { get; set; }
        
        private void Awake()
        {
            Data = GetComponent<PlayerData>().DataPacket;
            PlayerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
           // MovePlayer(_addedForce, true);
            
            CalculateVelocity();
            
            transform.Translate(Data.CurrentVelocity);
            
            Data.Position = transform.position;
        }
        private void CalculateVelocity()
        {
            //Temp Variables
            
            float deceleration;
            float gravity;
            float terminalVelocity;
                
            deceleration = .01f;
            gravity = -0;
            if (Data.RelativeLocation == PlayerDataPacket.PlayerLocation.Airborne)
                terminalVelocity = -.075f;
            else
                terminalVelocity = 0;
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
                    if (Data.KnockbackVelocity.y + gravity  > terminalVelocity)
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
                        Data.CurrentVelocity.x = Data.TargetVelocity.x;
                }
                //Perhaps decay X velocity if ignored, for now unknown
                if (Data.VelocityModifier != ActionInfo.VelocityModifier.ModificationType.IgnoreY)
                {
                    if (Data.TargetVelocity.y == 99)
                    {
                        Debug.Log("What the fuck is happening");
                    }
                    
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
                
                Debug.Log("this is it buster: " + Data.CurrentVelocity.y);
            }
            
            if (Data.CurrentAction == Types.ActionType.Jump)
                Debug.Log("YEAH HERES SOME VELOCITY FAG " + Data.CurrentVelocity);
            
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
            
            /*
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
                
            */
        }
        /*
        public void MovePlayer(Vector2 addedForce, bool sendNetworkAction)
        {
            if (addedForce.x != 0)
            {
                Rigidbody.AddForce(addedForce, ForceMode2D.Impulse);
                Data.CurrentVelocity += addedForce;
                //Events.OnEntityMoved(GetComponent<NetworkIdentity>(), addedForce, sendNetworkAction);
            }
        }
        
        */
    }
}