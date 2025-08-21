using CapitalGains.Application.CapitalGainApp;
using CapitalGains.Application.CapitalGainApp.Services;
using CapitalGains.Domain.Interfaces;
using CapitalGains.Presentation;
using System.Text;

namespace CapitalGains.Test.IntegrationTest;

public class EndToEndTest ()
{
    private readonly ICalculationService _calc = new CalculationService();
    private readonly IConsoleService _console = new ConsoleService();

    [Fact]
    public void Process_SmallInput_ProducesExpectedOutput()
    {
        // arrange
        var input = ReadAsset("input_small.txt");
        var expectedOutput = ReadAsset("expected_small.txt");

        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        Console.SetIn(new StreamReader(inputStream));

        var outputString = new StringWriter();
        Console.SetOut(outputString);

        var app = new CapitalGainApp(_calc, _console);
        var client = new Client(app);

        // act
        var exitCode = client.Run();

        // assert
        Assert.Equal(0, exitCode);
        Assert.Equal(expectedOutput.Trim(), outputString.ToString().Trim());
    }

    [Fact]
    public void Process_LargeInput_ProducesExpectedOutput()
    {
        // arrange
        var input = ReadAsset("input_large.txt");
        var expectedOutput = ReadAsset("expected_large.txt");

        var inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
        Console.SetIn(new StreamReader(inputStream));

        using var outputString = new StringWriter();
        Console.SetOut(outputString);

        var app = new CapitalGainApp(_calc, _console);
        var client = new Client(app);

        // act
        var exitCode = client.Run();

        // assert
        Assert.Equal(0, exitCode);
        Assert.Equal(expectedOutput.Trim(), outputString.ToString().Trim());
    }

    private static string ReadAsset(string relativePath)
    {
        var baseDir = AppContext.BaseDirectory; 
        var fullPath = Path.Combine(baseDir, "TestAssets", relativePath);
        return File.ReadAllText(fullPath);
    }
}
