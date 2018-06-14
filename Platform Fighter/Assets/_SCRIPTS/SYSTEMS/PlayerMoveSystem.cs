using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms2D;

namespace PlatformFighter
{
    public class PlayerMoveSystem : ComponentSystem
    {
        public struct PlayerData
        {
            public int Length;
            public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<PlayerInput> Input;
        }

        [Inject] private PlayerData players;

        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < players.Length; ++i)
            {
                PlayerInput input = players.Input[i];
                // Action Debug 
                if (input.lightLeft == 1)
                    Debug.Log("Walking Left");
                else if (input.strongLeft == 1)
                    Debug.Log("Running Left");

                if (input.lightRight == 1)
                    Debug.Log("Walking Right");
                else if (input.strongRight == 1)
                    Debug.Log("Running Right");
                 

            }
        }
    }
}