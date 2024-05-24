using System;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WhatsPlaying.Converters;

/// <summary>
/// Serializes and Deserializes a <see cref="PointF"/>.
/// </summary>
public class PointFConverter : JsonConverter<PointF>
{
    /// <summary>
    /// Reads a JSON Object as a <see cref="PointF"/>.
    /// </summary>
    /// <returns>The <see cref="PointF"/> created from the JSON Object.</returns>
    public override PointF ReadJson(JsonReader reader, Type objectType, PointF existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject @object = JObject.Load(reader);
        int x = @object.TryGetValue("x", out JToken xVal) ? (int)xVal : 0;
        int y = @object.TryGetValue("y", out JToken yVal) ? (int)yVal : 0;
        return new PointF(x, y);
    }
    /// <summary>
    /// Writes the <see cref="PointF"/> as a JSON Object.
    /// </summary>
    public override void WriteJson(JsonWriter writer, PointF value, JsonSerializer serializer)
    {
        JObject @object = new JObject
        {
            ["x"] = value.X,
            ["y"] = value.Y,
        };
        @object.WriteTo(writer);
    }
}
