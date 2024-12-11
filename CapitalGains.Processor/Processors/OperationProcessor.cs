using CapitalGains.Core.Entities.Enums;
using CapitalGains.Core.Interfaces;
using CapitalGains.Core.Entities;

namespace CapitalGains.Processor.Processors 
{
    public class OperationProcessor(IOperationHandler handler, IOperationConsole console) : IOperationProcessor
    {
        private readonly IOperationHandler _handler = handler;
        private readonly IOperationConsole _console = console;

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