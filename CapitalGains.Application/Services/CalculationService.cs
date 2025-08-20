using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Entities.Enums;
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Application.Services;

public class CalculationService : ICalculationService
{
    private const decimal _percentualTax = 0.2M;
    private const int _minValueToTax = 20000;

    public void CalculateBuyOperation(Operation operation, OperationsBalance operationsBalance)
    {
        operationsBalance.AverageBuyingCost = Calculate(CalculationType.average, operation, operationsBalance);
        operationsBalance.ShareQuantity += operation.Quantity;
    }

    public void CalculateSellOperation(Operation operation, OperationsBalance operationsBalance)
    {
        if (operation.Quantity > operationsBalance.ShareQuantity)
            throw new InvalidOperationException("Invalid operation: The quantity of shares being sold cannot exceed the current quantity of shares owned.");

        operationsBalance.ShareQuantity -= operation.Quantity;
        var gain = Calculate(CalculationType.gain, operation, operationsBalance);

        if (gain < 0)
            operationsBalance.Loss += gain;

        if (gain > 0)
        {
            gain += operationsBalance.Loss;

            if(gain <= 0)
                operationsBalance.Loss = gain;

            if (gain > 0 && operation.UnitCost * operation.Quantity > _minValueToTax)
                operation.Tax = Calculate(CalculationType.tax, operation, operationsBalance);  
        } 
    }

    private static decimal Calculate(CalculationType calc, Operation operation, OperationsBalance balance)
    {
        var calculator = new Dictionary<CalculationType, Func<Operation, OperationsBalance, decimal>>()
        {
            {CalculationType.average, (operation, balance) => (balance.ShareQuantity * balance.AverageBuyingCost + operation.Quantity * operation.UnitCost) / (balance.ShareQuantity + operation.Quantity)} ,
            {CalculationType.gain, (operation, balance) => operation.Quantity * (operation.UnitCost - balance.AverageBuyingCost)},
            {CalculationType.tax, (operation, balance) =>  _percentualTax * operation.Quantity * (operation.UnitCost - balance.AverageBuyingCost)}
        };

        return calculator[calc](operation, balance);
    }
}