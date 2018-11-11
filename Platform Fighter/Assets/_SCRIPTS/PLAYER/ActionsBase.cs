using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(InputSender)), RequireComponent(typeof(PlayerData))]
    public abstract class ActionsBase : MonoBehaviour
    {
        private PlayerController _playerController;
        
        protected InputSender Input;

        protected PlayerDataPacket Data;
        
        protected int CurrentActionFrame => _playerController.CurrentActionFrame;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            
            if (GetComponent<PlayerInput>() != null)
                Input = GetComponent<PlayerInput>();
            else
                Input = GetComponent<NetworkInput>();
            
            Data = GetComponent<PlayerData>().DataPacket;
        }

        private void Update()
        {
            Data.CurrentAction = GetCurrentAction();
        }

        protected abstract Types.ActionType GetCurrentAction();
    }
}