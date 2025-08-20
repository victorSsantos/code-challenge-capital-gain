using CapitalGains.Domain.Entities.Enums;
using Newtonsoft.Json;

namespace CapitalGains.Application.CapitalGainApp.DTOs;

public class OperationInput
{
    [JsonProperty("operation")]
    public OperationType operation;

    [JsonProperty("unit-cost")]
    public decimal unitCost;

    [JsonProperty("quantity")]
    public int quantity;
}
