using DATA;

namespace PLAYER
{
    public class TonkyActions : ActionsBase
    {
        // Returns action that should be started this frame based on current inputs
        // Assumes neutral/idle state

        protected override Types.ActionType GetCurrentAction()
        {
            if (Input.shortHop) return Types.ActionType.Shop;

            if (Input.fullHop) return Types.ActionType.Fhop;

            if (Data.Direction == Types.Direction.Right)
            {
                if (Data.Grounded)
                {
                    if (Input.lightRight) return Types.ActionType.Walk;

                    if (Input.strongRight) return Types.ActionType.Run;

                    if (Input.lightLeft || Input.strongLeft)
                    {
                        Data.Direction = Types.Direction.Left;
                        return Types.ActionType.Turn;
                    }
                }
            }
            else if (Data.Direction == Types.Direction.Left)
            {
                if (Data.Grounded)
                {
                    if (Input.lightLeft) return Types.ActionType.Walk;

                    if (Input.strongLeft) return Types.ActionType.Run;

                    if (Input.lightRight || Input.strongRight)
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