using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PlatformFighter
{
	public class PlayerInputSystem : ComponentSystem
	{

		private List<InputAction> inputActions;

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

			PlayerInput pi;

			Debug.Log(Gamepad.current.name);

			pi.Move = Gamepad.current.leftStick.ReadValue();

			players.Input[i] = pi;
		}

		public void CreateInputActions()
		{
			string actionPath = Path.Combine(Application.streamingAssetsPath, "_ACTIONS/TEST_CHARACTER/");

			foreach (var file in Directory.GetFiles(actionPath).Where(s => s.EndsWith(".json")))
			{
				string jsonData = File.ReadAllText(file);
				Debug.Log($"FILE: {file}");
				ActionInfo action = JsonUtility.FromJson<ActionInfo>(jsonData);
				Debug.Log($"NAME: {action.name}");
			}
			for (int i = 0; i < players.Length; i++)
			{
				
			}
		}

	}
}