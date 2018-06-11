using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms2D;

namespace PlatformFighter
{
    public class PlayerMoveSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentDataArray<Position2D> Position;
            public ComponentDataArray<PlayerInput> Input;
        }

        [Inject] private Data data;

        protected override void OnUpdate()
        {
            float dt = Time.deltaTime;
            for (int index = 0; index < data.Length; ++index)
            {
                var position = data.Position[index].Value;
                
                var playerInput = data.Input[index];

                position += dt * playerInput.Move;

                data.Position[index] = new Position2D { Value = position };
                data.Input[index] = playerInput;
            }
        }
    }
}