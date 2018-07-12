using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(TonkyActions))]
    public class PlayerScript : MonoBehaviour {

        private double _acceleration,
            _terminalVelocity;

        private TonkyActions _action;

        TonkyBehaviors behaviors = new TonkyBehaviors();

        // Use this for initialization
        private void Start () {
            _action = GetComponent(typeof(TonkyActions)) as TonkyActions;
        }
	
        // Update is called once per frame
        void Update () {

            behaviors.RunAction(_action.GetCurrentAction(), ref _acceleration, ref _terminalVelocity);

        }
    }
}
