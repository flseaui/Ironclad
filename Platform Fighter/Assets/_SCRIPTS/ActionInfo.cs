using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;

namespace PlatformFighter
{
    [Serializable]
    internal class ActionInfo
    {
        enum FrameType { Startup, Active, Recovery, Buffer }
        enum InputType { None, Up, Down, Left, Right, UpC, DownC, LeftC, RightC, Neutral, Special, Grab, Shield, Jump, Taunt }

        private List<FrameType> frames; 
        public int FrameCount { get => frames.Count; }

        private List<InputType> inputs;
        public int InputCount { get => inputs.Count; }

        public string name;

        private bool infinite;

        public ActionInfo()
        {
            frames = new List<FrameType>();
            frames.Add(FrameType.Startup);
            infinite = false;
        }

        private struct Box
        {
            public float2 knockbackAngle;
            public float x, y, width, height, damage, knockbackStrength;

            public Box(float x, float y, float width, float height, float damage, float knockbackStrength, float2 knockbackAngle)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
                this.damage = damage;
                this.knockbackStrength = knockbackStrength;
                this.knockbackAngle = knockbackAngle;
            }

            public Box(Boolean baseBox) : this(0, 0, 5, 5, 0, 0, new float2()) { }
        }
        
        private List<Box> hitboxes, hurtboxes;
    }

}
