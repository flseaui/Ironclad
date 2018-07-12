using UnityEngine;
using static DATA.Types;

namespace PLAYER
{
    [
        RequireComponent(typeof(PlayerInput)), 
        RequireComponent(typeof(PlayerData))
    ]
    public abstract class ActionsBase : MonoBehaviour
    {
        protected PlayerInput Input { get; set; }

        protected PlayerData Data { get; set; }

        private void Start()
        {
            Input = GetComponent<PlayerInput>();
            Data = GetComponent<PlayerData>();
        }

        private void Update() => Data.CurrentAction = GetCurrentAction();

        protected abstract ActionType GetCurrentAction();

    }
}
