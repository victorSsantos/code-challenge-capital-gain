using CapitalGains.Core.Entities.Enums;
using CapitalGains.Core.Entities;
using CapitalGains.Core.Interfaces;

namespace CapitalGains.Worker
{
    public class Worker(ILogger<Worker> logger, IOperationHandler handler, IOperationConsole console) : BackgroundService
    {
        private readonly ILogger<Worker> _logger = logger;
        private readonly IOperationHandler _handler = handler;
        private readonly IOperationConsole _console = console;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Run(() => 
                { 
                    try
                    {
                        Execute();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }, stoppingToken);             
            }
        }

        public void Execute()
        {
            List<List<Operation>> operations = [];

            try
            {
                _console.ReadInput(ref operations);

                Parallel.ForEach(operations, (line) =>
                {
                    var operationBalance = new OperationsBalance();

                    for (var currentOperation = 0; currentOperation < line.Count; currentOperation++)
                    {
                        var operation = line[currentOperation];

                        if (operation.OperationType == OperationType.buy)
                            _handler.HandleBuyOperation(ref operation, ref operationBalance);

                        if (operation.OperationType == OperationType.sell)
                            _handler.HandleSellOperation(ref operation, ref operationBalance);

                        line[currentOperation] = operation;
                    }
                });

                _console.WriteOutput(ref operations);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
