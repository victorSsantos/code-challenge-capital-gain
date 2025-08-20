namespace CapitalGains.Application.CapitalGainApp.Interfaces;

public interface ICapitalGainApp
{
    void Process(CancellationToken token);
}