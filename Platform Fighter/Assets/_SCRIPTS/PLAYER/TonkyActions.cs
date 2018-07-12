using UnityEngine;

namespace PlatFighter.PLAYER
{
    public class TonkyActions : ActionsBase
    {
        private PlayerInput _input;

        // TEMP VARIABLES WE ARE NOT USING THESE

        // (false - left, true - right)
        private Types.Direction _direction = Types.Direction.RIGHT;

        private bool _grounded = true;

        private Types.ActionType currentAction;

        private void Start()
        {
            _input = GetComponent(typeof(PlayerInput)) as PlayerInput;
        }

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
        public Types.ActionType GetCurrentAction()
        {
            if (_input.shortHop)
            {
                return Types.ActionType.SHOP;
            }

            if (_input.fullHop)
            {
                return Types.ActionType.FHOP;
            }
        
            if (_direction == Types.Direction.RIGHT)
            {
                if (_grounded)
                {
                    if (_input.lightRight)
                    {
                        return Types.ActionType.WALK;
                    }

                    if (_input.strongRight)
                    {
                        return Types.ActionType.RUN;
                    }

                    if (_input.lightLeft || _input.strongLeft)
                    {
                        _direction = Types.Direction.LEFT;
                        return Types.ActionType.TURN;
                    }
                }
            }
            else if (_direction == Types.Direction.LEFT)
            {
                if (_grounded)
                {
                    if (_input.lightLeft)
                    {
                        return Types.ActionType.WALK;
                    }

                    if (_input.strongLeft)
                    {
                        return Types.ActionType.RUN;
                    }

                    if (_input.lightRight || _input.strongRight)
                    {
                        _direction = Types.Direction.RIGHT;
                        return Types.ActionType.TURN;
                    }
                }
            }

            return Types.ActionType.IDLE;

        }
    }
}