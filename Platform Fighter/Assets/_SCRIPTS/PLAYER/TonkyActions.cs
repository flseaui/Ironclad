using NETWORKING;
using Rewired;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class TonkyActions : ActionsBase
    {
        // Returns action that should be started this frame based on current inputs
        // Assumes neutral/idle state

        private void Start()
        {
            Data.RelativeLocation = PlayerDataPacket.PlayerLocation.Grounded;
        }

        protected override Types.ActionType GetCurrentAction()
        {
            Debug.Log($"Right is {Input.Inputs[(int) Types.Input.LightRight]} on {P2PHandler.Instance.InputPacketsSent}");
            bool inputRight =
                Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight] ? true : 
                    (Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft] ? false : 
                        Data.Direction ==  Types.Direction.Right);                    
            
            //If on the Ground
            
            if (Data.RelativeLocation == PlayerDataPacket.PlayerLocation.Grounded)
            {   
                if (Input.Inputs[(int) Types.Input.Jump])
                {
                    Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;
                    return Types.ActionType.Jump;
                }
                
                if (Input.Inputs[(int) Types.Input.Neutral])
                {
                    if (Input.Inputs[(int) Types.Input.Up]) return Types.ActionType.Utilt;
                    if (Input.Inputs[(int) Types.Input.Down]) return Types.ActionType.Dtilt;
                    
                    if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight] ||
                        Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft])
                    {
                        Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;
                        return Types.ActionType.Ftilt;
                    }                 
                        return Types.ActionType.Jab;
                }
                
                if (Input.Inputs[(int) Types.Input.Strong])
                {
                    if (Input.Inputs[(int) Types.Input.Up]) return Types.ActionType.Ustrong;
                    if (Input.Inputs[(int) Types.Input.Down]) return Types.ActionType.Dstrong;
                    
                    if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight] ||
                        Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft])
                    {
                        Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;
                        return Types.ActionType.Fstrong;
                    }
                        return Types.ActionType.Nstrong;
                }
                
                if (Input.Inputs[(int) Types.Input.Special])
                {
                    if (Input.Inputs[(int) Types.Input.Up]) return Types.ActionType.Uspecial;
                    if (Input.Inputs[(int) Types.Input.Down]) return Types.ActionType.Dspecial;
                    
                    if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight] ||
                        Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft])
                    {
                        Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;
                        return Types.ActionType.Fstrong;
                    }
                        return Types.ActionType.Nstrong;
                }

                if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.LightLeft])
                {
                    if (Data.Direction == (inputRight ? Types.Direction.Right : Types.Direction.Left))
                    {
                        GetComponent<PlayerFlags>().SetFlagState(Types.Flags.ResetAction, Types.FlagState.Pending);
                        return Types.ActionType.Walk;
                    }

                    Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;

                    return Types.ActionType.Turn;
                }
                
                if (Input.Inputs[(int) Types.Input.StrongRight] || Input.Inputs[(int) Types.Input.StrongLeft])
                {
                    if (Data.Direction == (inputRight ? Types.Direction.Right : Types.Direction.Left)) return Types.ActionType.Run;

                    Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;
                    return Types.ActionType.Turn;
                } 
                
                GetComponent<PlayerFlags>().SetFlagState(Types.Flags.ResetAction, Types.FlagState.Pending);
                return Types.ActionType.Idle;
            }
            // if in the air
            
            Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;

            if (Input.Inputs[(int) Types.Input.Neutral])
            {
                if (Input.Inputs[(int) Types.Input.Up]) return Types.ActionType.Uair;
                if (Input.Inputs[(int) Types.Input.Down]) return Types.ActionType.Dair;
                    
                if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight] ||
                    Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft])
                        return Types.ActionType.Fair;             
                return Types.ActionType.Nair;
            }
            
            if (Input.Inputs[(int) Types.Input.Special])
            {
                if (Input.Inputs[(int) Types.Input.Up]) return Types.ActionType.Uspecial;
                if (Input.Inputs[(int) Types.Input.Down]) return Types.ActionType.Dspecial;
                    
                if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight] ||
                    Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft])
                        return Types.ActionType.Fstrong;
                return Types.ActionType.Nstrong;
            }
            
            if (Data.CurrentAction == Types.ActionType.Jump)
            {
                if (CurrentActionFrame < 7)
                    return Types.ActionType.Jump;
                if (CurrentActionFrame > 7)
                {
                    if (GetComponent<PlayerFlags>().GetFlagState(Types.Flags.FullHop) == Types.FlagState.Pending)
                    {
                        GetComponent<PlayerFlags>().SetFlagState(Types.Flags.ResetAction, Types.FlagState.Pending);
                        return Types.ActionType.Jump;
                    }
                        
                    if(Data.CurrentVelocity.y >= 0)
                        return Types.ActionType.Jump;

                    return Types.ActionType.Fall;
                }
                //Frame 7
                
                if(GetComponent<PlayerFlags>().GetFlagState(Types.Flags.FullHop) == Types.FlagState.Pending)
                    return Types.ActionType.Jump;

                return Types.ActionType.Fall;
            }

            return Types.ActionType.Fall;
        }
    }
}