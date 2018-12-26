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

        public void HandleInputs()
        {
            var numReceivedInputSets = _receivedInputSets.Count;
            
            Debug.Log("HandleInputs: " + numReceivedInputSets);
            if (numReceivedInputSets > 0)
            {
                for (var index = 0; index < numReceivedInputSets; index++)
                {
                    var receivedPacketNum = _receivedInputSets[0].PacketNumber % 600;

                    var curPacketsReceived =
                        _p2pHandler.InputPacketsReceived < 300 && _receivedInputSets[0].PacketNumber > 300
                            ? _p2pHandler.InputPacketsReceived + 600
                            : _p2pHandler.InputPacketsReceived;

                    var numPerdictedInputSets = _predictedInputSets.Count;

                    var currentPacketIndex = curPacketsReceived + numPerdictedInputSets;

                    Debug.Log($"received: {receivedPacketNum}, total: {curPacketsReceived}, total+predicted: {currentPacketIndex}");
                    if (receivedPacketNum == currentPacketIndex)
                    {
                        ParseInputs(_receivedInputSets[0]);
                        RollbackManager.Instance.SaveGameState();
                        P2PHandler.Instance.OnInputPacketsReceived();
                        ArchivedInputSets.Add(_receivedInputSets[0]);
                        _receivedInputSets.RemoveAt(0);
                        _queuePrediction = false;
                    }
                    else
                    {
                        if (receivedPacketNum > currentPacketIndex)
                        {
                            _queuedInputSets.Add(_receivedInputSets[0]);
                            _receivedInputSets.RemoveAt(0);
                        }
                        else if (receivedPacketNum < currentPacketIndex)
                        {
                            var i = 0;
                            for (; i < numPerdictedInputSets; i++)
                            {
                                Debug.Log(
                                    $"predicted {_predictedInputSets[i].PacketNumber}, received: {receivedPacketNum}");
                                if (_predictedInputSets[i].PacketNumber == receivedPacketNum)
                                {
                                    var temp = "==RECEIVED==" + Environment.NewLine;
                                    var temp2 = "==PREDICTED==" + Environment.NewLine;
                                    foreach (var input in _receivedInputSets[0].Inputs)
                                    {
                                        var state = input.State ? "Pressed" : "Released";
                                        temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                                    }
                                    foreach (var input in _predictedInputSets[i].Inputs)
                                    {
                                        var state = input.State ? "Pressed" : "Released";    
                                        temp2 += $"[{input.InputType}]->{state}{Environment.NewLine}";
                                    }
                                    Debug.Log(temp + temp2);
                                    if (_receivedInputSets[0].Inputs.SequenceEqual(_predictedInputSets[i].Inputs))
                                    {
                                        P2PHandler.Instance.OnInputPacketsReceived();
                                        _predictedInputSets.RemoveAt(i);
                                        ArchivedInputSets.Add(_receivedInputSets[0]);
                                        _receivedInputSets.RemoveAt(0);
                                    }
                                    else
                                    {
                                        ArchivedInputSets.AddRange(_receivedInputSets);
                                        RollbackManager.Instance.Rollback(0);
                                        _predictedInputSets.Clear();
                                        _receivedInputSets.Clear();
                                        return;
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
            else
            {
                _queuePrediction = true;
            }

            var numQueuedInputSet = _queuedInputSets.Count;

            for (var i = 0; i < numQueuedInputSet; i++)
            {
                var queuedInputSet = _queuedInputSets[0];
                var curPacketsReceived = _p2pHandler.InputPacketsReceived < 300 && queuedInputSet.PacketNumber > 300
                    ? _p2pHandler.InputPacketsReceived + 600
                    : _p2pHandler.InputPacketsReceived;

                if (queuedInputSet.PacketNumber == curPacketsReceived)
                {
                    ParseInputs(queuedInputSet);
                    P2PHandler.Instance.OnInputPacketsReceived();
                    ArchivedInputSets.Add(_queuedInputSets[0]);
                    _queuedInputSets.RemoveAt(0);
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
            HandleInputs();
        }

        private P2PInputSet PredictInputs()
        {
            return new P2PInputSet(new P2PInputSet.InputChange[]{ }, (P2PHandler.Instance.InputPacketsReceived + _predictedInputSets.Count) % 600);
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