using CapitalGains.Application.CapitalGainApp.Interfaces;

namespace CapitalGains.Presentation;

public sealed class Client(ICapitalGainApp capitalGainApp)
{
    private const int ExitOk = 0;
    private const int ExitError = 1;
    private const int ExitCanceled = 130;

    private readonly ICapitalGainApp _capitalGainApp = capitalGainApp;

    public int Run()
    {
        using var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            if (!cts.IsCancellationRequested) cts.Cancel();
        };

        try
        {
            _capitalGainApp.Process(cts.Token);
            return ExitOk;
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            Console.Error.WriteLine("[WARN] Operation canceled.");
            return ExitCanceled;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR] {ex.Message}");
            return ExitError;
        }
    }
}
