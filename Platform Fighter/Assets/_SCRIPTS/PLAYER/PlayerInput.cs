using System.Collections.Generic;
using MANAGERS;
using MISC;
using NETWORKING;
using Rewired;
using UnityEngine;
using Types = DATA.Types;

namespace PLAYER
{
    public class PlayerInput : InputSender
    {
        public int Id { get; set; }

        private List<P2PInputSet.InputChange> _changedInputs;

        private Player _player;
        
        private int _jumpFramesHeld;
        
        private void Start()
        {
            base.Awake();
            _changedInputs = new List<P2PInputSet.InputChange>();
            
            _player = ReInput.players.GetPlayer(Id);
        }

        protected override void PressEvent(int index)
        {
            _changedInputs.Add(new P2PInputSet.InputChange((Types.Input) index, Inputs[index]));
        }

        protected override void ReleaseEvent(int index)
        {
            _changedInputs.Add(new P2PInputSet.InputChange((Types.Input) index, Inputs[index], InputFramesHeld[index]));
        }
        
        protected override void InputUpdate()
        {
            UpdatePlayerInput();
        }

        private void UpdatePlayerInput()
        {                            
            for (var index = 0; index < Inputs.Length; index++)
            {
                Inputs[index] = false;
            }

            if (_player.controllers.hasKeyboard)
            {
                if (_player.GetAxis("Run") < -GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongLeft] = true;
                else if (_player.GetAxis("Move") < 0)
                    Inputs[(int) Types.Input.LightLeft] = true;

                if (_player.GetAxis("Run") > GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongRight] = true;
                else if (_player.GetAxis("Move") > 0)
                    Inputs[(int) Types.Input.LightRight] = true;

                if (_player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    Inputs[(int) Types.Input.Down] = true;
                else if (_player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    Inputs[(int) Types.Input.Up] = true;
            }
            else
            {
                if (_player.GetAxis("Move") < -GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongLeft] = true;
                else if (_player.GetAxis("Move") < 0)
                    Inputs[(int) Types.Input.LightLeft] = true;

                if (_player.GetAxis("Move") > GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongRight] = true;
                else if (_player.GetAxis("Move") > 0)
                    Inputs[(int) Types.Input.LightRight] = true;

                if (_player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    Inputs[(int) Types.Input.Down] = true;
                else if (_player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    Inputs[(int) Types.Input.Up] = true;
            }

            if(_player.GetButton("Hop"))
            {
                if(_jumpFramesHeld == 0 && GetComponent<PlayerFlags>().GetFlagState(Types.Flags.ShortHop) != Types.FlagState.Pending)
                    GetComponent<PlayerFlags>().SetFlagState(Types.Flags.FullHop, Types.FlagState.Pending);
                
                if (_jumpFramesHeld < 7 && GetComponent<PlayerFlags>().GetFlagState(Types.Flags.FullHop) != Types.FlagState.Pending)
                {
                    GetComponent<PlayerFlags>().SetFlagState(Types.Flags.FullHop, Types.FlagState.Resolved);
                    GetComponent<PlayerFlags>().SetFlagState(Types.Flags.ShortHop, Types.FlagState.Pending);
                }
 
                ++_jumpFramesHeld;
            }
            else
                _jumpFramesHeld = 0;

            if (_player.GetButtonDown("Neutral"))
                Inputs[(int) Types.Input.Neutral] = true;
            
            if (_player.GetButton("Strong"))
                Inputs[(int) Types.Input.Strong] = true;

            if (_player.GetButtonDown("Special"))
                Inputs[(int) Types.Input.Special] = true;

            if (_player.GetButton("Shield"))
                Inputs[(int) Types.Input.Shield] = true;

            if (_player.GetButtonDown("Grab"))
                Inputs[(int) Types.Input.Grab] = true;

            PlayerData.DataPacket.MovementStickAngle.x = _player.GetAxis("Move");
            PlayerData.DataPacket.MovementStickAngle.y = _player.GetAxis("Crouch");
            
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer)
                Events.OnInputsChanged(GetComponent<NetworkIdentity>(), _changedInputs.ToArray(), true);
            
            _changedInputs.Clear();
        }
    }
}