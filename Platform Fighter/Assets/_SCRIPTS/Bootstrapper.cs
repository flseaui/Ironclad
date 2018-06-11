using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine.Experimental.Input;

namespace PlatformFighter
{
	public sealed class Bootstrapper
	{
		public static EntityArchetype PlayerArchetype;

		public static GameSettings Settings;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
			
			var entityManager = World.Active.GetOrCreateManager<EntityManager>();

			PlayerArchetype = entityManager.CreateArchetype
			(
				typeof(Position2D), typeof(PlayerInput), typeof(Health), typeof(SpriteRenderer), typeof(TransformMatrix), typeof(Animator)
			);
        }

		public static void NewGame()
		{

			var entityManager = World.Active.GetOrCreateManager<EntityManager>();

			Entity player = entityManager.CreateEntity(PlayerArchetype);

			entityManager.SetComponentData(player, new Position2D { Value = new float2(0.0f, 0.0f) });
			entityManager.SetComponentData(player, new Health { Value = Settings.playerInitialHealth });
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		public static void InitializeWithScene()
		{
			var settingsGO = GameObject.Find("Settings");
			Settings = settingsGO?.GetComponent<GameSettings>();
			if (!Settings)
				return;

			World.Active.GetOrCreateManager<UpdatePlayerHUD>().SetupGameObjects();	

			NewGame();			
		}

		private static MeshInstanceRenderer GetLookFromPrototype(string protoName)
        {
            var proto = GameObject.Find(protoName);
            var result = proto.GetComponent<MeshInstanceRendererComponent>().Value;
            Object.Destroy(proto);
            return result;
        }

	}
}