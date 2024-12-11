using CapitalGains.Application.Services.Handlers;
using CapitalGains.Core.Entities;
using CapitalGains.Core.Entities.Enums;
using CapitalGains.Core.Interfaces;

namespace CapitalGains.Test
{
    public class OperationHandlerTest ()
    {
        private readonly IOperationHandler _handler = new OperationHandler();

        [Fact]
        public void HandleBuyOperation_ShouldHandleWithSuccess_WhenValidData()
        {
            // Arrange
            Operation operation =  new(OperationType.buy, 10.0, 100);
            OperationsBalance balance = new();

            OperationsBalance balanceExpected = new() { AverageBuyingCost = 10.0, ShareQuantity = 100};
            Operation operationExpected = new(OperationType.buy, 10.0, 100, 0.0);           

            // Act
            _handler.HandleBuyOperation(ref operation, ref balance);

            // Assert
            Assert.Equal(operationExpected.OperationType, operation.OperationType);
            Assert.Equal(operationExpected.Quantity, operation.Quantity);
            Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
            Assert.Equal(operationExpected.Tax, operation.Tax);
            Assert.Equal(balanceExpected.Loss, balance.Loss); 
            Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
            Assert.Equal(balanceExpected.ShareQuantity, balance.ShareQuantity);
        }

        [Fact]
        public void HandleSellOperation_ShouldHandleWithSuccess_WhenLoss()
        {
            // Arrange
            Operation operation = new(OperationType.sell, 5.0, 50);
            OperationsBalance balance = new() { Loss = 0.0, AverageBuyingCost = 10.0, ShareQuantity = 100 };

            Operation operationExpected = new(OperationType.sell, 5.0, 50, 0.0);
            OperationsBalance balanceExpected = new() { Loss = -250.0, AverageBuyingCost = 10.0, ShareQuantity = 50 };

            // Act
            _handler.HandleSellOperation(ref operation, ref balance);

            // Assert
            Assert.Equal(operationExpected.OperationType, operation.OperationType);
            Assert.Equal(operationExpected.Quantity, operation.Quantity);
            Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
            Assert.Equal(operationExpected.Tax, operation.Tax);
            Assert.Equal(balanceExpected.Loss, balance.Loss);
            Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
            Assert.Equal(balanceExpected.ShareQuantity, balance.ShareQuantity);
        }

        [Fact]
        public void HandleSellOperation_ShouldHandleWithSuccess_WhenGain()
        {
            // Arrange
            Operation operation = new(OperationType.sell, 25.0, 1000);
            OperationsBalance balance = new() { Loss = 0.0, AverageBuyingCost = 10.0, ShareQuantity = 2000 };

            Operation operationExpected = new(OperationType.sell, 25.0, 1000, 3000);
            OperationsBalance balanceExpected = new() { Loss = 0.0, AverageBuyingCost = 10.0, ShareQuantity = 1000 };

            // Act
            _handler.HandleSellOperation(ref operation, ref balance);

            // Assert
            Assert.Equal(operationExpected.OperationType, operation.OperationType);
            Assert.Equal(operationExpected.Quantity, operation.Quantity);
            Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
            Assert.Equal(operationExpected.Tax, operation.Tax);
            Assert.Equal(balanceExpected.Loss, balance.Loss);
            Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
            Assert.Equal(balanceExpected.ShareQuantity, balance.ShareQuantity);
        }

        [Fact]
        public void HandleSellOperation_ShouldHandleWithSuccess_WhenGainWithLossBefore()
        {
            // Arrange
            Operation operation = new(OperationType.sell, 25.0, 1000);
            OperationsBalance balance = new() { Loss = -20000.0, AverageBuyingCost = 10.0, ShareQuantity = 2000 };

            Operation operationExpected = new(OperationType.sell, 25.0, 1000, 0.0);
            OperationsBalance balanceExpected = new() { Loss = -5000.0, AverageBuyingCost = 10.0, ShareQuantity = 1000 };

            // Act
            _handler.HandleSellOperation(ref operation, ref balance);

            // Assert
            Assert.Equal(operationExpected.OperationType, operation.OperationType);
            Assert.Equal(operationExpected.Quantity, operation.Quantity);
            Assert.Equal(operationExpected.UnitCost, operation.UnitCost);
            Assert.Equal(operationExpected.Tax, operation.Tax);
            Assert.Equal(balanceExpected.Loss, balance.Loss);
            Assert.Equal(balanceExpected.AverageBuyingCost, balance.AverageBuyingCost);
            Assert.Equal(balanceExpected.ShareQuantity, balance.ShareQuantity);
        }

        [Fact]
        public void HandleSellOperation_ShouldThrowJsonException_WhenInvalidData()
        {
            // Arrange
            OperationsBalance balance = new() { Loss = 0.0, AverageBuyingCost = 10.0, ShareQuantity = 1000 };
            Operation operation = new(OperationType.sell, 15.0, 2000);

            // Act
            var exception = Assert
                .Throws<InvalidOperationException>(() => _handler
                .HandleSellOperation(ref operation, ref balance));

            // Assert
            Assert.Contains("Invalid operation: The quantity of shares being " +
                "sold cannot exceed the current quantity of shares owned.", exception.Message);
        }
    }
}

