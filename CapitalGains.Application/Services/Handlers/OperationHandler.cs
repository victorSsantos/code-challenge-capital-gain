using CapitalGains.Core.Entities;
using CapitalGains.Core.Interfaces;

namespace CapitalGains.Application.Services.Handlers;

public class OperationHandler() : IOperationHandler
{
    private const double _percentualTax = 0.2;
    private const int _minValueToTax = 20000;

    public void HandleBuyOperation(ref Operation operation, ref OperationsBalance operationsBalance)
    {
        operationsBalance.AverageBuyingCost = Calculate("average", operation, operationsBalance); ;
        operationsBalance.ShareQuantity += operation.Quantity;
    }

    public void HandleSellOperation(ref Operation operation, ref OperationsBalance operationsBalance)
    {
        if (operation.Quantity > operationsBalance.ShareQuantity)
            throw new InvalidOperationException("Invalid operation: The quantity of shares being sold cannot exceed the current quantity of shares owned.");

        operationsBalance.ShareQuantity -= operation.Quantity;
        var gain = Calculate("gain", operation, operationsBalance);

        if (gain < 0)
            operationsBalance.Loss += gain;

        if (gain > 0)
        {
            gain += operationsBalance.Loss;

            if(gain <= 0)
                operationsBalance.Loss = gain;

            if (gain > 0 && operation.UnitCost * operation.Quantity > _minValueToTax)
                operation.Tax = Calculate("tax", operation, operationsBalance);  
        } 
    }

    public double Calculate(string calc, Operation operation, OperationsBalance balance)
    {
        var calculator = new Dictionary<string, Func<Operation, OperationsBalance, double>>()
        {
            {"average", (operation, balance) => (balance.ShareQuantity * balance.AverageBuyingCost + operation.Quantity * operation.UnitCost) / (balance.ShareQuantity + operation.Quantity)} ,
            {"gain", (operation, balance) => operation.Quantity * (operation.UnitCost - balance.AverageBuyingCost)},
            {"tax", (operation, balance) =>  _percentualTax * operation.Quantity * (operation.UnitCost - balance.AverageBuyingCost)}
        };

        return calculator[calc](operation, balance);
    }
}
