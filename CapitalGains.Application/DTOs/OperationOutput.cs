using CapitalGains.Application.Utils;
using Newtonsoft.Json;

namespace CapitalGains.Application.DTOs;

public class OperationOutput(decimal tax)
{
    [JsonProperty("tax")]
    [JsonConverter(typeof(DecimalWithTwoDecimalsConverter))]
    public decimal tax = tax;
}
