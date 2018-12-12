using System;
using MANAGERS;
using UnityEngine;
using UnityEngine.Serialization;
using Types = DATA.Types;

namespace PLAYER
{
    public class InputSender : MonoBehaviour
    {
        public bool[] Inputs;

        protected bool[] PrevInputs;
        protected int[] InputFramesHeld;
        
        protected PlayerData PlayerData { get; set; }
        
        protected virtual void Awake()
        {
            Inputs = new bool[Enum.GetNames(typeof(Types.Input)).Length];
            PrevInputs = new bool[Inputs.Length];
            InputFramesHeld = new int[Inputs.Length];
            PlayerData = GetComponent<PlayerData>();
        }

        private void Update()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && !MatchStateManager.Instance.ReadyToFight)
                return;
            
            Inputs.CopyTo(PrevInputs, 0);
            
            InputUpdate();
            
            for (var index = 0; index < Inputs.Length; index++)
            {
                // if input is held from previous frame increase frames held counter
                if (Inputs[index] == PrevInputs[index])
                {
                    InputFramesHeld[index]++;
                }
                else if (Inputs[index] != PrevInputs[index])
                {
                    // if input change is a release
                    if (!Inputs[index])
                    {
                        ReleaseEvent.Invoke(index);
                        InputFramesHeld[index] = 0;
                    }
                    else
                    {
                        PressEvent.Invoke(index);  
                    }
                }
            }
        }

        // called after PrevInputs reset, before InputFramesHeld increased
        protected virtual void InputUpdate() { }
        protected Action<int> ReleaseEvent;
        protected Action<int> PressEvent;


    }
}