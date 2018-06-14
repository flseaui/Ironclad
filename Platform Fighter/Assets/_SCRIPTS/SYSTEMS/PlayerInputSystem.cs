using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.Experimental.Input.Interactions;

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

			//Debug.Log(Gamepad.current.name);

			pi.Move = Gamepad.current.leftStick.ReadValue();

			players.Input[i] = pi;
		}

		public void CreateInputActions()
		{

			foreach(var action in LoadActions())
			{
				switch (action.name)
				{
					case "Idle":
						Bootstrapper.Settings.controls.XInputGamepad.Idle.performed += _ => LogAction(action);
					break;
					case "WalkLeft":
						Bootstrapper.Settings.controls.XInputGamepad.WalkLeft.performed += _ => LogAction(action);
					break;
					case "WalkRight":
						Bootstrapper.Settings.controls.XInputGamepad.WalkRight.performed += _ => LogAction(action);
					case "RunLeft":
						Bootstrapper.Settings.controls.XInputGamepad.RunLeft.performed += _ => LogAction(action);
					break;
					case "RunRight":
						Bootstrapper.Settings.controls.XInputGamepad.RunRight.performed += _ => LogAction(action);
					break;
					case "ShortHop":
						Bootstrapper.Settings.controls.XInputGamepad.ShortHop.performed += _ => LogAction(action);
					break;
					case "Jump":
						Bootstrapper.Settings.controls.XInputGamepad.Jump.performed += _ => LogAction(action);
					break;
					case "Crouch":
						Bootstrapper.Settings.controls.XInputGamepad.Crouch.performed += _ => LogAction(action);
					break;
					case "Jab":
						Bootstrapper.Settings.controls.XInputGamepad.Jab.performed += _ => LogAction(action);
					break;
					case "NeutralSpecial":
						Bootstrapper.Settings.controls.XInputGamepad.NeutralSpecial.performed += _ => LogAction(action);
					break;
				}
				
				Bootstrapper.Settings.controls.Enable();

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