using CapitalGains.Core.Entities;

namespace CapitalGains.Core.Interfaces;

public interface IOperationConsole
{
    void ReadInput(ref List<List<Operation>> operationsInput);
    void WriteOutput(ref List<List<Operation>> operationsOutput); 
}
