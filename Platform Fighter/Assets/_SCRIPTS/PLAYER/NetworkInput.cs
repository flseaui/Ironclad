using System;
using System.Collections.Generic;
using System.Linq;
using MANAGERS;
using NETWORKING;
using UnityEngine;
using static MISC.MathUtils;
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
        }

        public void HandleInputs()
        {
            var numReceivedInputSets = _receivedInputSets.Count;
            
            var numQueuedInputSets = _queuedInputSets.Count;
            
            if (numReceivedInputSets > 0 || numQueuedInputSets != 0)
            {
                if (numReceivedInputSets == 0)
                {
                    for (var i = 0; i < numQueuedInputSets; i++)
                    {
                        var queuedInputSet = _queuedInputSets[i];

                        if (Mod(queuedInputSet.PacketNumber + _p2pHandler.Delay, 600) == _p2pHandler.InputPacketsReceived)
                        {
                            _receivedInputSets.Insert(numReceivedInputSets, _queuedInputSets[i]);
                            numReceivedInputSets++;
                            _queuedInputSets.RemoveAt(i);
                            break;
                        }
                    }
                }

                for (var index = 0; index < numReceivedInputSets; index++)
                {
                    var receivedPacketNum = Mod(_receivedInputSets[0].PacketNumber, 600);

                    var curPacketsReceived = _p2pHandler.InputPacketsReceived;

                    var numPredictedInputSets = _predictedInputSets.Count;

                    var currentPacketIndex = curPacketsReceived + numPredictedInputSets;

                    //Debug.Log($"received: {receivedPacketNum}, total: {curPacketsReceived}, total+predicted: {currentPacketIndex}");
                    if (Mod(receivedPacketNum + _p2pHandler.Delay, 600) == currentPacketIndex)
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
                        if (receivedPacketNum < 200 && currentPacketIndex > 400)
                            receivedPacketNum += 600;
                        
                        if (receivedPacketNum >= currentPacketIndex )
                        {
                            _queuedInputSets.Add(_receivedInputSets[0]);
                            _receivedInputSets.RemoveAt(0);
                        }
                        else
                        {
                            var i = 0;
                            for (; i < numPredictedInputSets; i++)
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
                        }
                    }
                }
            }
            else
            {
                _queuePrediction = true;
            }

            if (_queuePrediction)
            {
                var predictedInputSet = PredictInputs();
                _predictedInputSets.Add(predictedInputSet);
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
            return new P2PInputSet(new P2PInputSet.InputChange[]{ }, Mod(P2PHandler.Instance.InputPacketsReceived + _predictedInputSets.Count, 600));
        }
    
        public void ParseInputs(P2PInputSet inputSet)
        {
            if (inputSet.Inputs.Length > 0)
            {
                var temp = $"PARSED [{inputSet.PacketNumber}] on {P2PHandler.Instance.InputPacketsSent} {Environment.NewLine}";
                foreach (var input in inputSet.Inputs)
                {
                    var state = input.State ? "Pressed" : "Released";
                    temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                }
                Debug.Log(temp);
            }

            foreach (var inputChange in inputSet.Inputs)
            {
                Inputs[(int) inputChange.InputType] = inputChange.State;
            }
        }
    }
}