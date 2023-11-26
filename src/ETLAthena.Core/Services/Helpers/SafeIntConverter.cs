using Newtonsoft.Json;

public class SafeIntConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(int) || objectType == typeof(int?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {   
            return objectType == typeof(int) ? (int)0 : (int?)null;
        }

        if (reader.TokenType == JsonToken.Integer)
        {   
            var val = (long)reader.Value;
            if (val >= int.MinValue && val <= int.MaxValue)
                    return (int)val; // Safely cast to int
                throw new JsonSerializationException($"Integer value {val} is too large for an Int32.");
        }

        if (reader.TokenType == JsonToken.String)
        {
            if (int.TryParse(reader.Value.ToString(), out int parsedInt))
                return parsedInt;
            else
                return (int?)null;
        }

        return objectType == typeof(int) ? (int)0 : (int?)null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}