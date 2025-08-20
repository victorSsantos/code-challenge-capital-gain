using CapitalGains.Application.CapitalGainApp.Interfaces;
using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Entities.Enums;
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Application.CapitalGainApp;

public class CapitalGainApp(ICalculationService calculator, IConsoleService console) : ICapitalGainApp
{
    private readonly ICalculationService _calculator = calculator;
    private readonly IConsoleService _console = console;

    public void Process(CancellationToken cancelToken = default)
    {
        var operations = _console.ReadIn();

        if (operations is null || operations.Count == 0)
            return;

        var options = new ParallelOptions
        {
            CancellationToken = cancelToken,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        Parallel.ForEach(operations, options, row =>
        {
            var balance = new OperationsBalance();

            foreach (var operation in row)
            {
                options.CancellationToken.ThrowIfCancellationRequested();

                switch (operation.OperationType)
                {
                    case OperationType.buy:
                        _calculator.CalculateBuyOperation(operation, balance);
                        break;
                    case OperationType.sell:
                        _calculator.CalculateSellOperation(operation, balance);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported operation type: {operation.OperationType}");
                }
            }
        });

        cancelToken.ThrowIfCancellationRequested();

        if (operations.Any())
            _console.WriteOut(operations);
    }
}
