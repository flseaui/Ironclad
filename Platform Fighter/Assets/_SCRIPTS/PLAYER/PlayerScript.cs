using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private double acceleration,
                   terminal_velocity;

    private Tonky_Actions _action;

    Tonky_Behaviors behaviors = new Tonky_Behaviors();


    // Use this for initialization
    void Start () {
        _action = GetComponent(typeof(Tonky_Actions)) as Tonky_Actions;
    }
	
	// Update is called once per frame
	void Update () {

        behaviors.ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName(_action.GetCurrentAction(), ref acceleration, ref terminal_velocity);

	}
}
