using System;
using System.Collections.Generic;
using System.Linq;
using DATA;
using MANAGERS;
using NETWORKING;
using UnityEngine;
using UnityScript.Steps;
using Types = DATA.Types;

namespace PLAYER
{
    public class NetworkInput : InputSender
    {
        private bool _queueEvaluation;
        private bool _queueParse;
        private bool _predicting;
        
        private int _framesOfPrediction;
        
        private List<P2PInputSet> _receivedInputSets;
        private List<P2PInputSet> _queuedInputSets;
        private List<P2PInputSet> _predictedInputSets;
        
        protected override void Awake()
        {
            base.Awake();
            _receivedInputSets = new List<P2PInputSet>();
            _queuedInputSets = new List<P2PInputSet>();
            _predictedInputSets = new List<P2PInputSet>();
        }

        public void GiveInputs(P2PInputSet receivedInputs)
        {
            Debug.Log($"recieved on {P2PHandler.Instance.FramesLapsed} from {receivedInputs.PacketNumber}");
            
            _receivedInputSets.Add(receivedInputs);
        }
        
        private void FixedUpdate()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && !MatchStateManager.Instance.ReadyToFight)
                return;
            
            HandleInputs();
        }

        private void HandleInputs()
        {
            _queueEvaluation = false;
            _queueParse = false;
            
            if (_receivedInputSets.Count > 0)
            {
                var queuedParseInput = new P2PInputSet();
                foreach (var receivedInputs in _receivedInputSets)
                {
                    var receivedPacketNum = receivedInputs.PacketNumber % 600;
                    
                    var curPacketsReceived = P2PHandler.Instance.PacketsReceived < 300 && receivedInputs.PacketNumber > 300
                        ? P2PHandler.Instance.PacketsReceived + 600
                        : P2PHandler.Instance.PacketsReceived;

                    //Debug.Log("recievedFrame: " + receivedInputFrame + " tempFrame: " + tempFramesLapsed);
                    
                    if (receivedPacketNum == curPacketsReceived)
                    {
                        queuedParseInput = receivedInputs;
                        _queueParse = true;
                    }
                    else if (receivedPacketNum < curPacketsReceived)
                        _queueEvaluation = true;
                    else if (receivedPacketNum > curPacketsReceived)
                        _queuedInputSets.Add(receivedInputs);
                }

                if (_queueEvaluation)
                {
                    for (var i = 0; i < _predictedInputSets.Count; i++)
                    {
                        foreach (var receivedInputSet in _receivedInputSets)
                        {
                            if (receivedInputSet.PacketNumber == _predictedInputSets[i].PacketNumber)
                            {
                                if (!receivedInputSet.Inputs.SequenceEqual(_predictedInputSets[i].Inputs))
                                {
                                    RollbackManager.Instance.Rollback(0);
                                    Debug.Log("Rollback");
                                    _receivedInputSets.Clear();
                                    _predictedInputSets.Clear();
                                    return;
                                }

                                break;
                            }
                        }
                    }
                }

                if (_queueParse)
                {
                    ParseInputs(queuedParseInput);   
                    _receivedInputSets.Clear();
                    Debug.Log("Parsed");
                    RollbackManager.Instance.SaveGameState();
                }        
            }
            else
            {
                if (_queuedInputSets.Count != 0)
                {
                    for (var i = 0; i < _queuedInputSets.Count; i++)
                    {
                        var queuedInputSet = _queuedInputSets[i];
                        if (queuedInputSet.PacketNumber == P2PHandler.Instance.PacketsReceived)
                        {
                            _receivedInputSets.Add(queuedInputSet);
                            _queuedInputSets.RemoveAt(i);
                            HandleInputs();
                            return;
                        }
                    }
                }
                
                _predictedInputSets.Add(PredictInputs());
                Debug.Log("predicting...");
                return;
            }
        }
        
        private P2PInputSet PredictInputs()
        {
            return new P2PInputSet(new P2PInputSet.InputChange[]{ }, P2PHandler.Instance.PacketsReceived + _predictedInputSets.Count + 1);            
        }
    
        public void ParseInputs(P2PInputSet inputSet)
        {
            foreach (var inputChange in inputSet.Inputs)
            {
                Inputs[(int) inputChange.InputType] = inputChange.State;
            }
        }
    }
}