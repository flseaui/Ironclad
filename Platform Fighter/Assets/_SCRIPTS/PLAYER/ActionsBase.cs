using UnityEngine;

namespace PlatFighter.PLAYER
{
    [RequireComponent(typeof(PlayerInput))]
    public class ActionsBase : MonoBehaviour
    {
        private PlayerInput _input;

        private void Start()
        {
            _input = GetComponent(typeof(PlayerInput)) as PlayerInput;
        }

        public Types.ActionType GetCurrentAction()
        {
            return Types.ActionType.NOTHING;
        }

    }
}
