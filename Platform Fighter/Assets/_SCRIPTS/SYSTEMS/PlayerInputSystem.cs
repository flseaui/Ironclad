using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rewired;

namespace PlatformFighter
{
	public class PlayerInputSystem : ComponentSystem
	{
		
		private List<Player> playerControllers = new List<Player>();

		struct PlayerData
		{
			public int Length;

			public ComponentDataArray<PlayerInput> Input;
		}

		[Inject] private PlayerData players;

		protected override void OnUpdate()
		{
			float dt = Time.deltaTime;

			for (int i = 0; i < players.Length; i++)
			{
				UpdatePlayerInput(i, dt);
			}
		}

		private void UpdatePlayerInput(int i, float dt)
		{
			var settings = Bootstrapper.Settings;
			var player = playerControllers[i];

			PlayerInput pi;

			pi.lightLeft = 0;
			pi.strongLeft = 0;
			pi.lightRight = 0;
			pi.strongRight = 0;
			pi.up = 0;
			pi.down = 0;
			pi.shortHop = 0;
			pi.fullHop = 0;
			pi.neutral = 0;
			pi.special = 0;
			pi.shield = 0;
			pi.grab = 0;
			pi.upC = 0;
			pi.downC = 0;
			pi.leftC = 0;
			pi.rightC = 0;

			if(player.GetAxis("Move") < -.65)
				pi.strongLeft = 1;
			else if (player.GetAxis("Move") < 0)
				pi.lightLeft = 1;

			if(player.GetAxis("Move") > .65)
				pi.strongRight = 1;
			else if (player.GetAxis("Move") > 0)
				pi.lightRight = 1;

			if (player.GetAxis("Crouch") < -.4)
				pi.down = 1;
			else if (player.GetAxis("Crouch") > .2)
				pi.up = 1;

			if (player.GetButtonLongPressDown("Hop"))
				pi.fullHop = 1;
			else if (player.GetButtonShortPressDown("Hop"))
				pi.shortHop = 1;
				
			if (player.GetButtonDown("Neutral"))
				pi.neutral = 1;

			if (player.GetButtonDown("Special"))
				pi.special = 1;

			if (player.GetButtonDown("Shield"))
				pi.shield = 1;

			if (player.GetButtonDown("Grab"))
				pi.grab = 1;

			players.Input[i] = pi;
		}

		public void CreateInputActions()
		{
			playerControllers.Add(ReInput.players.GetPlayer(0));
			foreach(var action in LoadActions())
			{

			}
		}

		private void LogAction(ActionInfo action)
		{
			Debug.Log($"ACTION: { action.name }");
		}

		// reads in all of a characters actions and returns a list of them
		private List<ActionInfo> LoadActions(string character = "TEST_CHARACTER")
		{
			var actions = new List<ActionInfo>();

			var actionPath = Path.Combine(Application.streamingAssetsPath, $"_ACTIONS/{ character }/");

			if (!Directory.Exists(actionPath))
				throw new DirectoryNotFoundException($"INVALID CHARACTER DIRECTORY { actionPath }");

			foreach (var file in Directory.GetFiles(actionPath).Where(s => s.EndsWith(".json")))
			{
				Debug.Log($"READ FILE: { file }");

				var jsonData = File.ReadAllText(file);
				var action = JsonUtility.FromJson<ActionInfo>(jsonData);

				actions.Add(action);
			}

			return actions;
		}

	}
}