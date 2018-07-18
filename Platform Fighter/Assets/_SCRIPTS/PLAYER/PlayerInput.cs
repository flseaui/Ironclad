using MISC;
using Rewired;
using UnityEngine;

namespace PLAYER
{
    public class PlayerInput : MonoBehaviour
    {
        [HideInInspector] public bool lightLeft,
            strongLeft,
            lightRight,
            strongRight,
            up,
            down,
            shortHop,
            fullHop,
            neutral,
            special,
            shield,
            grab,
            upC,
            downC,
            leftC,
            rightC;

        public int Id { get; set; }

        private void Update()
        {
            UpdatePlayerInput();
        }

        private void UpdatePlayerInput()
        {
            var player = ReInput.players.GetPlayer(Id);

            lightLeft = false;
            strongLeft = false;
            lightRight = false;
            strongRight = false;
            up = false;
            down = false;
            shortHop = false;
            fullHop = false;
            neutral = false;
            special = false;
            shield = false;
            grab = false;
            upC = false;
            downC = false;
            leftC = false;
            rightC = false;

            if (player.controllers.hasKeyboard)
            {
                if (player.GetAxis("Run") < -GameSettings.Instance.runThreshold)
                    strongLeft = true;
                else if (player.GetAxis("Move") < 0)
                    lightLeft = true;

                if (player.GetAxis("Run") > GameSettings.Instance.runThreshold)
                    strongRight = true;
                else if (player.GetAxis("Move") > 0)
                    lightRight = true;

                if (player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    down = true;
                else if (player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    up = true;
            }
            else
            {
                if (player.GetAxis("Move") < -GameSettings.Instance.runThreshold)
                    strongLeft = true;
                else if (player.GetAxis("Move") < 0)
                    lightLeft = true;

                if (player.GetAxis("Move") > GameSettings.Instance.runThreshold)
                    strongRight = true;
                else if (player.GetAxis("Move") > 0)
                    lightRight = true;

                if (player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
                    down = true;
                else if (player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
                    up = true;
            }

            if (player.GetButtonLongPressDown("Hop"))
                fullHop = true;
            else if (player.GetButtonShortPressDown("Hop"))
                shortHop = true;

            if (player.GetButtonDown("Neutral"))
                neutral = true;

            if (player.GetButtonDown("Special"))
                special = true;

            if (player.GetButtonDown("Shield"))
                shield = true;

            if (player.GetButtonDown("Grab"))
                grab = true;
        }
    }
}