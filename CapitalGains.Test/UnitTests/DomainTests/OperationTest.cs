using CapitalGains.Domain.Entities;
using CapitalGains.Domain.Entities.Enums;

namespace CapitalGains.Test.UnitTests.DomainTests;

public class OperationTest
{
    [Fact]
    public void Operation_ShouldCreateObject_WhenValidData()
    {
        // Arrange
        var operationTypeExpected = OperationType.sell;
        var unitCostExpected = 10.0M;
        var quantityExpected = 100;
        var taxExpected = 0.0M;

        // Act
        var operation = new Operation(OperationType.sell, 10.0M, 100);

        // Assert
        Assert.Equal(quantityExpected, operation.Quantity);
        Assert.Equal(unitCostExpected, operation.UnitCost);
        Assert.Equal(operationTypeExpected, operation.OperationType);
        Assert.Equal(taxExpected, operation.Tax);
    }

    [Fact]
    public void Operation_ShouldThrowArgumentException_WhenNegativeUnitCost()
    {
        // Arrange
        var operation = OperationType.sell;
        var unitCost = -10.0M;
        var quantity = 100;

        // Act
        var exception = Assert.Throws<ArgumentException>(() => new Operation(operation, unitCost, quantity));

        // Assert
        Assert.Contains("Unit Cost must be a number greater than 0.", exception.Message);
    }

    [Fact]
    public void Operation_ShouldThrowArgumentException_WhenNegativeQuantity() 
    { 
        // Arrange
        var operation = OperationType.sell;
        var unitCost = 10.0M;
        var quantity = -100;

        // Act
        var exception = Assert.Throws<ArgumentException>(() => new Operation(operation, unitCost, quantity));

        // Assert
        Assert.Contains("Quantity must be a number greater than 0.", exception.Message);
    }

    [Fact]
    public void Operation_ShouldThrowArgumentException_WhenNegativeTax()
    {
        // Arrange
        var operation = OperationType.sell;
        var unitCost = 10.0M;
        var quantity = 100;
        var tax = -20;

        // Act
        var exception = Assert.Throws<ArgumentException>(() => new Operation(operation, unitCost, quantity, tax));

        // Assert
        Assert.Contains("Tax must be a number greater or equal to 0.", exception.Message);
    }
}
