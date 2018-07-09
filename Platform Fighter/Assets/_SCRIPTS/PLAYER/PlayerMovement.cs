using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input;

    private void Start()
    {
       input = GetComponent(typeof(PlayerInput)) as PlayerInput;
    }

    private void Update()
    {
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