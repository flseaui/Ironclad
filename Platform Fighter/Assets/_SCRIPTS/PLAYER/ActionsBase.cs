﻿using ATTRIBUTES;
using MANAGERS;
using MISC;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    [StepOrder(0), RequireComponent(typeof(InputSender)), RequireComponent(typeof(PlayerData))]
    public abstract class ActionsBase : Steppable
    {
        private PlayerController _playerController;

        protected PlayerDataPacket Data;

        protected InputSender Input;

        protected int CurrentActionFrame => _playerController.CurrentActionFrame;

        protected sealed override void Step()
        {
            if (TimeManager.Instance.FixedUpdatePaused)
                return;
            
            var newAction = GetCurrentAction();
            if (Data.CurrentAction != newAction)
                GetComponent<PlayerFlags>().RaiseFlag(Types.PlayerFlags.ResetAction);

            Data.CurrentAction = newAction;
        }

        protected override void LateAwake()
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

        protected abstract Types.ActionType GetCurrentAction();
    }
}