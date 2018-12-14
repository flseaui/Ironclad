using System;
using System.Collections.Generic;
using System.Linq;
using DATA;
using MANAGERS;
using NETWORKING;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        private bool _predicting;
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
        
        private void FixedUpdate()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && !MatchStateManager.Instance.ReadyToFight)
                return;

            /*
             * if changed inputs has 1 item
             *     parse the item
             *     save game state
             * else if changed inputs has 0 items
             *     start predicting
             * else if changed inputs has more than 1 item
             *     for input in changed inputs
             *         if changed inputs[i] != predicted inputs[i]
             *             rollback
             *             return
             *     save game state
             */

            Debug.Log(_changedInputs.Count);
            
            if (_changedInputs.Count == 1 && _predictedInputChanges.Count == 0)
            {
                ParseInputs(ref _changedInputs);
                RollbackManager.Instance.SaveGameState();
                return;
            }

            if (_changedInputs.Count == 0)
            {
                _predictedInputChanges.Add(PredictInputs());
                Debug.Log("predicting...");
                return;
            }

            if (_predictedInputChanges.Count != 0)
            {
                _predictedInputChanges.Add(PredictInputs());
                Debug.Log("predicting again...");
            }

            for (var i = 0; i < _changedInputs.Count; i++)
            {
                if (!_changedInputs[i].SequenceEqual(_predictedInputChanges[i]))
                {
                    RollbackManager.Instance.Rollback(0);
                    _changedInputs.Clear();
                    _predictedInputChanges.Clear();
                    return;
                }
            }
            
            Debug.Log("11111");
            _changedInputs.Clear();
            _predictedInputChanges.Clear();
            RollbackManager.Instance.SaveGameState();           
        }

        private P2PInputSet.InputChange[] PredictInputs()
        {
            return new P2PInputSet.InputChange[]
            {
              
            };
        }
    
        public void ParseInputs(ref List<P2PInputSet.InputChange[]> inputChanges)
        {
            for (int i = 0; i < inputChanges.Count; i++)
            {
                foreach (var inputChange in inputChanges[i])
                {
                    Inputs[(int) inputChange.InputType] = inputChange.State;
                }
            }

            Debug.Log("2222fffffffffffffffffffffffffffffffffffffffffff22");
            inputChanges.Clear();
        }

    }
}