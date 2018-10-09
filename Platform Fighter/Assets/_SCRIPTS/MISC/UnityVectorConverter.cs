using System;
using Newtonsoft.Json;
using UnityEngine;

namespace MISC
{
    public class UnityVectorConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return serializer.Deserialize<Vector2>(reader);
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(Vector2);
    }
}