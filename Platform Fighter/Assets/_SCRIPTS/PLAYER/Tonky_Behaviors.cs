using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Types;

public class Tonky_Behaviors : MonoBehaviour {

    double acclereation;
    double terminal_velocity;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ActionType CurrentAction = 0;
        acclereation = 0;
        terminal_velocity = 0;
        ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName(CurrentAction);
	}

    public void ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName (ActionType action) {
        
        switch (action) {
            case (ActionType.WALK):
                acclereation = .25;
                terminal_velocity = 2.5;
                break;

            case (ActionType.RUN):
                acclereation = .25;
                terminal_velocity = 2.5;
                break;

        }


    }
}
