using Newtonsoft.Json;
using CapitalGains.Application.DTOs.Converters;

namespace CapitalGains.Test.DTOs.Converters
{
    public class DoubleWithTwoDecimalsConverterTests
    {
        private readonly DoubleWithTwoDecimalsConverter _converter = new();

        [Fact]
        public void ReadJson_ShouldReadDoubleValue_WhenValidInput()
        {
            // Arrange
            var json = "123.456";
            var reader = new JsonTextReader(new StringReader(json));

            // Act
            reader.Read();
            var result = _converter.ReadJson(reader, typeof(double), 0.0, false, null);

            // Assert
            Assert.Equal(123.456, result);
        }

        [Fact]
        public void WriteJson_ShouldWriteFormattedDouble_WhenValidInput()
        {
            // Arrange
            var writer = new StringWriter();
            var jsonWriter = new JsonTextWriter(writer);
            var value = 123.456;

            // Act
            _converter.WriteJson(jsonWriter, value, null);

            // Assert
            jsonWriter.Flush();
            var result = writer.ToString();
            Assert.Equal("123.46", result);
        }
    }
}