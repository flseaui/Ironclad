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
        private bool _predicting;
        private bool _rollbackScheduled;
        private bool _gameSaveScheduled;
        private int _framesOfPrediction;

        private List<P2PInputSet.InputChange[]> _changedInputs;
        private List<P2PInputSet.InputChange[]> _predictedInputChanges;
        
        protected override void Awake()
        {
            base.Awake();
            _changedInputs = new List<P2PInputSet.InputChange[]>();
            _predictedInputChanges = new List<P2PInputSet.InputChange[]>();
        }

        public void GiveInputs(P2PInputSet.InputChange[] changedInputs)
        {
            _changedInputs.Add(changedInputs);
        }

        protected override void InputUpdate()
        {
            if (_changedInputs.Count > 0)
            {
                if (_predicting)
                {
                    for (var i = 0; i < _changedInputs.Count; i++)
                    {
                        Debug.Log("wow there is " + i + " epic input changes");
                        if (_changedInputs[i] != _predictedInputChanges[i])
                        {
                            ScheduleRollback();
                            break;
                        }
                    }

                    ScheduleGameSave();
                    _predicting = false;
                }
                else
                {
                    ScheduleGameSave();
                }
                
                ParseInputs(ref _changedInputs);
            }
            else
            {
                _predicting = true;
                ++_framesOfPrediction;
                
                var predictedInputs = PredictInputs();
                _predictedInputChanges.AddRange(predictedInputs);
                
                ParseInputs(ref predictedInputs);
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
                //RollbackManager.Instance.Rollback(0);

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
    
        public void ParseInputs(ref List<P2PInputSet.InputChange[]> inputs)
        {
            for (var index = 0; index < inputs.Count; index++)
            {
                var inputList = inputs[index];
                foreach (var input in inputList)
                {
                    if (input.FramesHeld != InputFramesHeld[(int) input.InputType])
                    {
                        _rollbackScheduled = true;
                    }
                    
                    Inputs[(int) input.InputType] = input.State;
                }

                inputs.RemoveAt(index);
            }
        }

        private void ScheduleGameSave() => _gameSaveScheduled = true;
        private void ScheduleRollback() => _rollbackScheduled = true;

    }
}