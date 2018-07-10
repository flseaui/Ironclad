using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Types;

public class Behaviors : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName (ActionType action) {
        
        switch (action) {
            case (ActionType.WALK):
                break;
            case (ActionType.RUN):
                break;

        }


    }
}
