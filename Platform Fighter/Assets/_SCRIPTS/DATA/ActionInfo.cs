using System.Collections.Generic;
using System.Numerics;
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

        public string Name;

        [JsonProperty (ItemConverterType = typeof(StringEnumConverter))]
        private List<FrameType> _frames; 

        [JsonIgnore]
        public int FrameCount => _frames.Count;

        public FrameType FrameTypeAt(int i) => _frames[i];

        [JsonConverter(typeof(StringEnumConverter))]
        public Types.ActionType Type;

        [JsonProperty]
        private Vector2 _infinite;

        [JsonIgnore]
        public float InfiniteRangeMin { get => _infinite.X; set => _infinite.X = value; }
        [JsonIgnore]
        public float InfiniteRangeMax { get => _infinite.Y; set => _infinite.Y = value; }

        public Vector2 Anchor;

        public ActionInfo()
        {
            _frames = new List<FrameType> { FrameType.Startup };
            _infinite = new Vector2(-1, -1);
            Anchor = new Vector2(0, 0);
        }

        public class Box
        {
            public Vector2 KnockbackAngle;
            public double Damage, KnockbackStrength;
            public int Lifespan, X, Y, Width, Height;

            public Box(int x, int y, int width, int height, double damage, double knockbackStrength, Vector2 knockbackAngle, int lifespan)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Damage = damage;
                KnockbackStrength = knockbackStrength;
                KnockbackAngle = knockbackAngle;
                Lifespan = lifespan;
            }

            public Box() : this(0, 0, 5, 5, 0, 0, new Vector2(), 1) { }
        }

        public List<List<Box>> Hitboxes, Hurtboxes, Grabboxes, Armorboxes, Collisionboxes, Databoxes;

    }

}
