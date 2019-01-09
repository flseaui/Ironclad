using MISC;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [RequireComponent(typeof(InputSender))]
    [RequireComponent(typeof(PlayerData))]
    public abstract class ActionsBase : MonoBehaviour, ISteppable
    {
        private PlayerController _playerController;

        protected PlayerDataPacket Data;

        protected InputSender Input;

        protected int CurrentActionFrame => _playerController.CurrentActionFrame;

        public void Step()
        {
            var newAction = GetCurrentAction();
            if (Data.CurrentAction != newAction)
                GetComponent<PlayerFlags>().SetFlagState(Types.Flags.ResetAction, Types.FlagState.Pending);

            Data.CurrentAction = newAction;
        }

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();

            if (GetComponent<UserInput>() != null)
                Input = GetComponent<UserInput>();
            else if (GetComponent<NetworkUserInput>() != null)
                Input = GetComponent<NetworkUserInput>();
            else
                Input = GetComponent<NetworkInput>();

            Data = GetComponent<PlayerData>().DataPacket;
        }

        private void FixedUpdate()
        {
            Step();
        }

        protected abstract Types.ActionType GetCurrentAction();
    }
}