using CapitalGains.Application.Services.Consoles;
using CapitalGains.Application.Services.Handlers;
using CapitalGains.Processor.Processors;

try
{  
    var operationProcessor = new OperationProcessor(new OperationHandler(), new OperationConsole());
    operationProcessor.Execute();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
