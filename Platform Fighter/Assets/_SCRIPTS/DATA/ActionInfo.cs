using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MISC;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TOOLS;
using UnityEngine;

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

        [JsonProperty] public string Name { get; private set; }
        
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public Types.ActionType Type { get; private set; }
        
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        private List<FrameType> _frames;

        [JsonConverter(typeof(UnityVectorConverter))]
        [JsonProperty] private Vector2 _infinite;

        [JsonConverter(typeof(UnityVectorConverter))]
        [JsonProperty] public Vector2 Anchor { get; private set; }

        [JsonProperty] public List<List<Box>> Hitboxes { get; private set; }
        [JsonProperty] public List<List<Box>> Hurtboxes { get; private set; }
        [JsonProperty] public List<List<Box>> Grabboxes { get; private set; }
        [JsonProperty] public List<List<Box>> Armorboxes { get; private set; }
        [JsonProperty] public List<List<Box>> Collisionboxes { get; private set; }
        [JsonProperty] public List<List<Box>> Databoxes { get; private set; }
        
        [JsonProperty] public List<FrameProperty> FrameProperties { get; private set; }

        public List<List<Box>> AllBoxes => CollectionTools.Concat(Hitboxes, Hurtboxes, Grabboxes, Armorboxes, Collisionboxes, Databoxes).ToList();

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
        
        public class Box
        {   
            public enum BoxType
            {
                [UsedImplicitly] Hit,
                [UsedImplicitly] Hurt,
                [UsedImplicitly] Grab,
                [UsedImplicitly] Armor,
                [UsedImplicitly] Collision,
                [UsedImplicitly] Data,
                [UsedImplicitly] Null
            }
     
            [JsonProperty] public double Damage { get; private set; }
            [JsonProperty] public double KnockbackStrength { get; private set; }
            [JsonProperty] public double KnockbackAngle { get; private set; }
            
            [JsonProperty] public int Lifespan { get; private set; }
            [JsonProperty] public int X { get; private set; }
            [JsonProperty] public int Y { get; private set; }
            [JsonProperty] public int Width { get; private set; }
            [JsonProperty] public int Height { get; private set; }
            
            [JsonConverter(typeof(StringEnumConverter))]
            [JsonProperty] public BoxType Type { get; private set; }
            
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

        public class FrameProperty
        {
            [JsonConverter(typeof(UnityVectorConverter))]
            [JsonProperty] public Vector2 Velocity { get; private set; }

            public FrameProperty(Vector2 velocity)
            {
                Velocity = velocity;
            }

            public FrameProperty()
            {
                Velocity = new Vector2(0, 0);
            }
            
        }
    }
}