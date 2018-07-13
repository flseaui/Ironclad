using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class TonkyActions : ActionsBase
    {

        // Returns action that should be started this frame based on current inputs
        // Assumes neutral/idle state
        
        protected override Types.ActionType GetCurrentAction()
        {
            if (Input.shortHop)
            {
                return Types.ActionType.SHOP;
            }

            if (Input.fullHop)
            {
                return Types.ActionType.FHOP;
            }
        
            if (Data.Direction == Types.Direction.RIGHT)
            {
                if (Data.Grounded)
                {
                    if (Input.lightRight)
                    {
                        return Types.ActionType.WALK;
                    }

                    if (Input.strongRight)
                    {
                        return Types.ActionType.RUN;
                    }

                    if (Input.lightLeft || Input.strongLeft)
                    {
                        Data.Direction = Types.Direction.LEFT;
                        return Types.ActionType.TURN;
                    }
                }
            }
            else if (Data.Direction == Types.Direction.LEFT)
            {
                if (Data.Grounded)
                {
                    if (Input.lightLeft)
                    {
                        return Types.ActionType.WALK;
                    }

                    if (Input.strongLeft)
                    {
                        return Types.ActionType.RUN;
                    }

                    if (Input.lightRight || Input.strongRight)
                    {
                        Data.Direction = Types.Direction.RIGHT;
                        return Types.ActionType.TURN;
                    }
                }
            }

            return Types.ActionType.IDLE;

        }
    }
}