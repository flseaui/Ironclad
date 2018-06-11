using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace PlatformFighter
{
	public class PlayerInputSystem : ComponentSystem
	{

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

	}
}