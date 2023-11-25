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
            return objectType == typeof(int) ? -1 : (int?)null;

        if (reader.TokenType == JsonToken.Integer)
            return reader.Value;

        if (reader.TokenType == JsonToken.String)
        {
            if (int.TryParse(reader.Value.ToString(), out int parsedInt))
                return parsedInt;
        }

        return objectType == typeof(int) ? -1 : (int?)null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, value);
    }
}