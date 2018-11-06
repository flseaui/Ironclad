using System;
using System.Collections.Generic;
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

        private bool[] _prevInputs;
        
        private void Start()
        {
            _changedInputs = new List<P2PInputSet.InputChange>();
            _prevInputs = new bool[Inputs.Length];
        }

        private void Update()
        {
            UpdatePlayerInput();
        }

        private void UpdatePlayerInput()
        {
            var player = ReInput.players.GetPlayer(Id);

            _changedInputs.Clear();

            Inputs.CopyTo(_prevInputs, 0);
                
            for (var index = 0; index < Inputs.Length; index++)
            {
                Inputs[index] = false;
            }

            if (player.controllers.hasKeyboard)
            {
                if (player.GetAxis("Run") < -GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongLeft] = true;
                else if (player.GetAxis("Move") < 0)
                    Inputs[(int) Types.Input.LightLeft] = true;

                if (player.GetAxis("Run") > GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongRight] = true;
                else if (player.GetAxis("Move") > 0)
                    Inputs[(int) Types.Input.LightRight] = true;

                if (player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    Inputs[(int) Types.Input.Down] = true;
                else if (player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    Inputs[(int) Types.Input.Up] = true;
            }
            else
            {
                if (player.GetAxis("Move") < -GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongLeft] = true;
                else if (player.GetAxis("Move") < 0)
                    Inputs[(int) Types.Input.LightLeft] = true;

                if (player.GetAxis("Move") > GameSettings.Instance.runThreshold)
                    Inputs[(int) Types.Input.StrongRight] = true;
                else if (player.GetAxis("Move") > 0)
                    Inputs[(int) Types.Input.LightRight] = true;

                if (player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    Inputs[(int) Types.Input.Down] = true;
                else if (player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    Inputs[(int) Types.Input.Up] = true;
            }

            if (player.GetButtonLongPressDown("Hop"))
                Inputs[(int) Types.Input.FullHop] = true;
            else if (player.GetButtonShortPressDown("Hop"))
                Inputs[(int) Types.Input.ShortHop] = true;

            if (player.GetButtonDown("Neutral"))
                Inputs[(int) Types.Input.Neutral] = true;
            
            if (player.GetButtonDown("Strong"))
                Inputs[(int) Types.Input.Strong] = true;

            if (player.GetButtonDown("Special"))
                Inputs[(int) Types.Input.Special] = true;

            if (player.GetButtonDown("Shield"))
                Inputs[(int) Types.Input.Shield] = true;

            if (player.GetButtonDown("Grab"))
                Inputs[(int) Types.Input.Grab] = true;

            for (var index = 0; index < Inputs.Length; index++)
            {
                if (Inputs[index] != _prevInputs[index])
                {
                    _changedInputs.Add(new P2PInputSet.InputChange((Types.Input) index, Inputs[index]));
                }
            }

            Events.OnInputsChanged(GetComponent<NetworkIdentity>(), _changedInputs.ToArray(), true);
        }
    }
}