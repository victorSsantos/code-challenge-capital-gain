using CapitalGains.Domain.Entities;

namespace CapitalGains.Domain.Interfaces;

public interface IConsoleService
{
    void ReadInput(List<List<Operation>> operationsInput);
    void WriteOutput(List<List<Operation>> operationsOutput); 
}
