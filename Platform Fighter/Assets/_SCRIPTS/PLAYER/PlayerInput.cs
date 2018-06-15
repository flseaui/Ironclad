using UnityEngine;
using System.Collections.Generic;
using Rewired;

namespace PlatformFighter
{
	public class PlayerInput : MonoBehaviour
	{
		private List<Player> playerControllers = new List<Player>();

		[HideInInspector]
        public bool lightLeft, strongLeft,
                    lightRight, strongRight,
                    up, down,
                    shortHop, fullHop, 
                    neutral, special, shield, grab,
                    upC, downC, leftC, rightC;

		private void Start()
		{
			SetupControllers();
		}

		private void Update()
		{
			UpdatePlayerInput(0);
		}

		public void SetupControllers()
		{
			playerControllers.Add(ReInput.players.GetPlayer(0));
		}

		private void UpdatePlayerInput(int i)
		{
			var player = playerControllers[i];

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

			if(player.GetAxis("Move") < -GameSettings.Instance.runThreshold)
				strongLeft = true;
			else if (player.GetAxis("Move") < 0)
				lightLeft = true;

			if(player.GetAxis("Move") > GameSettings.Instance.runThreshold)
				strongRight = true;
			else if (player.GetAxis("Move") > 0)
				lightRight = true;

			if (player.GetAxis("Crouch") < GameSettings.Instance.crouchThreshold)
				down = true;
			else if (player.GetAxis("Crouch") > GameSettings.Instance.upThreshold)
				up = true;

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