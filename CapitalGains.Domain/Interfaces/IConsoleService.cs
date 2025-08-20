using CapitalGains.Domain.Entities;

namespace CapitalGains.Domain.Interfaces;

public interface IConsoleService
{
    List<List<Operation>> ReadIn(CancellationToken cancellationToken = default);
    void WriteOut(List<List<Operation>> operationsOutput, CancellationToken cancellationToken = default);
}
