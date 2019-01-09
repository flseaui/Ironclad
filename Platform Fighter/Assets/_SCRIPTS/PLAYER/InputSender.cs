using System;
using System.Collections.Generic;
using MANAGERS;
using NETWORKING;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class InputSender : MonoBehaviour
    {
        private bool[] _prevInputs;

        [NonSerialized] public List<P2PInputSet> ArchivedInputSets;

        [NonSerialized] protected int[] InputFramesHeld;

        [NonSerialized] public bool[] Inputs;

        [NonSerialized] protected bool[] RealTimeInputs;

        protected PlayerData PlayerData { get; private set; }

        protected virtual void Awake()
        {
            Inputs = new bool[Enum.GetNames(typeof(Types.Input)).Length];
            RealTimeInputs = new bool[Inputs.Length];
            InputFramesHeld = new int[Inputs.Length];
            _prevInputs = new bool[Inputs.Length];
            PlayerData = GetComponent<PlayerData>();
            ArchivedInputSets = new List<P2PInputSet>();
        }

        private void Update()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer &&
                !P2PHandler.Instance.LatencyCalculated)
                return;

            RealTimeInputs.CopyTo(_prevInputs, 0);

            InputUpdate();

            for (var i = 0; i < RealTimeInputs.Length; i++)
                // if input is held from previous frame increase frames held counter
                if (RealTimeInputs[i] == _prevInputs[i])
                {
                    InputFramesHeld[i]++;
                }
                else if (RealTimeInputs[i] != _prevInputs[i])
                {
                    // if input change is a release
                    if (!RealTimeInputs[i])
                    {
                        ReleaseEvent(i);
                        InputFramesHeld[i] = 0;
                    }
                    else
                    {
                        PressEvent(i);
                    }
                }

            InputLateUpdate();
        }

        public void ApplyArchivedInputSet(int index)
        {
            Debug.Log(
                $"[player{GetComponent<NetworkIdentity>().Id}] index: {index}, length: {ArchivedInputSets.Count}");
            foreach (var input in ArchivedInputSets[index].Inputs) Inputs[(int) input.InputType] = input.State;

            if (GetComponent<NetworkIdentity>().Id != MatchStateManager.Instance.ClientPlayerId)
                if (ArchivedInputSets[index].PacketNumber > P2PHandler.Instance.InputPacketsProcessed)
                    P2PHandler.Instance.OnInputPacketsProcessed();
        }

        // called after PrevInputs reset, before InputFramesHeld increased
        protected virtual void InputUpdate()
        {
        }

        protected virtual void InputLateUpdate()
        {
        }

        protected virtual void ReleaseEvent(int index)
        {
        }

        protected virtual void PressEvent(int index)
        {
        }
    }
}