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

        private int _frameCounter = -1;
        private int previousDelay = 0;

        private bool ParsedForFrame;
        
        private List<P2PInputSet> _receivedInputSets;
        [SerializeField]
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
            
            if (receivedInputs.Inputs.Length > 0)
                Debug.Log($"Received: {receivedInputs.PacketNumber} on {P2PHandler.Instance.InputPacketsSent}");
            
            _receivedInputSets.Add(receivedInputs);
        }

        public void HandleInputs()
        {
            _frameCounter += Mod(1 +(previousDelay - _p2pHandler.Delay), 600);

            previousDelay = _p2pHandler.Delay;        
            
            var numReceivedInputSets = _receivedInputSets.Count;
            
            var numQueuedInputSets = _queuedInputSets.Count;
            
            if (numReceivedInputSets > 0 || numQueuedInputSets > 0)
            {
                for (var i = 0; i < numQueuedInputSets; i++)
                {
                    var queuedInputSet = _queuedInputSets[i];

                    if (queuedInputSet.PacketNumber == _frameCounter)
                    {
                        _receivedInputSets.Add(_queuedInputSets[i]);
                        numReceivedInputSets++;
                        _queuedInputSets.RemoveAt(i);
                        break;
                    }
                }

                ParsedForFrame = false;

                for (var index = 0; index < numReceivedInputSets; index++)
                {
                    var receivedPacketNum = Mod(_receivedInputSets[0].PacketNumber, 600);

                    var curPacketsProcessed = _p2pHandler.InputPacketsProcessed;

                    var numPredictedInputSets = _predictedInputSets.Count;

                    var currentPacketIndex = curPacketsProcessed + numPredictedInputSets - _p2pHandler.Delay + (_p2pHandler.InputPacketsReceived - curPacketsProcessed) - 1;

                    //Debug.Log($"received: {receivedPacketNum}, total: {curPacketsReceived}, total+predicted: {currentPacketIndex}");
                    if (receivedPacketNum == _frameCounter && !ParsedForFrame)
                    {
                        ParseInputs(_receivedInputSets[0]);
                        RollbackManager.Instance.SaveGameState();
                        P2PHandler.Instance.OnInputPacketsProcessed();
                        ArchivedInputSets.Add(_receivedInputSets[0]);
                        _receivedInputSets.RemoveAt(0);
                        _queuePrediction = false;
                        ParsedForFrame = true;
                    }
                    else
                    {
                        if (receivedPacketNum < 200 && _frameCounter > 400)
                            receivedPacketNum += 600;
                        
                        if (receivedPacketNum + _p2pHandler.Delay >= _frameCounter)
                        {
                            Debug.Log($"received: {receivedPacketNum} index: {currentPacketIndex}");
                            _queuedInputSets.Add(_receivedInputSets[0]);
                            _receivedInputSets.RemoveAt(0);
                        }
                        else
                        {
                            for (var i = 0; i < numPredictedInputSets; i++)
                            {
                                Debug.Log($"predicted {_predictedInputSets[0].PacketNumber}, received: {receivedPacketNum}");
                            
                                var temp = "==RECEIVED==" + Environment.NewLine;
                                var temp2 = "==PREDICTED==" + Environment.NewLine;
                            
                                foreach (var input in _receivedInputSets[i].Inputs)
                                {
                                    var state = input.State ? "Pressed" : "Released";
                                    temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                                }
                                foreach (var input in _predictedInputSets[0].Inputs)
                                {
                                    var state = input.State ? "Pressed" : "Released";    
                                    temp2 += $"[{input.InputType}]->{state}{Environment.NewLine}";
                                }
                                Debug.Log(temp + temp2);
                                if (_receivedInputSets[i].Inputs.SequenceEqual(_predictedInputSets[0].Inputs))
                                {
                                    P2PHandler.Instance.OnInputPacketsProcessed();
                                    _predictedInputSets.RemoveAt(0);
                                    ArchivedInputSets.Add(_receivedInputSets[i]);
                                }
                                else
                                {
                                    ArchivedInputSets.AddRange(_receivedInputSets);
                                    RollbackManager.Instance.Rollback(0);
                                    _predictedInputSets.Clear();
                                    _receivedInputSets.Clear();
                                    return;
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
            return new P2PInputSet(new P2PInputSet.InputChange[]{ }, Mod(_frameCounter, 600));
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