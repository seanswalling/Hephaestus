using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hephaestus.CLI
{
    public class GroupingConverter<TKey, TElement> : JsonConverter<IGrouping<TKey, TElement>>
    {
        public override IGrouping<TKey, TElement> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement deserialization logic here
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IGrouping<TKey, TElement> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Key");
            JsonSerializer.Serialize(writer, value.Key, options);
            writer.WritePropertyName("Elements");
            JsonSerializer.Serialize(writer, value.ToList(), options);
            writer.WriteEndObject();
        }
    }
}