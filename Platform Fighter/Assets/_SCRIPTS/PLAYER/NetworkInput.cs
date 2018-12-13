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
                    Debug.Log("wow there are " + _changedInputs.Count + " epic input changes");
                    var badPrediction = false;
                    for (var i = 0; i < _changedInputs.Count; i++)
                    {
                        if (_changedInputs[i] != _predictedInputChanges[i])
                        {
                            ScheduleRollback();
                            badPrediction = true;
                            break;
                        }
                    }

                    if (!badPrediction)
                    {
                        ScheduleGameSave();
                        _predicting = false; 
                        ParseInputs(ref _changedInputs);
                    }
                }
                else
                {
                    ScheduleGameSave();
                    ParseInputs(ref _changedInputs);
                }
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
    
        public void ParseInputs(ref List<P2PInputSet.InputChange[]> inputChanges)
        {
            for (var index = 0; index < inputChanges.Count; index++)
            {
                var inputChangeList = inputChanges[index];
                foreach (var inputChange in inputChangeList)
                {
                    if (inputChange.State == false && inputChange.FramesHeld != InputFramesHeld[(int) inputChange.InputType])
                    {
                        ScheduleRollback();
                    }
                    else
                    {
                        Inputs[(int) inputChange.InputType] = inputChange.State;
                    }
                }

                inputChanges.RemoveAt(index);
            }
        }

        private void ScheduleGameSave() => _gameSaveScheduled = true;
        private void ScheduleRollback() => _rollbackScheduled = true;

    }
}