using System;
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
        
        private bool[] _prevInputs;
        
        private void Start()
        {
            _changedInputs = new List<P2PInputSet.InputChange>();
            _prevInputs = new bool[Inputs.Length];
            
            _player = ReInput.players.GetPlayer(Id);
        }

        private void Update()
        {
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer && !MatchStateManager.Instance.ReadyToFight)
                return;
                
            UpdatePlayerInput();
        }

        private void UpdatePlayerInput()
        {
            _changedInputs.Clear();

            Inputs.CopyTo(_prevInputs, 0);
                
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

            if (_player.GetButtonLongPressDown("Hop"))
                Inputs[(int) Types.Input.FullHop] = true;
            else if (_player.GetButtonShortPressDown("Hop"))
                Inputs[(int) Types.Input.ShortHop] = true;

            if (_player.GetButtonDown("Neutral"))
                Inputs[(int) Types.Input.Neutral] = true;
            
            if (_player.GetButtonDown("Strong"))
                Inputs[(int) Types.Input.Strong] = true;

            if (_player.GetButtonDown("Special"))
                Inputs[(int) Types.Input.Special] = true;

            if (_player.GetButtonDown("Shield"))
                Inputs[(int) Types.Input.Shield] = true;

            if (_player.GetButtonDown("Grab"))
                Inputs[(int) Types.Input.Grab] = true;

            PlayerData.DataPacket.MovementStickAngle.x = _player.GetAxis("Move");
            PlayerData.DataPacket.MovementStickAngle.y = _player.GetAxis("Crouch");
            
            for (var index = 0; index < Inputs.Length; index++)
            {
                if (Inputs[index] != _prevInputs[index])
                {
                    _changedInputs.Add(new P2PInputSet.InputChange((Types.Input) index, Inputs[index]));
                }
            }
   
            if (GameManager.Instance.MatchType == Types.MatchType.OnlineMultiplayer)
                Events.OnInputsChanged(GetComponent<NetworkIdentity>(), _changedInputs.ToArray(), true);
        }
    }
}