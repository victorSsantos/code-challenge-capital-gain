using CapitalGains.Application.CapitalGainApp.Utils;
using Newtonsoft.Json;

namespace CapitalGains.Application.CapitalGainApp.DTOs;

public class OperationOutput(decimal tax)
{
    [JsonProperty("tax")]
    [JsonConverter(typeof(DecimalFormatter))]
    public decimal tax = tax;
}
