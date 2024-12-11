using CapitalGains.Core.Entities;

namespace CapitalGains.Core.Interfaces;

public interface IOperationHandler 
{
    void HandleBuyOperation(ref Operation operation, ref OperationsBalance operationsBalance);
    void HandleSellOperation(ref Operation operation, ref OperationsBalance operationsBalance);
    double Calculate(string calc, Operation operation, OperationsBalance operationsBalance);

}
