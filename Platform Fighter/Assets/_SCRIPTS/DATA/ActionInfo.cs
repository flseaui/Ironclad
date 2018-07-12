using System;
using System.Collections.Generic;
using UnityEngine;

namespace DATA
{
    [Serializable]
    public class ActionInfo
    {
        enum FrameType { Startup, Active, Recovery, Buffer }
        enum InputType { None, Up, Down, Left, Right, UpC, DownC, LeftC, RightC, Neutral, Special, Grab, Shield, Jump, Taunt }

        private List<FrameType> frames;

        public int FrameCount { get => frames.Count; }

        private List<InputType> inputs;
        public int[] Inputs { get => System.Array.ConvertAll(inputs.ToArray(), value => (int)value); }

        public string name;

        public Types.ActionType type;

        private bool infinite;

        public ActionInfo()
        {
            frames = new List<FrameType>();
            frames.Add(FrameType.Startup);
            infinite = false;
        }

        private struct Box
        {
            public Vector2 knockbackAngle;
            public float x, y, width, height, damage, knockbackStrength;

            public Box(float x, float y, float width, float height, float damage, float knockbackStrength, Vector2 knockbackAngle)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
                this.damage = damage;
                this.knockbackStrength = knockbackStrength;
                this.knockbackAngle = knockbackAngle;
            }

            public Box(Boolean baseBox) : this(0, 0, 5, 5, 0, 0, new Vector2()) { }
        }

        private List<Box> hitboxes, hurtboxes;
    }
}