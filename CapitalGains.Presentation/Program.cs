using CapitalGains.Domain.Interfaces;
using CapitalGains.Application.CapitalGainApp.Services;
using CapitalGains.Application.CapitalGainApp.Interfaces;
using CapitalGains.Application.CapitalGainApp;
using CapitalGains.Presentation;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<Client>();
builder.Services.AddSingleton<ICapitalGainApp, CapitalGainApp>();
builder.Services.AddSingleton<IConsoleService, ConsoleService>();
builder.Services.AddSingleton<ICalculationService, CalculationService>();

using var host = builder.Build();
var client = host.Services.GetRequiredService<Client>();

return client.Run();