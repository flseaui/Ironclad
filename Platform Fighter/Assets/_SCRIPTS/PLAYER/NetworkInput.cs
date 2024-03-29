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
            _receivedInputSets.Add(receivedInputs);
        }

        public void HandleInputs()
        {
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
                        _queuedInputSets.RemoveAt(i);
                        break;
                    }
                }

                ParsedForFrame = false;
                _queuePrediction = true;

                for (var index = 0; index < numReceivedInputSets; index++)
                {
                    var receivedPacketNum =_receivedInputSets[0].PacketNumber;

                    var numPredictedInputSets = _predictedInputSets.Count;

                    if (receivedPacketNum == _p2pHandler.FrameCounter && !ParsedForFrame)
                    {
                        ParseInputs(_receivedInputSets[0]);
                        RollbackManager.Instance.SaveGameState(_receivedInputSets[0].PacketNumber, false);
                        ArchivedInputSets.Add(_receivedInputSets[0]);
                        _receivedInputSets.RemoveAt(0);
                        _queuePrediction = false;
                        ParsedForFrame = true;
                    }
                    else
                    {
                        if (receivedPacketNum > _p2pHandler.FrameCounter)
                        {
                            _queuedInputSets.Add(_receivedInputSets[0]);
                            _receivedInputSets.RemoveAt(0);
                        }
                        else
                        {
                            if (numPredictedInputSets > 0)
                            {
                                if (_receivedInputSets[0].Inputs.SequenceEqual(_predictedInputSets[0].Inputs))
                                {
                                    _predictedInputSets.RemoveAt(0);
                                    ArchivedInputSets.Add(_receivedInputSets[0]);
                                    _receivedInputSets.RemoveAt(0);
                                }
                                else
                                {
                                    var receivedInputSetsCount = _receivedInputSets.Count;
                                    var targetPacket = _receivedInputSets[0].PacketNumber;
                                    for (var i = 0; i < receivedInputSetsCount; i++)
                                    {
                                        if (_receivedInputSets[0].PacketNumber > targetPacket)
                                            break;

                                        ArchivedInputSets.Add(_receivedInputSets[0]);
                                        _receivedInputSets.RemoveAt(0);
                                        _predictedInputSets.RemoveAt(0);
                                        --numReceivedInputSets;
                                    }

                                    RollbackManager.Instance.Rollback(targetPacket);
                                }
                            }
                            else
                            {
                                ParseInputs(_receivedInputSets[0]);
                                RollbackManager.Instance.SaveGameState(_receivedInputSets[0].PacketNumber, false);
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
                _predictedInputSets.Add(predictedInputSet);
                ParseInputs(predictedInputSet);
                _queuePrediction = false;
            }
            
            //if (P2PHandler.Instance.DataPacket.FrameCounter % 20 == 0)
                //RollbackManager.Instance.SaveGameState(P2PHandler.Instance.DataPacket.FrameCounter);
        }

        private void FixedUpdate()
        {
            if (TimeManager.Instance.FixedUpdatePaused)
                return;
            if (!P2PHandler.Instance.AllPlayersReady)
                return;
            
            HandleInputs();
        }

        private P2PInputSet PredictInputs()
        {
            return new P2PInputSet(new InputChange[] { }, PlayerData.DataPacket.MovementStickAngle,_p2pHandler.FrameCounter);
        }

        public new void ApplyArchivedInputSet(int index)
        {
            Debug.Log(
                $"networked [player{GetComponent<NetworkIdentity>().Id}] index: {index}, length: {ArchivedInputSets.Count}");
            var temp = Environment.NewLine;

            if (ArchivedInputSets.Count <= index)
            {
                var prediction = PredictInputs();
                if (prediction.Inputs.Length > 0)
                {
                    foreach (var input in prediction.Inputs)
                    {
                        var state = input.State ? "Pressed" : "Released";
                        temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                    }
                }
                Debug.Log("archived prediction");
                PlayerData.DataPacket.MovementStickAngle = prediction.Angle;
                foreach (var input in prediction.Inputs) Inputs[(int) input.InputType] = input.State;
                
                Debug.Log($"predict Applied ({prediction.PacketNumber}) on ({P2PHandler.Instance.DataPacket.FrameCounter}) containing {temp}");
            }
            else
            {
                if (ArchivedInputSets[index].Inputs.Length > 0)
                {
                    foreach (var input in ArchivedInputSets[index].Inputs)
                    {
                        var state = input.State ? "Pressed" : "Released";
                        temp += $"[{input.InputType}]->{state}{Environment.NewLine}";
                    }
                }
                PlayerData.DataPacket.MovementStickAngle = ArchivedInputSets[index].Angle;
                foreach (var input in ArchivedInputSets[index].Inputs) Inputs[(int) input.InputType] = input.State;

                Debug.Log($"Applied ({ArchivedInputSets[index].PacketNumber}) on ({P2PHandler.Instance.DataPacket.FrameCounter}) containing {temp}");
            }
        }
        
        public void ParseInputs(P2PInputSet inputSet)
        {
            PlayerData.DataPacket.MovementStickAngle = inputSet.Angle;
            foreach (var inputChange in inputSet.Inputs) Inputs[(int) inputChange.InputType] = inputChange.State;
        }
    }
}