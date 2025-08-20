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
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (JsonException e)
        {
            throw new JsonException($"Error to Deserialize Object: {e.Message}", e);
        }

        return operationsInput;
    }

    public void WriteOut(List<List<Operation>> operations, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var operatiosOutput = operations
            .SelectMany(row => row.Select(y => new OperationOutput(y.Tax)))
            .ToList();

        if(operatiosOutput.Count == 0)
            return;

        var json = JsonConvert.SerializeObject(operatiosOutput, Formatting.None);

        Console.Out.WriteLine(json);
        Console.Out.Flush();
        
    }
}
