using CapitalGains.Application.DTOs;
using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Interfaces;
using Newtonsoft.Json;

namespace CapitalGains.Application.Services;

public class ConsoleService : IConsoleService
{
    public void ReadInput(List<List<Operation>> operationsInput)
    {
        string? line;

        try
        {
            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                var operationLine = JsonConvert.DeserializeObject<List<OperationInput>>(line);

                if (operationLine?.Count > 0)
                {
                    var operations = operationLine.Select(x => new Operation(x.operation, x.unitCost, x.quantity)).ToList();
                    operationsInput.Add(operations);
                }                  
            }
        }
        catch (JsonException e)
        {
            throw new JsonException($"Error to Deserialize Object: {e.Message}", e);
        }
    }

    public void WriteOutput(List<List<Operation>> operationsOutput)
    {
        if (operationsOutput.Any())
        {
            var serializedOperations = operationsOutput
                .Select(x => JsonConvert.SerializeObject(x.Select(y => new OperationOutput(y.Tax))));

            Console.WriteLine(string.Join(Environment.NewLine, serializedOperations));
        }
    }
}
