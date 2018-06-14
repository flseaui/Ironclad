using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace PlatformFighter
{
    public struct PlayerInput : IComponentData
    {
        public float2 Move;
    }

    public struct Health : IComponentData
    {
        public half Value;
    }

}