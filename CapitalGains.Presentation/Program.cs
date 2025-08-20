using CapitalGains.Domain.Interfaces;
using CapitalGains.Application.Services;

namespace CapitalGains.Presentation;

public class Program
{
    public static int Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddSingleton<ICalculationService, CalculationService>();
        builder.Services.AddSingleton<IConsoleService, ConsoleService>();
        builder.Services.AddSingleton<Client>();

        var host = builder.Build();

        using var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        try
        {
            var client = host.Services.GetRequiredService<Client>();
            client.ProcessOperations(cts.Token);
            return 0; 
        }
        catch (OperationCanceledException)
        {
            return 130; // (Ctrl+C)
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERRO] {ex.Message}");
            Console.Error.WriteLine(ex);
            return 1;
        }
    }
}