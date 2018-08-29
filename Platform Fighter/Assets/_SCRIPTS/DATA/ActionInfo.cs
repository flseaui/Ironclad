using System;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DATA
{
    public class ActionInfo
    {
        public enum FrameType
        {
            [UsedImplicitly] Startup,
            [UsedImplicitly] Active,
            [UsedImplicitly] Recovery,
            [UsedImplicitly] Buffer
        }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        private List<FrameType> _frames;

        [JsonProperty] private Vector2 _infinite;

        public Vector2 Anchor;

        public List<List<Box>> Hitboxes, Hurtboxes, Grabboxes, Armorboxes, Collisionboxes, Databoxes;

        public string Name;

        [JsonConverter(typeof(StringEnumConverter))]
        public Types.ActionType Type;

        public ActionInfo()
        {
            _frames = new List<FrameType> {FrameType.Startup};
            _infinite = new Vector2(-1, -1);
            Anchor = new Vector2(0, 0);
        }

        [JsonIgnore]
        public int FrameCount => _frames.Count;

        [JsonIgnore]
        public float InfiniteRangeMin
        {
            get => _infinite.x;
            set => _infinite.x = value;
        }

        [JsonIgnore]
        public float InfiniteRangeMax
        {
            get => _infinite.y;
            set => _infinite.y = value;
        }

        public FrameType FrameTypeAt(int i) => _frames[i];

        public enum BoxType
        {
            [UsedImplicitly] Hit,
            [UsedImplicitly] Hurt,
            [UsedImplicitly] Grab,
            [UsedImplicitly] Armor,
            [UsedImplicitly] Collision,
            [UsedImplicitly] Data
        }
        
        public class Box
        {
            public double Damage, KnockbackStrength, KnockbackAngle;
            public int Lifespan, X, Y, Width, Height;
            [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
            public BoxType Type;
            
            public Box(BoxType type, int x, int y, int width, int height, double damage, double knockbackStrength,
                double knockbackAngle, int lifespan)
            {
                Type = type;
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Damage = damage;
                KnockbackStrength = knockbackStrength;
                KnockbackAngle = knockbackAngle;
                Lifespan = lifespan;
            }

            public Box() : this(BoxType.Hit, 0, 0, 5, 5, 0, 0, 0, 1) { }
        }
    }
}