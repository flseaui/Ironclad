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
        
        [NonSerialized] protected bool[] Inputs;  
        [NonSerialized] protected bool[] RealTimeInputs;
        [NonSerialized] protected int[] InputFramesHeld;
        
        public List<P2PInputSet> ArchivedInputSets;

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
            if (TimeManager.Instance.FixedUpdatePaused)
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
            var temp = Environment.NewLine;
            if (ArchivedInputSets[index].Inputs.Length > 0)
            {
                foreach (var input in ArchivedInputSets[index].Inputs)
                {
                    var state = input.State ? "Pressed" : "Released";
                    temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                }
            }

            PlayerData.DataPacket.MovementStickAngle = ArchivedInputSets[index].Angle;
            foreach (var input in ArchivedInputSets[index].Inputs) Inputs[(int) input.InputType] = input.State;
            
            Debug.Log($"Applied ({ArchivedInputSets[index].PacketNumber}) on ({P2PHandler.Instance.DataPacket.FrameCounter}) containing {temp}");
        }

        public bool InputState(Types.Input input)
        {
            return Inputs[(int) input];
        }

        public int FramesHeld(Types.Input input)
        {
            return InputFramesHeld[(int) input];
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