using static DATA.Types;

namespace PLAYER
{
    public class TonkyActions : ActionsBase
    {

        // Returns action that should be started this frame based on current inputs
        // Assumes neutral/idle state
        protected override ActionType GetCurrentAction()
        {
            if (Input.shortHop)
            {
                return ActionType.SHOP;
            }

            if (Input.fullHop)
            {
                return ActionType.FHOP;
            }
        
            if (Data.Direction == Direction.RIGHT)
            {
                if (Data.Grounded)
                {
                    if (Input.lightRight)
                    {
                        return ActionType.WALK;
                    }

                    if (Input.strongRight)
                    {
                        return ActionType.RUN;
                    }

                    if (Input.lightLeft || Input.strongLeft)
                    {
                        Data.Direction = Direction.LEFT;
                        return ActionType.TURN;
                    }
                }
            }
            else if (Data.Direction == Direction.LEFT)
            {
                if (Data.Grounded)
                {
                    if (Input.lightLeft)
                    {
                        return ActionType.WALK;
                    }

                    if (Input.strongLeft)
                    {
                        return ActionType.RUN;
                    }

                    if (Input.lightRight || Input.strongRight)
                    {
                        Data.Direction = Direction.RIGHT;
                        return ActionType.TURN;
                    }
                }
            }

            return ActionType.IDLE;

        }
    }
}