using System.Reflection;
using System.Threading.Tasks;
using DustInTheWind.ConsoleTools.Commando.Hosting.Autofac.Demo.Commands;

namespace DustInTheWind.ConsoleTools.Commando.Hosting.Autofac.Demo;

internal class Program
{
    public static async Task Main(string[] args)
    {
        CommandoHost host = CommandoHost.CreateBuilder()
            .RegisterCommandsFrom(typeof(DummyCommand).Assembly)
            .Build();

        await host.RunAsync(args);
    }
}