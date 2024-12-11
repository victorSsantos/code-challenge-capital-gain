using CapitalGains.Core.Entities.Enums;
using Newtonsoft.Json;

namespace CapitalGains.Application.DTOs;

public class OperationInput
{
    [JsonProperty("operation")]
    public OperationType operation;

    [JsonProperty("unit-cost")]
    public double unitCost;

    [JsonProperty("quantity")]
    public int quantity;
}
