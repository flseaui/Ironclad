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
        private bool _rollbackScheduled;
        private bool _gameSaveScheduled;
        private int _framesOfPrediction;
        
        private List<P2PInputSet.InputChange[]> ChangedInputs { get; set; }

        protected override void Awake()
        {
            base.Awake();
            ChangedInputs = new List<P2PInputSet.InputChange[]>();
        }

        public void GiveInputs(P2PInputSet.InputChange[] changedInputs)
        {
            ChangedInputs.Add(changedInputs);
            HasInputs = true;
        }

        private void Update()
        {
            if (!MatchStateManager.Instance.ReadyToFight)
                return;
            
            if (HasInputs)
            {
                _gameSaveScheduled = true;

                if (_predicting)
                {
                    _rollbackScheduled = true;
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
        
        private void FixedUpdate()
        {
            if (!MatchStateManager.Instance.ReadyToFight)
                return;

            if (_gameSaveScheduled)
            {
                RollbackManager.Instance.SaveGameState();

                _gameSaveScheduled = false;
            }

            if (_rollbackScheduled)
            {
                RollbackManager.Instance.Rollback(0);

                _rollbackScheduled = false;
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