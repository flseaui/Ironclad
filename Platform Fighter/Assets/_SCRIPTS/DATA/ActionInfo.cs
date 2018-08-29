using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DATA
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ActionInfo
    {
        public enum FrameType
        {
            [UsedImplicitly] Startup,
            [UsedImplicitly] Active,
            [UsedImplicitly] Recovery,
            [UsedImplicitly] Buffer
        }

        [JsonProperty] public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Types.ActionType Type { get; set; }
        
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        private List<FrameType> _frames;

        [JsonProperty] private Vector2 _infinite;

        [JsonProperty] public Vector2 Anchor { get; set; }

        [JsonProperty] public List<List<Box>> Hitboxes { get; set; }
        [JsonProperty] public List<List<Box>> Hurtboxes { get; set; }
        [JsonProperty] public List<List<Box>> Grabboxes { get; set; }
        [JsonProperty] public List<List<Box>> Armorboxes { get; set; }
        [JsonProperty] public List<List<Box>> Collisionboxes { get; set; }
        [JsonProperty] public List<List<Box>> Databoxes { get; set; }

        public ActionInfo()
        {
            _frames = new List<FrameType> {FrameType.Startup};
            _infinite = new Vector2(-1, -1);
            Anchor = new Vector2(0, 0);
        }

        public int FrameCount => _frames.Count;

        public float InfiniteRangeMin
        {
            get => _infinite.x;
            set => _infinite.x = value;
        }

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
            public double Damage { get; }
            public double KnockbackStrength { get; }
            public double KnockbackAngle { get; }
            
            public int Lifespan { get; }
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }
            
            [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
            public BoxType Type { get; }
            
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