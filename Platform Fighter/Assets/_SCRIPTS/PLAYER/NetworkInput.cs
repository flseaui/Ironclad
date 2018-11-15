using System;
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
        
        private P2PInputSet.InputChange[] ChangedInputs { get; set; }

        public void GiveInputs(P2PInputSet.InputChange[] changedInputs)
        {
            ChangedInputs = changedInputs;
            HasInputs = true;
        }
        
        private void FixedUpdate()
        {
            if (!MatchStateManager.Instance.ReadyToFight)
                return;
            
            for (var index = 0; index < Inputs.Length; index++)
            {
                Inputs[index] = false;
            }
            
            if (HasInputs)
            {
                Debug.Log("WE GOT " + ChangedInputs);
                
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

        private P2PInputSet.InputChange[] PredictInputs()
        {
            return new P2PInputSet.InputChange[] {  };
        }
        
        public void ParseInputs(P2PInputSet.InputChange[] inputs)
        {
            foreach (var input in inputs)
            {
                Inputs[(int) input.InputType] = input.State;
            }
        }
    }
}