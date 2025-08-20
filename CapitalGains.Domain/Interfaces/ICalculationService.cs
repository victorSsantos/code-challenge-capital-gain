using CapitalGains.Domain.Entities;

namespace CapitalGains.Domain.Interfaces;

public interface ICalculationService 
{
    void CalculateBuyOperation(Operation operation, OperationsBalance operationsBalance);
    void CalculateSellOperation(Operation operation, OperationsBalance operationsBalance);

}
