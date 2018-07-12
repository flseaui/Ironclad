using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    public class TonkyActions : ActionsBase
    {

        // TEMP VARIABLES WE ARE NOT USING THESE

        // (false - left, true - right)
        private Direction _direction = Direction.RIGHT;

        private bool _grounded = true;

        private ActionType currentAction;

        private void Update()
        {
            //if (currentAction == ActionType.NOTHING)
            //{
            currentAction = GetCurrentAction();
            //}

            Debug.Log($"{currentAction} {_direction}");

        }

        // Returns action that should be started this frame based on current inputs
        // Assumes neutral/idle state
        public new ActionType GetCurrentAction()
        {
            if (_input.shortHop)
            {
                return ActionType.SHOP;
            }

            if (_input.fullHop)
            {
                return ActionType.FHOP;
            }
        
            if (_direction == Direction.RIGHT)
            {
                if (_grounded)
                {
                    if (_input.lightRight)
                    {
                        return ActionType.WALK;
                    }

                    if (_input.strongRight)
                    {
                        return ActionType.RUN;
                    }

                    if (_input.lightLeft || _input.strongLeft)
                    {
                        _direction = Direction.LEFT;
                        return ActionType.TURN;
                    }
                }
            }
            else if (_direction == Direction.LEFT)
            {
                if (_grounded)
                {
                    if (_input.lightLeft)
                    {
                        return ActionType.WALK;
                    }

                    if (_input.strongLeft)
                    {
                        return ActionType.RUN;
                    }

                    if (_input.lightRight || _input.strongRight)
                    {
                        _direction = Direction.RIGHT;
                        return ActionType.TURN;
                    }
                }
            }

            return ActionType.IDLE;

        }
    }
}