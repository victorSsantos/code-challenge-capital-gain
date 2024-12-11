using CapitalGains.Application.DTOs.Converters;
using Newtonsoft.Json;

namespace CapitalGains.Application.DTOs;

public class OperationOutput(double tax)
{
    [JsonProperty("tax")]
    [JsonConverter(typeof(DoubleWithTwoDecimalsConverter))]
    public double tax = tax;
}
