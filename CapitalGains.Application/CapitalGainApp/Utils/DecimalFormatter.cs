using Newtonsoft.Json;

namespace CapitalGains.Application.CapitalGainApp.Utils;

public class DecimalFormatter : JsonConverter<decimal>
{
    public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        return reader.Value != null ? Convert.ToDecimal(reader.Value) : 0.00M;
    }

    public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
    {
        writer.WriteRawValue(value.ToString("F2"));
    }
}
