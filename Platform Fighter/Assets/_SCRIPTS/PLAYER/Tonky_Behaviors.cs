using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Types;

public class Tonky_Behaviors{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName (ActionType action, ref double acclereation, ref double terminal_velocity) {
        
        switch (action)
        {

            case (ActionType.WALK):
                acclereation = .25;
                terminal_velocity = 2.5;
                break;

            case (ActionType.RUN):
                acclereation = .25;
                terminal_velocity = 5;
                break;

        }
    }
}
