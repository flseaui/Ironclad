using System;
using System.Collections.Generic;
using System.Linq;
using Boo.Lang.Environments;
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

        private bool _queuePrediction;
        private bool _receivedFirstInput;
        
        private int _framesOfPrediction;
        
        private List<P2PInputSet> _receivedInputSets;
        private List<P2PInputSet> _queuedInputSets;
        private List<P2PInputSet> _predictedInputSets;

        private P2PHandler _p2pHandler;
        
        protected override void Awake()
        {
            base.Awake();
            _receivedInputSets = new List<P2PInputSet>();
            _queuedInputSets = new List<P2PInputSet>();
            _predictedInputSets = new List<P2PInputSet>();
            _p2pHandler = P2PHandler.Instance;
        }

        public void GiveInputs(P2PInputSet receivedInputs)
        {
            _receivedFirstInput = true;
            
            _receivedInputSets.Add(receivedInputs);
            Debug.Log(receivedInputs.PacketNumber);
        }

        public void HandleInputs2()
        {
            Debug.Log("HandleInputs");
            if (_receivedInputSets.Count > 0)
            {
                for (var index = 0; index < _receivedInputSets.Count; index++)
                {
                    var receivedPacketNum = _receivedInputSets[index].PacketNumber % 600;

                    var curPacketsReceived =
                        _p2pHandler.InputPacketsReceived < 300 && _receivedInputSets[index].PacketNumber > 300
                            ? _p2pHandler.InputPacketsReceived + 600
                            : _p2pHandler.InputPacketsReceived;

                    Debug.Log($"receivedPacketNum: {receivedPacketNum}, curPacketsReceived: {curPacketsReceived} on {index}");
                    if (receivedPacketNum == curPacketsReceived)
                    {
                        ParseInputs(_receivedInputSets[index]);
                        RollbackManager.Instance.SaveGameState();
                        P2PHandler.Instance.OnInputPacketsReceived();
                        _receivedInputSets.RemoveAt(index);
                    }
                    else
                    {
                        _queuePrediction = true;
                        
                        if (receivedPacketNum > curPacketsReceived)
                        {
                            _queuedInputSets.Add(_receivedInputSets[index]);
                        }
                        else if (receivedPacketNum < curPacketsReceived + _predictedInputSets.Count)
                        {
                            var i = 0;
                            for (; i < _predictedInputSets.Count; i++)
                            {
                                if (_predictedInputSets[i].PacketNumber == receivedPacketNum)
                                {
                                    if (_receivedInputSets[index].Inputs.SequenceEqual(_predictedInputSets[i].Inputs))
                                    {
                                        _predictedInputSets.RemoveAt(i);
                                    }
                                    else
                                    {
                                        RollbackManager.Instance.Rollback(0);
                                        _predictedInputSets.Clear();
                                        _receivedInputSets.Clear();
                                    }

                                    break;
                                }
                            }

                            if (i == _predictedInputSets.Count)
                            {
                                //Debug.Log("THIS SHOULDNT HAPPEN");
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < _queuedInputSets.Count; i++)
            {
                var queuedInputSet = _queuedInputSets[i];
                var curPacketsReceived = _p2pHandler.InputPacketsReceived < 300 && queuedInputSet.PacketNumber > 300
                    ? _p2pHandler.InputPacketsReceived + 600
                    : _p2pHandler.InputPacketsReceived;

                if (queuedInputSet.PacketNumber == curPacketsReceived)
                {
                    ParseInputs(queuedInputSet);
                    P2PHandler.Instance.OnInputPacketsReceived();
                    _queuedInputSets.RemoveAt(i);
                }
            }

            if (_queuePrediction)
            {
                var predictedInputSet = PredictInputs();
                _predictedInputSets.Add(predictedInputSet);
                Debug.Log("PredictedInputSets: " + _predictedInputSets.Count);
                ParseInputs(predictedInputSet);
                _queuePrediction = false;
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && !P2PHandler.Instance.LatencyCalculated)
                return;
            
            if (!_receivedFirstInput) return;
            
            //HandleInputs();
            HandleInputs2();
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
                    
                    var curPacketsReceived = P2PHandler.Instance.InputPacketsReceived < 300 && receivedInputs.PacketNumber > 300
                        ? P2PHandler.Instance.InputPacketsReceived + 600
                        : P2PHandler.Instance.InputPacketsReceived;

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
                        if (queuedInputSet.PacketNumber == P2PHandler.Instance.InputPacketsReceived)
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
            return new P2PInputSet(new P2PInputSet.InputChange[]{ }, P2PHandler.Instance.InputPacketsReceived + _predictedInputSets.Count + 1);
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