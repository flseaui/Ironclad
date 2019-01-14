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
        private int _framesOfPrediction;

        private P2PHandlerPacket _p2pHandler;
        [SerializeField]
        private List<P2PInputSet> _predictedInputSets;
        [SerializeField]
        private List<P2PInputSet> _queuedInputSets;
        private bool _queueEvaluation;
        private bool _queueParse;

        private bool _queuePrediction;
        [SerializeField]
        private List<P2PInputSet> _receivedInputSets;

        private bool ParsedForFrame;

        protected override void Awake()
        {
            base.Awake();
            _receivedInputSets = new List<P2PInputSet>();
            _queuedInputSets = new List<P2PInputSet>();
            _predictedInputSets = new List<P2PInputSet>();
            _p2pHandler = P2PHandler.Instance.DataPacket;
        }

        public void GiveInputs(P2PInputSet receivedInputs)
        {

            var temp = Environment.NewLine;


            if (receivedInputs.Inputs.Length > 0)
            {
                foreach (var input in receivedInputs.Inputs)
                {
                    var state = input.State ? "Pressed" : "Released";
                    temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                }
            }

            Debug.Log($"Received: {receivedInputs.PacketNumber} on {_p2pHandler.FrameCounter} containing {temp}");

            _receivedInputSets.Add(receivedInputs);
        }

        public void HandleInputs()
        {
            Debug.Log($"-----HANDLE INPUTS [{_p2pHandler.FrameCounter}]----------------------------------------");

            var numReceivedInputSets = _receivedInputSets.Count;

            var numQueuedInputSets = _queuedInputSets.Count;

            if (numReceivedInputSets > 0 || numQueuedInputSets > 0)
            {
                for (var i = 0; i < numQueuedInputSets; i++)
                {
                    var queuedInputSet = _queuedInputSets[i];

                    if (queuedInputSet.PacketNumber == _p2pHandler.FrameCounter)
                    {
                        _receivedInputSets.Add(_queuedInputSets[i]);
                        numReceivedInputSets++;
                        Debug.Log($"removed {_queuedInputSets[i].PacketNumber}");
                        _queuedInputSets.RemoveAt(i);
                        break;
                    }
                }

                ParsedForFrame = false;
                _queuePrediction = true;

                for (var index = 0; index < numReceivedInputSets; index++)
                {
                    var receivedPacketNum = Mod(_receivedInputSets[0].PacketNumber, 600);

                    var numPredictedInputSets = _predictedInputSets.Count;

                    Debug.Log(
                        $"receivedNum: {receivedPacketNum}, numPredicted: {numPredictedInputSets}");

                    if (receivedPacketNum == _p2pHandler.FrameCounter && !ParsedForFrame)
                    {
                        Debug.Log($"parsed: {receivedPacketNum} counter: {_p2pHandler.FrameCounter}");
                        ParseInputs(_receivedInputSets[0]);
                        RollbackManager.Instance.SaveGameState(_receivedInputSets[0].PacketNumber);
                        P2PHandler.Instance.OnInputPacketsProcessed();
                        Debug.Log($"[ARCHIVED-parsed]: {_receivedInputSets[0].PacketNumber} on {_p2pHandler.FrameCounter}");
                        ArchivedInputSets.Add(_receivedInputSets[0]);
                        _receivedInputSets.RemoveAt(0);
                        _queuePrediction = false;
                        ParsedForFrame = true;
                    }
                    else
                    {
                        if (receivedPacketNum > _p2pHandler.FrameCounter ||
                            receivedPacketNum < 250 && _p2pHandler.FrameCounter > 450)
                        {
                            Debug.Log($"queued: {receivedPacketNum} counter: {_p2pHandler.FrameCounter}");
                            _queuedInputSets.Add(_receivedInputSets[0]);
                            _receivedInputSets.RemoveAt(0);
                        }
                        else
                        {
                            if (numPredictedInputSets > 0)
                            {
                                Debug.Log(
                                    $"predicted {_predictedInputSets[0].PacketNumber}, counter: {_p2pHandler.FrameCounter}");

                                var temp = "==RECEIVED==" + Environment.NewLine;
                                var temp2 = "==PREDICTED==" + Environment.NewLine;

                                foreach (var input in _receivedInputSets[0].Inputs)
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
                                if (_receivedInputSets[0].Inputs.SequenceEqual(_predictedInputSets[0].Inputs))
                                {
                                    RollbackManager.Instance.SaveGameState(_receivedInputSets[0].PacketNumber);
                                    P2PHandler.Instance.OnInputPacketsProcessed();
                                    _predictedInputSets.RemoveAt(0);
                                    Debug.Log($"[ARCHIVED-check]: {_receivedInputSets[0].PacketNumber} on {_p2pHandler.FrameCounter}");
                                    ArchivedInputSets.Add(_receivedInputSets[0]);
                                    _receivedInputSets.RemoveAt(0);
                                }
                                else
                                {
                                    var receivedInputSetsCount = _receivedInputSets.Count;
                                    for (var i = 0; i < receivedInputSetsCount; i++)
                                    {
                                        if (_receivedInputSets[0].PacketNumber > _p2pHandler.FrameCounter)
                                            break;

                                        Debug.Log(
                                            $"[ARCHIVED-predict]: {_receivedInputSets[0].PacketNumber} on {_p2pHandler.FrameCounter}");
                                        ArchivedInputSets.Add(_receivedInputSets[0]);
                                        _receivedInputSets.RemoveAt(0);
                                        _predictedInputSets.RemoveAt(0);
                                        --numReceivedInputSets;
                                    }

                                    RollbackManager.Instance.Rollback();
                                }
                            }
                            else
                            {
                                Debug.Log($"parsed2: {receivedPacketNum} counter: {_p2pHandler.FrameCounter}");
                                ParseInputs(_receivedInputSets[0]);
                                RollbackManager.Instance.SaveGameState(_receivedInputSets[0].PacketNumber);
                                P2PHandler.Instance.OnInputPacketsProcessed();
                                Debug.Log($"[ARCHIVED-parsed2]: {_receivedInputSets[0].PacketNumber} on {_p2pHandler.FrameCounter}");
                                ArchivedInputSets.Add(_receivedInputSets[0]);
                                _receivedInputSets.RemoveAt(0);
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
                Debug.Log($"predicted: {predictedInputSet.PacketNumber}");
                _predictedInputSets.Add(predictedInputSet);
                ParseInputs(predictedInputSet);
                _queuePrediction = false;
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer &&
                !P2PHandler.Instance.LatencyCalculated)
                return;

            //if (!P2PHandler.Instance.ReceivedFirstInput) return;

            //HandleInputs();
            HandleInputs();
        }

        private P2PInputSet PredictInputs()
        {
            return new P2PInputSet(new P2PInputSet.InputChange[] { }, Mod(_p2pHandler.FrameCounter, 600));
        }

        public void ParseInputs(P2PInputSet inputSet)
        {
            if (inputSet.Inputs.Length > 0)
            {
                var temp = $"PARSED [{inputSet.PacketNumber}] on {_p2pHandler.FrameCounter} {Environment.NewLine}";
                foreach (var input in inputSet.Inputs)
                {
                    var state = input.State ? "Pressed" : "Released";
                    temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                }

                Debug.Log(temp);
            }

            foreach (var inputChange in inputSet.Inputs) Inputs[(int) inputChange.InputType] = inputChange.State;
        }
    }
}