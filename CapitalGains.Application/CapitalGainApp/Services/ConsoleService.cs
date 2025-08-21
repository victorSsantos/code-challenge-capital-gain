using CapitalGains.Application.CapitalGainApp.DTOs;
using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Interfaces;
using Newtonsoft.Json;

namespace CapitalGains.Application.CapitalGainApp.Services;

public class ConsoleService : IConsoleService
{
    public List<List<Operation>> ReadIn(CancellationToken cancellationToken = default)
    {
        List<List<Operation>> operationsInput = [];
        string ? line;

        try
        {
            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                cancellationToken.ThrowIfCancellationRequested();
                var operationLine = JsonConvert.DeserializeObject<List<OperationInput>>(line);

                if (operationLine?.Count > 0)
                {
                    var operations = operationLine
                        .Select(x => new Operation(x.operation, x.unitCost, x.quantity))
                        .ToList();

                    operationsInput.Add(operations);
                }
            }  
        }
        catch (JsonException e)
        {
            throw new JsonException($"Error to Deserialize Object: {e.Message}", e);
        }

        return operationsInput;
    }

    public void WriteOut(List<List<Operation>> operations, CancellationToken cancellationToken = default)
    {
        if (operations is null || operations.Count == 0)
            return;

        var writer = Console.Out;

        foreach (var row in operations)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var outputs = row.Select(op => new OperationOutput(op.Tax));
            var json = JsonConvert.SerializeObject(outputs, Formatting.None);

            writer.WriteLine(json);
        }

        writer.Flush();
    }
}