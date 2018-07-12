using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(PlayerInput))]
    public class ActionsBase : MonoBehaviour
    {
        protected PlayerInput _input;

        private void Start()
        {
            _input = GetComponent(typeof(PlayerInput)) as PlayerInput;
        }

        public ActionType GetCurrentAction()
        {
            return ActionType.NOTHING;
        }

    }
}
