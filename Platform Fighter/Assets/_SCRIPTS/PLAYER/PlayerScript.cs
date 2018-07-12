using UnityEngine;

namespace PLAYER
{
    [RequireComponent(typeof(TonkyActions))]
    public class PlayerScript : MonoBehaviour
    {
        private PlayerData _data;

        private TonkyActions _actions;

        private IBehaviors _behaviors;

        private void Start () {
            _actions = GetComponent(typeof(TonkyActions)) as TonkyActions;

            _behaviors = new TonkyBehaviors();
        }
	
        private void Update () {

            _behaviors.RunAction(_actions.GetCurrentAction(), ref _data);

        }
    }
}
