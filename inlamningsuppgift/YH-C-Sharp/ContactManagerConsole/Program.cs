using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ContactManagerConsole.Services;
using SharedLogic.Services;

var builder = Host.CreateDefaultBuilder().ConfigureServices(services => {
    services.AddSingleton<ContactService>();
    services.AddSingleton<MenuService>();
}).Build();

builder.Start();
Console.Clear();

builder.Services.GetRequiredService<MenuService>().StartMenuLoop();