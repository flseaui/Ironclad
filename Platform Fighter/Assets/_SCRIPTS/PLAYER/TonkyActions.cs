using DATA;

namespace PLAYER
{
    public class TonkyActions : ActionsBase
    {
        // Returns action that should be started this frame based on current inputs
        // Assumes neutral/idle state

        private PlayerDataPacket.PlayerLocation Position;

        protected override Types.ActionType GetCurrentAction()
        {
            if (Input.Inputs[(int) Types.Input.ShortHop]) return Types.ActionType.Shop;

            if (Input.Inputs[(int) Types.Input.FullHop]) return Types.ActionType.Fhop;

            if (Data.Direction == Types.Direction.Right)
            {
                if (Position == PlayerDataPacket.PlayerLocation.Grounded)
                {
                    if (Input.Inputs[(int) Types.Input.LightRight]) return Types.ActionType.Walk;

                    if (Input.Inputs[(int) Types.Input.StrongRight]) return Types.ActionType.Run;

                    if (Input.Inputs[(int) Types.Input.LightLeft] || Input.Inputs[(int) Types.Input.StrongLeft])
                    {
                        Data.Direction = Types.Direction.Left;
                        return Types.ActionType.Turn;
                    }
                }
            }
            else if (Data.Direction == Types.Direction.Left)
            {
                if (Position == PlayerDataPacket.PlayerLocation.Grounded)
                {
                    if (Input.Inputs[(int) Types.Input.LightLeft]) return Types.ActionType.Walk;

                    if (Input.Inputs[(int) Types.Input.StrongLeft]) return Types.ActionType.Run;

                    if (Input.Inputs[(int) Types.Input.LightRight] || Input.Inputs[(int) Types.Input.StrongRight])
                    {
                        Data.Direction = Types.Direction.Right;
                        return Types.ActionType.Turn;
                    }
                }
            }

            return Types.ActionType.Idle;
        }
    }
}