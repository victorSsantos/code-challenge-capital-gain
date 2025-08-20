using CapitalGains.Domain.Entities.Enums;
using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Presentation;

public class Client(ICalculationService calculator, IConsoleService console)
{
    private readonly ICalculationService _calculator = calculator;
    private readonly IConsoleService _console = console;

    public void ProcessOperations(CancellationToken cancellationToken = default)
    {
        List<List<Operation>> operations = [];

        _console.ReadInput(operations);
        
        var po = new ParallelOptions
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        
        Parallel.ForEach(operations, po, line =>
        {
            var balance = new OperationsBalance();

            for (int i = 0; i < line.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var op = line[i];
        
                switch (op.OperationType)
                {
                    case OperationType.buy:
                        _calculator.CalculateBuyOperation(op, balance);
                        break;
                    case OperationType.sell:
                        _calculator.CalculateSellOperation(op, balance);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported operation type: {op.OperationType}");
                }
            }
        });
        
        _console.WriteOutput(operations);
    }
}
