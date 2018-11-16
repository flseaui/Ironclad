using System;
using System.Collections.Generic;
using DATA;
using MANAGERS;
using NETWORKING;
using UnityEngine;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        public bool HasInputs { get; set; }

        private bool _predicting;
        private int _framesOfPrediction;
        
        private List<P2PInputSet.InputChange[]> ChangedInputs { get; set; }

        private void Awake()
        {
            ChangedInputs = new List<P2PInputSet.InputChange[]>();
        }

        public void GiveInputs(P2PInputSet.InputChange[] changedInputs)
        {
            if (HasInputs) Debug.Log("this shouldnt be happening");
            ChangedInputs.Add(changedInputs);
            HasInputs = true;
        }
        
        private void FixedUpdate()
        {
            if (!MatchStateManager.Instance.ReadyToFight)
                return;
            
            if (HasInputs)
            {
                Debug.Log("We done got an input");
                RollbackManager.Instance.SaveGameState();
                
                if (_predicting)
                {
                    _predicting = false;
                    RollbackManager.Instance.Rollback(0);
                }
                ParseInputs(ChangedInputs);
                HasInputs = false;
            }
            else
            {
                _predicting = true;
                ++_framesOfPrediction;
                ParseInputs(PredictInputs());
            }
        }

        private List<P2PInputSet.InputChange[]> PredictInputs()
        {
            return new List<P2PInputSet.InputChange[]>
            {
                new P2PInputSet.InputChange[] { }
            };
        }
    
        
        public void ParseInputs(List<P2PInputSet.InputChange[]> inputs)
        {
            for (var index = 0; index < inputs.Count; index++)
            {
                var inputList = inputs[index];
                foreach (var input in inputList)
                {
                    Inputs[(int) input.InputType] = input.State;
                }

                inputs.RemoveAt(index);
            }
        }
    }
}