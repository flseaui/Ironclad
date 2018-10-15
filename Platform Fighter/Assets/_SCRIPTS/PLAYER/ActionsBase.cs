using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(InputSender)), RequireComponent(typeof(PlayerData))]
    public abstract class ActionsBase : MonoBehaviour
    {
        protected InputSender Input { get; set; }

        protected PlayerData Data { get; set; }

        private void Awake()
        {
            if (GetComponent<PlayerInput>() != null)
                Input = GetComponent<PlayerInput>();
            else
                Input = GetComponent<NetworkInput>();
            
            Data = GetComponent<PlayerData>();
        }

        private void Update()
        {
            Data.CurrentAction = GetCurrentAction();
        }

        protected abstract Types.ActionType GetCurrentAction();
    }
}