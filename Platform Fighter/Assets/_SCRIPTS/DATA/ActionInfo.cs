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

        [JsonIgnore] public int FrameCount => _frames.Count;

        [JsonIgnore]
        public float InfiniteRangeMin
        {
            get => _infinite.X;
            set => _infinite.X = value;
        }

        [JsonIgnore]
        public float InfiniteRangeMax
        {
            get => _infinite.Y;
            set => _infinite.Y = value;
        }

        public FrameType FrameTypeAt(int i)
        {
            return _frames[i];
        }

        public class Box
        {
            public double Damage, KnockbackStrength;
            public Vector2 KnockbackAngle;
            public int Lifespan, X, Y, Width, Height;

            public Box(int x, int y, int width, int height, double damage, double knockbackStrength,
                Vector2 knockbackAngle, int lifespan)
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

            public Box() : this(0, 0, 5, 5, 0, 0, new Vector2(), 1)
            {
            }
        }
    }
}