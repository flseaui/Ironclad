using DATA;
using static DATA.Types.Input;
using static DATA.Types.Direction;
using static DATA.Types.ActionType;

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
            var inputRight =
                Input.InputState(LightRight) ||
                Input.InputState(StrongRight) ||
                !Input.InputState(LightLeft) &&
                !Input.InputState(StrongLeft) &&
                Data.Direction == Right;

            //If on the Ground
            if (Data.RelativeLocation == PlayerDataPacket.PlayerLocation.Grounded)
            {
                if (Input.InputState(Types.Input.Jump))
                {
                    Data.Direction = inputRight ? Right : Left;
                    return Types.ActionType.Jump;
                }

                if (Input.InputState(Neutral))
                {
                    if (Input.InputState(Up)) return Utilt;
                    if (Input.InputState(Down)) return Dtilt;

                    if (Input.InputState(LightRight) || Input.InputState(StrongRight) ||
                        Input.InputState(LightLeft) || Input.InputState(StrongLeft))
                    {
                        Data.Direction = inputRight ? Right : Left;
                        return Ftilt;
                    }

                    return Jab;
                }

                if (Input.InputState(Strong))
                {
                    if (Input.InputState(Up)) return Ustrong;
                    if (Input.InputState(Down)) return Dstrong;

                    if (Input.InputState(LightRight) || Input.InputState(StrongRight) ||
                        Input.InputState(LightLeft) || Input.InputState(StrongLeft))
                    {
                        Data.Direction = inputRight ? Right : Left;
                        return Fstrong;
                    }

                    return Nstrong;
                }

                if (Input.InputState(Special))
                {
                    if (Input.InputState(Up)) return Uspecial;
                    if (Input.InputState(Down)) return Dspecial;

                    if (Input.InputState(LightRight) || Input.InputState(StrongRight) ||
                        Input.InputState(LightLeft) || Input.InputState(StrongLeft))
                    {
                        Data.Direction = inputRight ? Right : Left;
                        return Fstrong;
                    }

                    return Nstrong;
                }

                if (Input.InputState(LightRight) || Input.InputState(LightLeft))
                {
                    if (Data.Direction == (inputRight ? Right : Left))
                        return Walk;

                    Data.Direction = inputRight ? Right : Left;

                    //return Types.ActionType.Turn;
                }

                /*
                if (Input.InputState(Types.Input.StrongRight) || Input.InputState(Types.Input.StrongLeft))
                {
                    if (Data.Direction == (inputRight ? Types.Direction.Right : Types.Direction.Left)) return Types.ActionType.Run;

                    Data.Direction = inputRight ? Types.Direction.Right : Types.Direction.Left;
                    return Types.ActionType.Turn;
                } 
                */
                return Idle;
            }
            // if in the air

            Data.Direction = inputRight ? Right : Left;

            if (Input.InputState(Neutral))
            {
                if (Input.InputState(Up)) return Uair;
                if (Input.InputState(Down)) return Dair;

                if (Input.InputState(LightRight) || Input.InputState(StrongRight) ||
                    Input.InputState(LightLeft) || Input.InputState(StrongLeft))
                    return Fair;
                return Nair;
            }

            if (Input.InputState(Special))
            {
                if (Input.InputState(Up)) return Uspecial;
                if (Input.InputState(Down)) return Dspecial;

                if (Input.InputState(LightRight) || Input.InputState(StrongRight) ||
                    Input.InputState(LightLeft) || Input.InputState(StrongLeft))
                    return Fstrong;
                return Nstrong;
            }

            if (Data.CurrentAction == Types.ActionType.Jump)
            {                
                if (CurrentActionFrame < 7)
                    return Types.ActionType.Jump;
                
                if (Data.CurrentVelocity.y >= 0)
                    return Types.ActionType.Jump;
             
                if (Input.InputState(Types.Input.Jump))
                    return Types.ActionType.Jump;

                return Fall;
            }

            return Fall;
        }
    }
}