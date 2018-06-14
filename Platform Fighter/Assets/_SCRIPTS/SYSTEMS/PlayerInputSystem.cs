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
			Debug.Log(playerControllers[i].GetAxis("Walk"));
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