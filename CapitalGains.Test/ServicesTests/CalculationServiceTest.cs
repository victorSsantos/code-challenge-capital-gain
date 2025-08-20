using CapitalGains.Application.CapitalGainApp.Services;
using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Entities.Enums;

namespace CapitalGains.Test.ServicesTests;

public class CalculationServiceTest
{
    private readonly CalculationService _handler = new();

    [Fact]
    public void CalculateBuyOperation_ShouldCalculateWithSuccess_WhenValidData()
    {
        // Arrange
        Operation operation =  new(OperationType.buy, 10.0M, 100);
        OperationsBalance balance = new();

        OperationsBalance balanceExpected = new() { AverageBuyingCost = 10.0M, TotalQuantity = 100};
        Operation operationExpected = new(OperationType.buy, 10.0M, 100, 0.0M);           

        // Act
        _handler.CalculateBuyOperation(operation, balance);

        // Assert
        Assert.Equal(operationExpected.OperationType, operation.OperationType);
        Assert.Equal(operationExpected.Quantity, operation.Quantity);
        Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
        Assert.Equal(operationExpected.Tax, operation.Tax);
        Assert.Equal(balanceExpected.Loss, balance.Loss); 
        Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
        Assert.Equal(balanceExpected.TotalQuantity, balance.TotalQuantity);
    }

    [Fact]
    public void CalculateSellOperation_ShouldCalculateWithSuccess_WhenLoss()
    {
        // Arrange
        Operation operation = new(OperationType.sell, 5.0M, 50);
        OperationsBalance balance = new() { Loss = 0.0M, AverageBuyingCost = 10.0M, TotalQuantity = 100 };

        Operation operationExpected = new(OperationType.sell, 5.0M, 50, 0.0M);
        OperationsBalance balanceExpected = new() { Loss = -250.0M, AverageBuyingCost = 10.0M, TotalQuantity = 50 };

        // Act
        _handler.CalculateSellOperation(operation, balance);

        // Assert
        Assert.Equal(operationExpected.OperationType, operation.OperationType);
        Assert.Equal(operationExpected.Quantity, operation.Quantity);
        Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
        Assert.Equal(operationExpected.Tax, operation.Tax);
        Assert.Equal(balanceExpected.Loss, balance.Loss);
        Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
        Assert.Equal(balanceExpected.TotalQuantity, balance.TotalQuantity);
    }

    [Fact]
    public void CalculateSellOperation_ShouldCalculateWithSuccess_WhenGain()
    {
        // Arrange
        Operation operation = new(OperationType.sell, 25.0M, 1000);
        OperationsBalance balance = new() { Loss = 0.0M, AverageBuyingCost = 10.0M, TotalQuantity = 2000 };

        Operation operationExpected = new(OperationType.sell, 25.0M, 1000, 3000);
        OperationsBalance balanceExpected = new() { Loss = 0.0M, AverageBuyingCost = 10.0M, TotalQuantity = 1000 };

        // Act
        _handler.CalculateSellOperation(operation, balance);

        // Assert
        Assert.Equal(operationExpected.OperationType, operation.OperationType);
        Assert.Equal(operationExpected.Quantity, operation.Quantity);
        Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
        Assert.Equal(operationExpected.Tax, operation.Tax);
        Assert.Equal(balanceExpected.Loss, balance.Loss);
        Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
        Assert.Equal(balanceExpected.TotalQuantity, balance.TotalQuantity);
    }

    [Fact]
    public void CalculateSellOperation_ShouldCalculateWithSuccess_WhenGainWithLossBefore()
    {
        // Arrange
        Operation operation = new(OperationType.sell, 25.0M, 1000);
        OperationsBalance balance = new() { Loss = -20000.0M, AverageBuyingCost = 10.0M, TotalQuantity = 2000 };

        Operation operationExpected = new(OperationType.sell, 25.0M, 1000, 0.0M);
        OperationsBalance balanceExpected = new() { Loss = -5000.0M, AverageBuyingCost = 10.0M, TotalQuantity = 1000 };

        // Act
        _handler.CalculateSellOperation(operation, balance);

        // Assert
        Assert.Equal(operationExpected.OperationType, operation.OperationType);
        Assert.Equal(operationExpected.Quantity, operation.Quantity);
        Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
        Assert.Equal(operationExpected.Tax, operation.Tax);
        Assert.Equal(balanceExpected.Loss, balance.Loss);
        Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
        Assert.Equal(balanceExpected.TotalQuantity, balance.TotalQuantity);
    }

    [Fact]
    public void CalculateSellOperation_ShouldThrowJsonException_WhenInvalidData()
    {
        // Arrange
        OperationsBalance balance = new() { Loss = 0.0M, AverageBuyingCost = 10.0M, TotalQuantity = 1000 };
        Operation operation = new(OperationType.sell, 15.0M, 2000);

        // Act
        var exception = Assert
            .Throws<InvalidOperationException>(() => _handler
            .CalculateSellOperation(operation, balance));

        // Assert
        Assert.Contains("Invalid operation: The quantity of shares being " +
            "sold cannot exceed the current quantity of shares owned.", exception.Message);
    }
}