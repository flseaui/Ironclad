﻿using ATTRIBUTES;
using MANAGERS;
using MISC;
using UnityEngine;

namespace PLAYER
{
    [StepOrder(2), RequireComponent(typeof(PlayerData))]
    public abstract class Behaviors : Steppable
    {
        protected PlayerDataPacket Data { get; private set; }

        protected PlayerController PlayerController { get; private set; }

        protected sealed override void Step()
        {
            if (TimeManager.Instance.FixedUpdatePaused)
                return;
            
            RunAction();
        }

        protected override void LateAwake()
        {
            Data = GetComponent<PlayerData>().DataPacket;
            PlayerController = GetComponent<PlayerController>();
        }

        public abstract void RunAction();
    }
}