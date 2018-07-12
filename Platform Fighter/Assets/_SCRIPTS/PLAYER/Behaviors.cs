using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
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
}
