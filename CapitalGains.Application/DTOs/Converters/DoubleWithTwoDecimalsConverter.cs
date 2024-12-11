using Newtonsoft.Json;

namespace CapitalGains.Application.DTOs.Converters;

public class DoubleWithTwoDecimalsConverter : JsonConverter<double>
{
    public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.Value != null ? Convert.ToDouble(reader.Value) : 0.0;
    }

    public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
    {
        writer.WriteRawValue(value.ToString("F2"));
    }
}
