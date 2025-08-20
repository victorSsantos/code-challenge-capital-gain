using Newtonsoft.Json;
using CapitalGains.Application.CapitalGainApp.Utils;

namespace CapitalGains.Test.ServicesTests;

public class DecimalFormatterTests
{
    private readonly DecimalFormatter _converter = new();

    [Fact]
    public void ReadJson_ShouldReadDecimalValue_WhenValidInput()
    {
        // Arrange
        var json = "123.456";
        var reader = new JsonTextReader(new StringReader(json));

        // Act
        reader.Read();
        var result = _converter.ReadJson(reader, typeof(double), 0.0M, false, new JsonSerializer());

        // Assert
        Assert.Equal(123.456M, result);
    }

    [Fact]
    public void WriteJson_ShouldWriteFormattedDecimal_WhenValidInput()
    {
        // Arrange
        var writer = new StringWriter();
        var jsonWriter = new JsonTextWriter(writer);
        var value = 123.456M;

        // Act
        _converter.WriteJson(jsonWriter, value, new JsonSerializer());

        // Assert
        jsonWriter.Flush();
        var result = writer.ToString();
        Assert.Equal("123.46", result);
    }
}