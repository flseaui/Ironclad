using System;
using System.Collections.Generic;
using System.Linq;
using MANAGERS;
using MISC;
using NETWORKING;
using Rewired;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class NetworkUserInput : InputSender
    {
        private List<InputChange> _changedInputs;

        private List<P2PInputSet> _delayedInputSets;

        private int _jumpFramesHeld;

        private P2PInputSet _lastInputSet;

        private Vector2 _realTimeAngle;
        
        private Player _player;
        public int Id { get; set; }

        private void Start()
        {
            base.Awake();
            _changedInputs = new List<InputChange>();
            _delayedInputSets = new List<P2PInputSet>();
            _player = ReInput.players.GetPlayer(Id);

            _player.controllers.maps.SetMapsEnabled(false, "Menu");
            _player.controllers.maps.SetMapsEnabled(true, "Default");

            Events.OnGameStarted?.Invoke(GetComponent<NetworkIdentity>());
        }

        protected override void PressEvent(int index)
        {
            _changedInputs.Add(new InputChange((Types.Input) index, RealTimeInputs[index]));
        }

        protected override void ReleaseEvent(int index)
        {
            _changedInputs.Add(new InputChange((Types.Input) index, RealTimeInputs[index],
                InputFramesHeld[index]));
        }

        protected override void InputUpdate()
        {
            UpdatePlayerInput();
        }

        private void ApplyDelayedInputSets()
        {
            PlayerData.DataPacket.MovementStickAngle = _delayedInputSets.First().Angle;
            foreach (var input in _delayedInputSets.First().Inputs) Inputs[(int) input.InputType] = input.State;
            _delayedInputSets.RemoveAt(0);

            if (Inputs[(int) Types.Input.Jump])
            {
                if (_jumpFramesHeld == 0 && GetComponent<PlayerFlags>().GetFlagState(Types.Flags.ShortHop) !=
                    Types.FlagState.Pending)
                    GetComponent<PlayerFlags>().SetFlagState(Types.Flags.FullHop, Types.FlagState.Pending);

                if (_jumpFramesHeld < 7 && GetComponent<PlayerFlags>().GetFlagState(Types.Flags.FullHop) !=
                    Types.FlagState.Pending)
                {
                    GetComponent<PlayerFlags>().SetFlagState(Types.Flags.FullHop, Types.FlagState.Resolved);
                    GetComponent<PlayerFlags>().SetFlagState(Types.Flags.ShortHop, Types.FlagState.Pending);
                }

                ++_jumpFramesHeld;
            }
            else
            {
                _jumpFramesHeld = 0;
            }
        }

        private void UpdatePlayerInput()
        {
            for (var index = 0; index < RealTimeInputs.Length; index++) RealTimeInputs[index] = false;

            if (_player.controllers.hasKeyboard)
            {
                if (_player.GetAxis("Run") < -GameSettings.Instance.runThreshold)
                    RealTimeInputs[(int) Types.Input.StrongLeft] = true;
                else if (_player.GetAxis("Move") < 0)
                    RealTimeInputs[(int) Types.Input.LightLeft] = true;

                if (_player.GetAxis("Run") > GameSettings.Instance.runThreshold)
                    RealTimeInputs[(int) Types.Input.StrongRight] = true;
                else if (_player.GetAxis("Move") > 0)
                    RealTimeInputs[(int) Types.Input.LightRight] = true;

                if (_player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    RealTimeInputs[(int) Types.Input.Down] = true;
                else if (_player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    RealTimeInputs[(int) Types.Input.Up] = true;
            }
            else
            {
                if (_player.GetAxis("Move") < -GameSettings.Instance.runThreshold)
                    RealTimeInputs[(int) Types.Input.StrongLeft] = true;
                else if (_player.GetAxis("Move") < 0)
                    RealTimeInputs[(int) Types.Input.LightLeft] = true;

                if (_player.GetAxis("Move") > GameSettings.Instance.runThreshold)
                    RealTimeInputs[(int) Types.Input.StrongRight] = true;
                else if (_player.GetAxis("Move") > 0)
                    RealTimeInputs[(int) Types.Input.LightRight] = true;

                if (_player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    RealTimeInputs[(int) Types.Input.Down] = true;
                else if (_player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    RealTimeInputs[(int) Types.Input.Up] = true;
            }

            if (_player.GetButton("Hop"))
                RealTimeInputs[(int) Types.Input.Jump] = true;

            if (_player.GetButtonDown("Neutral"))
                RealTimeInputs[(int) Types.Input.Neutral] = true;

            if (_player.GetButton("Strong"))
                RealTimeInputs[(int) Types.Input.Strong] = true;

            if (_player.GetButtonDown("Special"))
                RealTimeInputs[(int) Types.Input.Special] = true;

            if (_player.GetButton("Shield"))
                RealTimeInputs[(int) Types.Input.Shield] = true;

            if (_player.GetButtonDown("Grab"))
                RealTimeInputs[(int) Types.Input.Grab] = true;

            _realTimeAngle.x = _player.GetAxis("Move");
            _realTimeAngle.y = _player.GetAxis("Crouch");
        }

        private void FixedUpdate()
        {
            if (TimeManager.Instance.FixedUpdatePaused) return;
            
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && P2PHandler.Instance.AllPlayersReady)
            {
                var inputArray = _changedInputs.ToArray();
                _lastInputSet = new P2PInputSet(inputArray, _realTimeAngle, P2PHandler.Instance.InputPacketsSent, P2PHandler.Instance.InputPacketsSentLoops);

                if (P2PHandler.Instance.Delay != 0)
                {
                    if (_delayedInputSets.Count == P2PHandler.Instance.Delay)
                    {
                        ApplyDelayedInputSets();
                    }
                    Events.OnInputsChanged(GetComponent<NetworkIdentity>(), inputArray, _realTimeAngle, true);
                    _delayedInputSets.Add(_lastInputSet);
                }
                else
                {
                    _delayedInputSets.Add(_lastInputSet);
                    ApplyDelayedInputSets();
                    Events.OnInputsChanged(GetComponent<NetworkIdentity>(), inputArray, _realTimeAngle, true);
                }
                
                ArchivedInputSets.Add(_lastInputSet);
            }

            _changedInputs.Clear();
        }

        public void ApplyLastInputSet()
        {
            ArchivedInputSets.Add(_lastInputSet);
        }
    }
}