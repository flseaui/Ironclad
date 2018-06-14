using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace PlatformFighter
{
    public struct PlayerInput : IComponentData
    {
        public int lightLeft, strongLeft,
                    lightRight, strongRight,
                    up, down,
                    shortHop, fullHop, 
                    neutral, special, shield, grab,
                    upC, downC, leftC, rightC;
    }

    public struct Health : IComponentData
    {
        public half Value;
    }

}