using UnityEngine;

namespace PlatFighter.PLAYER
{
    public class Behaviors : MonoBehaviour {

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void ThatOneMethodThatWeReallyShouldNameButWeJustCantThinkOfAName (Types.ActionType action) {
        
            switch (action) {
                case (Types.ActionType.WALK):
                    break;
                case (Types.ActionType.RUN):
                    break;

            }


        }
    }
}
