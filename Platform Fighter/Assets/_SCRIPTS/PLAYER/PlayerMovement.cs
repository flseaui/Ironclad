using Rewired.UI.ControlMapper;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;

    // TEMP VARIABLES WE ARE NOT USING THESE

    // (false - left, true - right)
    private bool facing;

    private bool grounded;


    private void Start()
    {
       input = GetComponent(typeof(PlayerInput)) as PlayerInput;
    }

    private void Update()
    {
        Types.ActionType action;

        if (facing)
        {
            if (grounded)
            {
                
                if (input.strongRight)
                {
                    if (input.neutral)
                    {
                        action = Types.ActionType.DASHATK;
                    }
                }
                else if (input.lightRight)
                {
                    if (input.neutral)
                    {
                        action = Types.ActionType.FTILT;
                    }
                }
                else if (input.lightRight || input.strongRight)
                {
                    if (input.special)
                    {
                        action = Types.ActionType.FSPECIAL;
                    }
                    else if (input.shield)
                    {
                        action = Types.ActionType.SHIELD;
                    }
                }
                if (input.lightLeft || input.strongLeft)
                {
                    if (input.neutral)
                    {
                        facing = false;
                        action = Types.ActionType.FTILT;
                    }
                    else if (input.special)
                    {
                        facing = false;
                        action = Types.ActionType.FSPECIAL;
                    }
                    else if (input.shield)
                    {
                        facing = false;
                        action = Types.ActionType.SPOTDODGE;
                    }
                }
            }
            else
            {
                if (input.lightRight || input.strongRight)
                {
                    if (input.neutral)
                    {
                        action = Types.ActionType.FAIR;
                    }
                    else if (input.special)
                    {
                        action = Types.ActionType.AIRFSPECIAL;
                    }
                    else if (input.shield)
                    {
                        action = Types.ActionType.AIRDODGE;
                    }
                }
            }
        }
        else
        {
            
        }

		// Action Debug 
		if (input.lightLeft)
			Debug.Log("Walking Left");
		else if (input.strongLeft)
			Debug.Log("Running Left");

		if (input.lightRight)
			Debug.Log("Walking Right");
		else if (input.strongRight)
			Debug.Log("Running Right");
            
    }
}