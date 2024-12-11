using Newtonsoft.Json;
using CapitalGains.Application.Services.Consoles;
using CapitalGains.Core.Entities;
using CapitalGains.Core.Entities.Enums;
using CapitalGains.Core.Interfaces;
using System;

namespace CapitalGains.Test
{
    public class OperationConsoleTest ()
    {
        private readonly IOperationConsole _console = new OperationConsole();

        [Fact]
        public void ReadInput_ShouldReadInputWithSuccess_WhenValidData()
        {
            // Arrange
            var input = $"[{{\"operation\":\"buy\",\"unit-cost\":10.0,\"quantity\":100}}]{Environment.NewLine}" +
                        $"[{{\"operation\":\"buy\",\"unit-cost\":10.0,\"quantity\":100}}]{Environment.NewLine}" +
                        $"[{{\"operation\":\"buy\",\"unit-cost\":10.0,\"quantity\":100}}]";
            var operations = new List<List<Operation>>();

            var operationExpected = new Operation(OperationType.buy, 10.0, 100);
            var countExpected = 3;

            // Act
            using var stringReader = new StringReader(input);
            Console.SetIn(stringReader);
            _console.ReadInput(ref operations);

            // Assert
            Assert.Single(operations[0]);
            Assert.Equal(countExpected, operations.Count);
            Assert.Equal(operationExpected.OperationType, operations[0][0].OperationType);
            Assert.Equal(operationExpected.UnitCost, operations[0][0].UnitCost);
            Assert.Equal(operationExpected.Quantity, operations[0][0].Quantity);
        }

        [Fact]
        public void ReadInput_ShouldReturnNull_WhenNullInputOrEmpty()
        {
            // Arrange
            string? input = Environment.NewLine;
            List<List<Operation>> operations = [];

            // Act
            using var stringReader = new StringReader(input);
            Console.SetIn(stringReader);
            _console.ReadInput(ref operations);

            // Assert
            Assert.Empty(operations);
        }

        [Fact]
        public void ReadInput_ShouldThrowJsonException_WhenInvalidData()
        {
            // Arrange
            string? input = "{}";
            List<List<Operation>> operations = [];

            // Act
            using var stringReader = new StringReader(input);
            Console.SetIn(stringReader);
            var exception = Assert.Throws<JsonException>(() =>  _console.ReadInput(ref operations));

            // Assert
            Assert.Contains("Error to Deserialize Object", exception.Message);
        }

        [Fact]
        public void WriteOutput_ShouldWriteOutputWithSuccess_WhenValidData()
        {
            // Arrange
            var operations = new List<List<Operation>>
            {
                new() 
                {
                    new(OperationType.buy, 10.0, 100, 0)
                },
                new() 
                {
                    new(OperationType.buy, 20.0, 50, 0), 
                    new(OperationType.sell, 30.0, 50, 0.0)
                },
                new() 
                {
                    new(OperationType.buy, 10.0, 1000, 0), 
                    new(OperationType.sell, 50.0, 1000, 80000), 
                    new(OperationType.buy, 20.0, 1000, 0), 
                    new(OperationType.sell, 50.0, 1000, 60000) 
                }
            };

            var expectedOutput =
                $"[{{\"tax\":0.00}}]{Environment.NewLine}" +
                $"[{{\"tax\":0.00}},{{\"tax\":0.00}}]{Environment.NewLine}" +
                $"[{{\"tax\":0.00}},{{\"tax\":80000.00}},{{\"tax\":0.00}},{{\"tax\":60000.00}}]";

            // Act
            using var stringWriter = new StringWriter();
            var originalOutPut = Console.Out;
            Console.SetOut(stringWriter);
            _console.WriteOutput(ref operations);
            Console.SetOut(originalOutPut);

            // Assert
            var output = stringWriter.ToString().Trim();
            Assert.Equal(expectedOutput, output);
        }

        [Fact]
        public void WriteOutput_ShouldThrowJsonException_WhenInvalidData() 
        {
            // Arrange
            var operations = new List<List<Operation>> { null };

            // Act 
            var exception = Assert.Throws<JsonException>(() => _console.WriteOutput(ref operations));

            // Assert
            Assert.Contains("Error to Serialize Object", exception.Message);
        }
    }
}