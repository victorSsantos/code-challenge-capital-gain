using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Entities.Enums;
using CapitalGains.Domain.Interfaces;

namespace CapitalGains.Application.CapitalGainApp.Services;

public class CalculationService : ICalculationService
{
    private const decimal _percentualTax = 0.2M;
    private const int _minValueToTax = 20000;

    public void CalculateBuyOperation(Operation operation, OperationsBalance operationsBalance)
    {
        operationsBalance.AverageBuyingCost = CalculateAvg(operation, operationsBalance);
        operationsBalance.TotalQuantity += operation.Quantity;
    }

    public void CalculateSellOperation(Operation operation, OperationsBalance operationsBalance)
    {
        if (operation.Quantity > operationsBalance.TotalQuantity)
            throw new InvalidOperationException("Invalid operation: The quantity of shares being sold cannot exceed the current quantity of shares owned.");

        var gain = (operation.UnitCost - operationsBalance.AverageBuyingCost ) * operation.Quantity;
        var totalOperationCost = operation.UnitCost * operation.Quantity;

        if (gain > 0m) 
        {
            if (operationsBalance.Loss < 0m && totalOperationCost > _minValueToTax)
            {
                var compensation = Math.Min(gain, -operationsBalance.Loss);
                gain -= compensation;             
                operationsBalance.Loss += compensation; 
            }

            if (totalOperationCost > _minValueToTax && gain > 0m)
                operation.Tax = gain * _percentualTax;
            else
                operation.Tax = 0m;
        }
        else 
        {
            operationsBalance.Loss =+ gain;
            operation.Tax = 0m;
        }
            
        operationsBalance.TotalQuantity -= operation.Quantity;
    }

    private static decimal CalculateAvg(Operation operation, OperationsBalance balance)
    {
        return (balance.TotalQuantity * balance.AverageBuyingCost + operation.Quantity * operation.UnitCost) / 
               (balance.TotalQuantity + operation.Quantity);     
    }
}