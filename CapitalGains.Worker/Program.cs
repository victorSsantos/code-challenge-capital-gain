using CapitalGains.Application.Services.Consoles;
using CapitalGains.Application.Services.Handlers;
using CapitalGains.Core.Interfaces;
using CapitalGains.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddTransient<IOperationHandler, OperationHandler>();
builder.Services.AddTransient<IOperationConsole, OperationConsole>();

var host = builder.Build();
host.Run();
