using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeSpanConverter : JsonConverter<TimeSpan>
{
    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string timeString = reader.GetString();
        if (TimeSpan.TryParseExact(timeString, @"hh\:mm", null, out TimeSpan time))
        {
            return time;
        }
        throw new JsonException("Invalid time format. Please use 'HH:mm' format.");
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(@"hh\:mm"));
    }
}
